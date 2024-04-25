using System;

namespace GarageGroup.Platform;

public sealed record class PdfGetOut
{
    public PdfGetOut(string fileUrl)
        =>
        FileUrl = fileUrl.OrEmpty();

    public string FileUrl { get; }
}