using System;
using System.Runtime.CompilerServices;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Platform.Pdf.Api.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Platform;

public static class PdfApiDependency
{
    public static Dependency<IPdfApi> UsePdfApi(this Dependency<PdfHttp, PdfApiOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IPdfApi>(CreateApi);

        static PdfApi CreateApi(PdfHttp pdfHttp, PdfApiOption option)
        {
            ArgumentNullException.ThrowIfNull(pdfHttp);
            ArgumentNullException.ThrowIfNull(option);

            return new(
                blobHttpApi: pdfHttp.BlobApi,
                builderHttpApi: pdfHttp.BuilderApi,
                utcProvider: UtcProvider.Instance,
                option: option);
        }
    }
}