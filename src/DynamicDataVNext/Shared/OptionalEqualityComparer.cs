namespace DynamicDataVNext;

// Placeholder. Will be replaced by ReactiveUI.Primitives
public class OptionalEqualityComparer<T>
    : IEqualityComparer<Optional<T>>
{
    public OptionalEqualityComparer(IEqualityComparer<T> valueComparer)
        => _valueComparer = valueComparer;
    
    public bool Equals(Optional<T> x, Optional<T> y)
        => (x.IsSpecified, y.IsSpecified) switch
        {
            (false, false)  => true,
            (true, true)    => _valueComparer.Equals(x.Value, y.Value),
            _               => false
        };

    public int GetHashCode(Optional<T> obj)
        => (obj is { IsSpecified: true, Value: { } value })
            ? _valueComparer.GetHashCode(value)
            : 0;

    private readonly IEqualityComparer<T> _valueComparer;
}
