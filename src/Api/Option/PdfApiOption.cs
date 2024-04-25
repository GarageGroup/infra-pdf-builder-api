namespace GarageGroup.Platform;

public sealed record class PdfApiOption
{
    public PdfApiOption(PdfBlobOption blob)
        =>
        Blob = blob;

    public PdfBlobOption Blob { get; }

    public bool IsCacheDisabled { get; init; }
}