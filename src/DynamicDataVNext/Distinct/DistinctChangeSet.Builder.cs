using System.Collections.Immutable;

namespace DynamicDataVNext;

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="DistinctChange{T}"/> values, over time, to be assembled into a <see cref="DistinctChangeSet{T}"/>, with correctness guarantees.
    /// </summary>
    public sealed class Builder
        : ChangeSetBuilderBase<DistinctChangeSet<T>, DistinctChange<T>, DistinctChangeType>
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

        protected override DistinctChangeSet<T> CreateChangeSet(
            ImmutableArray<DistinctChange<T>>   changes,
            ChangeSetType                       type,
            int                                 firstResetAdditionIndex)
            => new()
            {
                Changes            = changes,
                FirstAdditionIndex = firstResetAdditionIndex,
                Type               = type
            };
    }
}
