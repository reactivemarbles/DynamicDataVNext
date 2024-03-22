using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="KeyedChange{TKey, TItem}"/> values, over time, to be assembled into a <see cref="KeyedChangeSet{TKey, TItem}"/>, with correctness guarantees.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of items in the source collection.</typeparam>
    /// <typeparam name="TItem">The type of the items in the source collection.</typeparam>
    public sealed class Builder<TKey, TItem>
        : ChangeSetBuilderBase<KeyedChange<TKey, TItem>, KeyedChangeSet<TKey, TItem>>
    {
        /// <inheritdoc />
        public Builder()
            : base()
        { }

        /// <inheritdoc />
        public Builder(int initialCapacity)
            : base(initialCapacity)
        { }

        protected override KeyedChangeSet<TKey, TItem> Empty
            => default;

        protected override KeyedChangeSet<TKey, TItem> CreateChangeSet(
                ImmutableArray<KeyedChange<TKey, TItem>>    changes,
                ChangeSetType                               type)
            => new()
            {
                Changes = changes,
                Type    = type
            };

        protected override bool IsAddition(KeyedChange<TKey, TItem> change)
            => change.Type is KeyedChangeType.Addition;

        protected override bool IsRemoval(KeyedChange<TKey, TItem> change)
            => change.Type is KeyedChangeType.Removal;
    }
}
