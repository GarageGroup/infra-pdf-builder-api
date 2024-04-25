using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using GarageGroup.Infra;

namespace GarageGroup.Platform;

internal sealed partial class PdfApi(IHttpApi blobHttpApi, IHttpApi builderHttpApi, IUtcProvider utcProvider, PdfApiOption option) : IPdfApi
{
    private const string PingFailureMessage
        =
        "An unexpected http failure occured when trying to ping pdf builder:";

    private const string BlobFailureMessage
        =
        "An unexpected http failure occured when trying to check if file exists:";

    private const string BuilderFailureMessage
        =
        "An unexpected http failure occured when trying to build pdf document:";

    private const string CharSetUtf8 = "utf-8";

    private const string BlobResource = "b";

    private const string PermissionsRead = "r";

    private const string PermissionsUpload = "w";

    private const string Protocol = "https";

    private const string SasVersion = "2022-11-02";

    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

    private static readonly HttpSendIn PingInput;

    private static readonly JsonSerializerOptions SerializerOptions;

    static PdfApi()
    {
        PingInput = new(
            method: HttpVerb.Get,
            requestUri: "/healthCheck.php")
        {
            SuccessType = HttpSuccessType.OnlyStatusCode
        };

        SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }

    private PdfFile BuildPdfFile<T>(PdfGetIn<T> input)
        where T : notnull
    {
        var pdfBody = JsonSerializer.SerializeToUtf8Bytes(input.Data, SerializerOptions);
        var pdfHash = ComputeHash(pdfBody);

        var utcNow = utcProvider.UtcNow;

        var readerExpiryTime = (utcNow + option.Blob.BlobTokenTtl).ToString(DateTimeFormat);
        var builderExpiryTime = (utcNow + option.Blob.BuilderTokenTtl).ToString(DateTimeFormat);

        var blobPath = string.Join('/', [ input.BlobFolder, pdfHash, input.FileName ]);
        var blobFile = blobPath.Replace('\\', '/');

        using var hashAlgorithm = new HMACSHA256(Convert.FromBase64String(option.Blob.AccountKey));

        var readerSas = BuildSasToken(blobFile, hashAlgorithm, PermissionsRead, readerExpiryTime);
        var builderSas = BuildSasToken(blobFile, hashAlgorithm, PermissionsUpload, builderExpiryTime);

        var baseUrl = $"https://{option.Blob.AccountName}.blob.core.windows.net/{option.Blob.ContainerName}/{blobPath}";
        var builderUrl = $"{input.BuilderUrl}?fileUrl={HttpUtility.UrlEncode(baseUrl + builderSas)}";

        return new(baseUrl + readerSas, builderUrl, pdfBody);
    }

    private static string ComputeHash(byte[] pdfBody)
    {
        var hash = SHA256.HashData(pdfBody);
        var builder = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            builder = builder.Append(hash[i].ToString("x2"));
        }

        return builder.ToString();
    }

    private string BuildSasToken(string fileName, HMACSHA256 hashAlgorithm, string permissions, string expiryTime)
    {
        string[] signParameters =
        [
            permissions,
            string.Empty,
            expiryTime,
            $"/blob/{option.Blob.AccountName}/{option.Blob.ContainerName}/{fileName}",
            string.Empty,
            string.Empty,
            Protocol,
            SasVersion,
            BlobResource,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        ];

        var dataToSign = Encoding.UTF8.GetBytes(string.Join('\n', signParameters));
        var signature = Convert.ToBase64String(hashAlgorithm.ComputeHash(dataToSign));

        var escapedSignature = Uri.EscapeDataString(signature);
        return $"?sv={SasVersion}&spr={Protocol}&se={expiryTime}&sr={BlobResource}&sp={permissions}&sig={escapedSignature}";
    }

    private sealed record class PdfFile(string FileUrl, string BuilderUrl, byte[] PdfBody);
}