using System.Collections.Immutable;

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
        public Builder(bool isSourceEmpty)
            : base(isSourceEmpty)
        { }

        /// <inheritdoc/>
        public Builder(
            int     initialCapacity,
            bool    isSourceEmpty)
            : base(
                initialCapacity,
                isSourceEmpty)
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
