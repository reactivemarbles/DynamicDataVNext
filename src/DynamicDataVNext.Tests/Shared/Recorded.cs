using System;
using System.Globalization;

namespace DynamicDataVNext.Tests;

public readonly record struct Recorded<T>
    : IEquatable<Recorded<T>>
{
    public required long Time { get; init; }
    
    public required T Value { get; init; }

    public override string ToString()
        => $"{Value}@{Time.ToString(CultureInfo.CurrentCulture)}";
}
