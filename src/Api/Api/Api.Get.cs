using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Platform;

partial class PdfApi
{
    public ValueTask<Result<PdfGetOut, Failure<Unit>>> GetAsync<TDataJson>(
        PdfGetIn<TDataJson> input, CancellationToken cancellationToken)
        where TDataJson : notnull
    {
        var pdfFile = BuildPdfFile(input);

        if (option.IsCacheDisabled)
        {
            return InnerBuildAsync(pdfFile, cancellationToken);
        }

        return InnerGetAsync(pdfFile, cancellationToken);
    }

    private ValueTask<Result<PdfGetOut, Failure<Unit>>> InnerGetAsync(
        PdfFile pdfFile, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            pdfFile, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(
                method: HttpVerb.Head,
                requestUri: @in.FileUrl)
            {
                SuccessType = HttpSuccessType.OnlyStatusCode
            })
        .PipeValue(
            blobHttpApi.SendAsync)
        .MapSuccess(
            _ => new PdfGetOut(pdfFile.FileUrl))
        .RecoverValue(
            (failure, token) => failure.StatusCode switch
            {
                HttpFailureCode.NotFound => InnerBuildAsync(pdfFile, token),
                _ => new(result: failure.ToStandardFailure(BlobFailureMessage).WithFailureCode<Unit>(default))
            });

    private ValueTask<Result<PdfGetOut, Failure<Unit>>> InnerBuildAsync(
        PdfFile pdfFile, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            pdfFile, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(
                method: HttpVerb.Post,
                requestUri: @in.BuilderUrl)
            {
                Body = new()
                {
                    Type = new(MediaTypeNames.Application.Json, CharSetUtf8),
                    Content = new(@in.PdfBody)
                },
                SuccessType = HttpSuccessType.OnlyStatusCode
            })
        .PipeValue(
            builderHttpApi.SendAsync)
        .Map(
            _ => new PdfGetOut(pdfFile.FileUrl),
            static failure => failure.ToStandardFailure(BuilderFailureMessage).WithFailureCode<Unit>(default));
}