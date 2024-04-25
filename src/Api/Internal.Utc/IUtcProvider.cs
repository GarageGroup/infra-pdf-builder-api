using System;

namespace GarageGroup.Platform;

internal interface IUtcProvider
{
    DateTime UtcNow { get; }
}