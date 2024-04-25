using System;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiSource
{
    public static TheoryData<PdfApiOption, DateTime, PdfGetIn<object>, string> InputBlobUrlPdfGetTestData
        =>
        new()
        {
            {
                new(
                    blob: new(
                        accountKey: "bXlTZWNyZXRLZXkxMjM=",
                        accountName: "some-account",
                        containerName: "some-container",
                        blobTokenTtl: TimeSpan.FromMinutes(15),
                        builderTokenTtl: TimeSpan.FromHours(3)))
                {
                    IsCacheDisabled = false
                },
                new(2024, 02, 21, 12, 31, 55),
                new(
                    blobFolder: "some-folder",
                    fileName: "print-form-01221.pdf",
                    builderUrl: "/Form/index.php",
                    data: new
                    {
                        Lists = new object[]
                        {
                            new
                            {
                                Type = "Title",
                                Text = "Some text",
                                Footer = "Some footer"
                            },
                            new
                            {
                                Type = "Info",
                                Header = "Some header",
                                Text = "Another text",
                                Footer = (string?)null
                            }
                        },
                        Language = "zh"
                    }),
                "https://some-account.blob.core.windows.net/some-container/some-folder" +
                "/cd1e5e3db79c60944bcc6f00ce1ba3d719a0476c23ed6e6f8a914b570e86672e/print-form-01221.pdf" +
                "?sv=2022-11-02&spr=https&se=2024-02-21T12:46:55Z&sr=b&sp=r&sig=N6ANZ8mFM%2B6j0AG0LNFggKdnX%2Ff8RMSlV1x25VafD5U%3D"
            },
            {
                new(
                    blob: new(
                        accountKey: "U29tZSBzZWNyZXQga2V5",
                        accountName: "myAccount",
                        containerName: "pdfContainer",
                        blobTokenTtl: TimeSpan.FromHours(12),
                        builderTokenTtl: TimeSpan.FromMinutes(30)))
                {
                    IsCacheDisabled = false
                },
                new(2023, 11, 15, 03, 57, 41),
                new(
                    blobFolder: null!,
                    fileName: "some\\Form",
                    builderUrl: "methodName",
                    data: new
                    {
                        Name = "Some name",
                        Value = 271
                    }),
                "https://myAccount.blob.core.windows.net/pdfContainer/default" +
                "/8a6c8901890b1ce722488d34dcc2067308d6fba6018b89e4a2686fc75f3a9035/some\\Form" +
                "?sv=2022-11-02&spr=https&se=2023-11-15T15:57:41Z&sr=b&sp=r&sig=%2FjOJrwzwdHOZ3RVl%2BUPXp5MXakl09YOc5TtBoQ9tq%2BM%3D"
            },
            {
                new(
                    blob: new(
                        accountKey: "U29tZSBzZWNyZXQga2V5",
                        accountName: "some-account",
                        containerName: "pdfContainer",
                        blobTokenTtl: TimeSpan.FromSeconds(35),
                        builderTokenTtl: TimeSpan.FromMinutes(15)))
                {
                    IsCacheDisabled = false
                },
                new(2023, 11, 15, 03, 57, 41),
                new(
                    blobFolder: "some-pdf-folder",
                    fileName: new string(' ', 3),
                    builderUrl: "/Pdf/index.pxp",
                    data: new
                    {
                        Name = "Some name",
                        Value = 271
                    }),
                "https://some-account.blob.core.windows.net/pdfContainer/some-pdf-folder" +
                "/8a6c8901890b1ce722488d34dcc2067308d6fba6018b89e4a2686fc75f3a9035/print-form.pdf" +
                "?sv=2022-11-02&spr=https&se=2023-11-15T03:58:16Z&sr=b&sp=r&sig=8GTbysX97SC4qfol%2FA72%2BDodYHxNDlodYP7ZDKQ9N%2BQ%3D"
            }
        };
}