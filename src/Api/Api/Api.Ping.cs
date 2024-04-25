using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Platform;

partial class PdfApi
{
    public ValueTask<Result<Unit, Failure<Unit>>> PingAsync(Unit _, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            PingInput, cancellationToken)
        .PipeValue(
            builderHttpApi.SendAsync)
        .Map(
            Unit.From,
            static failure => failure.ToStandardFailure(PingFailureMessage).WithFailureCode<Unit>(default));
}