using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="DistinctChange{T}"/> values, over time, to be assembled into a <see cref="DistinctChangeSet{T}"/>, with correctness guarantees.
    /// </summary>
    /// <typeparam name="T">The type of the items in the source collection.</typeparam>
    public class Builder<T>
        : ChangeSetBuilderBase<DistinctChange<T>, DistinctChangeSet<T>>
    {
        /// <inheritdoc />
        public Builder()
            : base()
        { }

        /// <inheritdoc />
        public Builder(int initialCapacity)
            : base(initialCapacity)
        { }

        protected override DistinctChangeSet<T> Empty
            => default;

        protected override DistinctChangeSet<T> CreateChangeSet(
                ImmutableArray<DistinctChange<T>> changes,
                ChangeSetType                   type)
            => new()
            {
                Changes = changes,
                Type    = type
            };

        protected override bool IsAddition(DistinctChange<T> change)
            => change.Type is DistinctChangeType.Addition;

        protected override bool IsRemoval(DistinctChange<T> change)
            => change.Type is DistinctChangeType.Removal;
    }
}
