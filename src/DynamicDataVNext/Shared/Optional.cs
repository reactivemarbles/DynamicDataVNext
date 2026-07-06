using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

// Placeholder. Will be replaced by ReactiveUI.Primitives
public static class Optional
{
    public static Optional<T> FromValue<T>(T value)
        => Optional<T>.FromValue(value);

    public static Optional<T> Unspecified<T>()
        => Optional<T>.Unspecified;
}

// Placeholder. Will be replaced by ReactiveUI.Primitives
public readonly struct Optional<T>
{
    public static Optional<T> Unspecified
        => default;
    
    public static Optional<T> FromValue(T value)
        => new(
            isSpecified:    true,
            value:          value);

    private Optional(
        bool    isSpecified,
        T       value)
    {
        _isSpecified    = true;
        _value          = value;
    }
    
    public bool IsSpecified 
        => _isSpecified;
    
    public T Value
        => _value ?? throw new InvalidOperationException($"Invalid attempt to read an unspecified {nameof(Optional<>)} value");

    public bool Equals(Optional<T> other)
        =>      (_isSpecified == other._isSpecified)
            &&  EqualityComparer<T>.Default.Equals(_value, other._value);

    public override int GetHashCode()
        => _isSpecified
            ? _value?.GetHashCode() ?? 0
            : 0;

    public static implicit operator Optional<T>(T value)
        => FromValue(value);
    
    private readonly bool   _isSpecified;
    private readonly T      _value;
}
