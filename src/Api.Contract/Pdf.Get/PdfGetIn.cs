using System;

namespace GarageGroup.Platform;

public sealed record class PdfGetIn<TDataJson>
    where TDataJson : notnull
{
    public PdfGetIn(string blobFolder, string fileName, string builderUrl, TDataJson data)
    {
        BlobFolder = blobFolder.OrNullIfWhiteSpace() ?? "default";
        FileName = fileName.OrNullIfWhiteSpace() ?? "print-form.pdf";
        BuilderUrl = builderUrl.OrEmpty();
        Data = data;
    }

    public string BlobFolder { get; }

    public string FileName { get; }

    public string BuilderUrl { get; }

    public TDataJson Data { get; }
}