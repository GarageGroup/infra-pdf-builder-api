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
    public static async Task PingAsync_ExpectBuilderHttpApiCalledOnce()
    {
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);
        var api = new PdfApi(Mock.Of<IHttpApi>(), mockBuilderHttpApi.Object, Mock.Of<IUtcProvider>(), SomeOption);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await api.PingAsync(default, cancellationToken);

        var expectedInput = new HttpSendIn(
            method: HttpVerb.Get,
            requestUri: "/healthCheck.php")
        {
            SuccessType = HttpSuccessType.OnlyStatusCode
        };

        mockBuilderHttpApi.Verify(a => a.SendAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [MemberData(nameof(PdfApiSource.FailurePingTestData), MemberType = typeof(PdfApiSource))]
    public static async Task PingAsync_BuilderHttpResultIsFailure_ExpectFailure(
        HttpSendFailure builderHttpFailure, Failure<Unit> expected)
    {
        var mockBuilderHttpApi = BuildHttpApi(builderHttpFailure);
        var api = new PdfApi(Mock.Of<IHttpApi>(), mockBuilderHttpApi.Object, Mock.Of<IUtcProvider>(), SomeOption);

        var actual = await api.PingAsync(default, default);
        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task PingAsync_BuilderHttpResultIsSuccess_ExpectSuccess()
    {
        var mockBuilderHttpApi = BuildHttpApi(SomeSuccessOutput);
        var api = new PdfApi(Mock.Of<IHttpApi>(), mockBuilderHttpApi.Object, Mock.Of<IUtcProvider>(), SomeOption);

        var actual = await api.PingAsync(default, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}