using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Platform;

public interface IPdfGetSupplier
{
    ValueTask<Result<PdfGetOut, Failure<Unit>>> GetAsync<TDataJson>(PdfGetIn<TDataJson> input, CancellationToken cancellationToken)
        where TDataJson : notnull;
}