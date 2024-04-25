using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiSource
{
    public static TheoryData<PdfApiOption, DateTime, PdfGetIn<object>, HttpSendIn> InputBuilderPdfGetTestData
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
                        builderTokenTtl: TimeSpan.FromMinutes(5)))
                {
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
                new(
                    method: HttpVerb.Post,
                    requestUri: "/Form/index.php?fileUrl=https%3a%2f%2fsome-account.blob.core.windows.net%2fsome-container%2fsome-folder" +
                        "%2fcd1e5e3db79c60944bcc6f00ce1ba3d719a0476c23ed6e6f8a914b570e86672e%2fprint-form-01221.pdf" +
                        "%3fsv%3d2022-11-02%26spr%3dhttps%26se%3d2024-02-21T12%3a36%3a55Z%26sr%3db%26sp%3dw" +
                        "%26sig%3dybkCDrYXJ70uxehZgm7KKUOKdx5l0RtV3xSh2l1pFIU%253D")
                {
                    Body = HttpBody.SerializeAsJson(
                        value: new
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
                    SuccessType = HttpSuccessType.OnlyStatusCode
                }
            },
            {
                new(
                    blob: new(
                        accountKey: "U29tZSBzZWNyZXQga2V5",
                        accountName: "myAccount",
                        containerName: "pdfContainer",
                        blobTokenTtl: TimeSpan.FromHours(12),
                        builderTokenTtl: TimeSpan.FromSeconds(25)))
                {
                },
                new(2023, 11, 15, 03, 57, 41),
                new(
                    blobFolder: new string(' ', 1),
                    fileName: "some\\Form",
                    builderUrl: null!,
                    data: new
                    {
                        Name = new
                        {
                            Type = "Text",
                            Text = "Some text",
                            Value = (string?)null
                        },
                        Value = 271
                    }),
                new(
                    method: HttpVerb.Post,
                    requestUri: "?fileUrl=https%3a%2f%2fmyAccount.blob.core.windows.net%2fpdfContainer%2fdefault" +
                        "%2f39eeb5c24c9aae335ba3ee13203c71da1b6b45983a5779645ca31e4d51ba93c8%2fsome%5cForm" +
                        "%3fsv%3d2022-11-02%26spr%3dhttps%26se%3d2023-11-15T03%3a58%3a06Z%26sr%3db%26sp%3dw" + 
                        "%26sig%3dkTCOd5fzMymk4W8YK16QqkY%252B1lDi2vDmCzuTT%252FmYx6k%253D")
                {
                    Body = HttpBody.SerializeAsJson(
                        value: new
                        {
                            Name = new
                            {
                                Type = "Text",
                                Text = "Some text",
                                Value = (string?)null
                            },
                            Value = 271
                        }),
                    SuccessType = HttpSuccessType.OnlyStatusCode
                }
            },
            {
                new(
                    blob: new(
                        accountKey: "U29tZSBzZWNyZXQga2V5",
                        accountName: "some-account",
                        containerName: "pdfContainer",
                        blobTokenTtl: TimeSpan.FromSeconds(35),
                        builderTokenTtl: TimeSpan.FromHours(3)))
                {
                },
                new(2023, 11, 15, 03, 57, 41),
                new(
                    blobFolder: "some-pdf-folder",
                    fileName: null!,
                    builderUrl: "/Pdf/index.pxp",
                    data: new
                    {
                        Name = "Some name",
                        Value = 271
                    }),
                new(
                    method: HttpVerb.Post,
                    requestUri: "/Pdf/index.pxp?fileUrl=https%3a%2f%2fsome-account.blob.core.windows.net%2fpdfContainer%2fsome-pdf-folder" +
                        "%2f8a6c8901890b1ce722488d34dcc2067308d6fba6018b89e4a2686fc75f3a9035%2fprint-form.pdf" +
                        "%3fsv%3d2022-11-02%26spr%3dhttps%26se%3d2023-11-15T06%3a57%3a41Z%26sr%3db%26sp%3dw" +
                        "%26sig%3dbdtsaYGjYThjJBUk%252BWroKGDqsZPb3LhMxKFA1tJrt1I%253D")
                {
                    Body = HttpBody.SerializeAsJson(
                        value: new
                        {
                            Name = "Some name",
                            Value = 271
                        }),
                    SuccessType = HttpSuccessType.OnlyStatusCode
                }
            }
        };
}