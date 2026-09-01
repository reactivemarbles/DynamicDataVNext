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
