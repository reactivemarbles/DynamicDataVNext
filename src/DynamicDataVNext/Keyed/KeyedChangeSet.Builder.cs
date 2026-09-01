namespace DynamicDataVNext;

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="KeyedChange{TKey, TItem}"/> values, over time, to be assembled into a <see cref="KeyedChangeSet{TKey, TItem}"/>, with correctness guarantees.
    /// </summary>
    public sealed class Builder
        : ChangeSetBuilderBase<KeyedChangeSet<TKey, TItem>, KeyedChange<TKey, TItem>, KeyedChangeType>
    {
        /// <inheritdoc/>
        public Builder(int sourceCount)
            : base(sourceCount)
        { }

        /// <inheritdoc/>
        public Builder(
            int initialCapacity,
            int sourceCount)
            : base(
                initialCapacity,
                sourceCount)
        { }

        protected override KeyedChangeSet<TKey, TItem> CreateChangeSet(
            ImmutableArray<KeyedChange<TKey, TItem>>    changes,
            ChangeSetType                               type,
            int                                         firstResetAdditionIndex)
            => new()
            {
                Changes            = changes,
                FirstAdditionIndex = firstResetAdditionIndex,
                Type               = type
            };
    }
}
