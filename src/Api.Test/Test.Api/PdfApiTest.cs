using System;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Platform.Pdf.Api.Test;

public static partial class PdfApiTest
{
    private static readonly PdfApiOption SomeOption
        =
        new(
            blob: new(
                accountKey: "c29tZUFjY291bnRLZXk=",
                accountName: "someAccountName",
                containerName: "someContainerName",
                blobTokenTtl: TimeSpan.FromHours(1),
                builderTokenTtl: TimeSpan.FromMinutes(5)))
        {
            IsCacheDisabled = false
        };

    private static readonly PdfGetIn<object> SomePdfGetInput
        =
        new(
            blobFolder: "some-folder",
            fileName: "some.pdf",
            builderUrl: "/SomePrintForm/index.php",
            data: new
            {
                Text = "Some text",
                Price = 7900.5m
            });

    private static readonly DateTime SomeUtc
        =
        new(2024, 02, 15, 14, 03, 57);

    private static readonly HttpSendOut SomeSuccessOutput
        =
        new()
        {
            StatusCode = HttpSuccessCode.OK
        };

    private static readonly HttpSendFailure SomeHttpNotFoundOutput
        =
        new()
        {
            StatusCode = HttpFailureCode.NotFound
        };

    private static IUtcProvider BuildUtcProvider(DateTime utcNow)
        =>
        Mock.Of<IUtcProvider>(
            p => p.UtcNow == utcNow);

    private static Mock<IHttpApi> BuildHttpApi(in Result<HttpSendOut, HttpSendFailure> result)
    {
        var mock = new Mock<IHttpApi>();

        _ = mock.Setup(static a => a.SendAsync(It.IsAny<HttpSendIn>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        return mock;
    }
}
