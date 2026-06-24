namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <summary>
    /// Retrieve a copy of the empty changeset.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of items in the collection.</typeparam>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    /// <returns>A copy of the empty changeset.</returns>
    public static KeyedChangeSet<TKey, TItem> Empty<TKey, TItem>()
        => KeyedChangeSet<TKey, TItem>.Empty;
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// A copy of the empty changeset.
    /// </summary>
    public static readonly KeyedChangeSet<TKey, TItem> Empty
        = default;
}
