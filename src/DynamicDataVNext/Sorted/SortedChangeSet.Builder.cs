using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class SortedChangeSet
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="SortedChange{T}"/> values, over time, to be assembled into a <see cref="SortedChangeSet{T}"/>, with correctness guarantees.
    /// </summary>
    /// <typeparam name="T">The type of the items in the source collection.</typeparam>
    public class Builder<T>
        : ChangeSetBuilderBase<SortedChange<T>, SortedChangeSet<T>>
    {
        /// <inheritdoc />
        public Builder()
            : base()
        { }

        /// <inheritdoc />
        public Builder(int initialCapacity)
            : base(initialCapacity)
        { }

        protected override SortedChangeSet<T> Empty
            => default;

        protected override SortedChangeSet<T> CreateChangeSet(
                ImmutableArray<SortedChange<T>> changes,
                ChangeSetType                   type)
            => new()
            {
                Changes = changes,
                Type    = type
            };

        protected override bool IsAddition(SortedChange<T> change)
            => change.Type is SortedChangeType.Insertion;

        protected override bool IsRemoval(SortedChange<T> change)
            => change.Type is SortedChangeType.Removal;
    }
}
