using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiTest
{
    [Fact]
    public static async Task GetAsync_CacheIsDisabled_ExpectBlobHttpApiCalledNever()
    {
        var mockBlobHttpApi = BuildHttpApi(SomeHttpNotFoundOutput);
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);

        var option = new PdfApiOption(
            blob: new(
                accountKey: "c29tZUFjY291bnRLZXk=",
                accountName: "someAccountName",
                containerName: "someContainerName",
                blobTokenTtl: TimeSpan.FromHours(1),
                builderTokenTtl: TimeSpan.FromMinutes(5)))
        {
            IsCacheDisabled = true
        };

        var utcProvider = BuildUtcProvider(SomeUtc);
        var api = new PdfApi(mockBlobHttpApi.Object, mockBuilderHttpApi.Object, utcProvider, option);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.GetAsync(SomePdfGetInput, cancellationToken);

        mockBlobHttpApi.Verify(static a => a.SendAsync(It.IsAny<HttpSendIn>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.InputBlobUrlPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_CacheIsNotDisabled_ExpectBlobHttpApiCalledOnce(
        PdfApiOption option, DateTime utcNow, PdfGetIn<object> input, string expectedBlobUrl)
    {
        var mockBlobHttpApi = BuildHttpApi(SomeHttpNotFoundOutput);
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);

        var utcProvider = BuildUtcProvider(utcNow);
        var api = new PdfApi(mockBlobHttpApi.Object, mockBuilderHttpApi.Object, utcProvider, option);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.GetAsync(input, cancellationToken);

        var expectedInput = new HttpSendIn(
            method: HttpVerb.Head,
            requestUri: expectedBlobUrl)
        {
            SuccessType = HttpSuccessType.OnlyStatusCode
        };

        mockBlobHttpApi.Verify(a => a.SendAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.InputBlobUrlPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_BlobHttpResultIsSuccess_ExpectSuccess(
        PdfApiOption option, DateTime utcNow, PdfGetIn<object> input, string expectedBlobUrl)
    {
        var mockBlobHttpApi = BuildHttpApi(SomeSuccessOutput);
        var utcProvider = BuildUtcProvider(utcNow);

        var api = new PdfApi(mockBlobHttpApi.Object, Mock.Of<IHttpApi>(), utcProvider, option);

        var actual = await api.GetAsync(input, default);
        var expected = new PdfGetOut(expectedBlobUrl);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.UnexpectedFailureHttpBlobPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_BlobHttpResultIsUnexpectedFailure_ExpectFailure(
        HttpSendFailure blobHttpFailure, Failure<Unit> expected)
    {
        var mockBlobHttpApi = BuildHttpApi(blobHttpFailure);
        var utcProvider = BuildUtcProvider(SomeUtc);

        var api = new PdfApi(mockBlobHttpApi.Object, Mock.Of<IHttpApi>(), utcProvider, SomeOption);
        var actual = await api.GetAsync(SomePdfGetInput, default);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.InputBuilderPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_BlobHttpResultIsNotFoundFailure_ExpectBuilderHttpApiCalledOnce(
        PdfApiOption option, DateTime utcNow, PdfGetIn<object> input, HttpSendIn expectedInput)
    {
        var mockBlobHttpApi = BuildHttpApi(SomeHttpNotFoundOutput);
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);

        var utcProvider = BuildUtcProvider(utcNow);
        var api = new PdfApi(mockBlobHttpApi.Object, mockBuilderHttpApi.Object, utcProvider, option);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.GetAsync(input, cancellationToken);

        mockBuilderHttpApi.Verify(a => a.SendAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.FailureHttpBuilderPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_BuilderHttpResultIsFailure_ExpectFailure(
        HttpSendFailure builderHttpFailure, Failure<Unit> expected)
    {
        var mockBlobHttpApi = BuildHttpApi(SomeHttpNotFoundOutput);
        var mockBuilderHttpApi = BuildHttpApi(builderHttpFailure);

        var utcProvider = BuildUtcProvider(SomeUtc);
        var api = new PdfApi(mockBlobHttpApi.Object, mockBuilderHttpApi.Object, utcProvider, SomeOption);

        var actual = await api.GetAsync(SomePdfGetInput, default);
        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.InputBlobUrlPdfGetTestData), MemberType = typeof(PdfApiSource))]
    public static async Task GetAsync_BuilderHttpResultIsSuccess_ExpectSuccess(
        PdfApiOption option, DateTime utcNow, PdfGetIn<object> input, string expectedBlobUrl)
    {
        var mockBlobHttpApi = BuildHttpApi(SomeHttpNotFoundOutput);
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);

        var utcProvider = BuildUtcProvider(utcNow);
        var api = new PdfApi(mockBlobHttpApi.Object, mockBuilderHttpApi.Object, utcProvider, option);

        var actual = await api.GetAsync(input, default);
        var expected = new PdfGetOut(expectedBlobUrl);

        Assert.StrictEqual(expected, actual);
    }
}