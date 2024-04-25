using System;
using GarageGroup.Infra;

namespace GarageGroup.Platform;

public sealed class PdfHttp
{
    public static PdfHttp FromBlobAndBuildApi(IHttpApi blobApi, IHttpApi builderApi)
    {
        ArgumentNullException.ThrowIfNull(blobApi);
        ArgumentNullException.ThrowIfNull(builderApi);

        return new(blobApi: blobApi, builderApi: builderApi);
    }

    private PdfHttp(IHttpApi blobApi, IHttpApi builderApi)
    {
        BlobApi = blobApi;
        BuilderApi = builderApi;
    }

    public IHttpApi BlobApi { get; }

    public IHttpApi BuilderApi { get; }
}
