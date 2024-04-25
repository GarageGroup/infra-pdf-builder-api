using System;

namespace GarageGroup.Platform;

public sealed record class PdfBlobOption
{
    public PdfBlobOption(
        string accountKey,
        string accountName,
        string containerName,
        TimeSpan blobTokenTtl,
        TimeSpan builderTokenTtl)
    {
        AccountKey = accountKey.OrEmpty();
        AccountName = accountName.OrEmpty();
        ContainerName = containerName.OrEmpty();
        BlobTokenTtl = blobTokenTtl;
        BuilderTokenTtl = builderTokenTtl;
    }

    public string AccountKey { get; }

    public string AccountName { get; }

    public string ContainerName { get; }

    public TimeSpan BlobTokenTtl { get; }

    public TimeSpan BuilderTokenTtl { get; }
}