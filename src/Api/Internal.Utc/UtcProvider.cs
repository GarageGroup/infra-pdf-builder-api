using System;

namespace GarageGroup.Platform;

internal sealed class UtcProvider : IUtcProvider
{
    public static readonly UtcProvider Instance;

    static UtcProvider()
        =>
        Instance = new();

    private UtcProvider()
    {
    }

    public DateTime UtcNow
        =>
        DateTime.UtcNow;
}