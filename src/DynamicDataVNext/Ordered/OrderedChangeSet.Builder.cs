using System.Collections.Immutable;

namespace DynamicDataVNext;

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// An object capable of efficiently collecting individual <see cref="OrderedChange{T}"/> values, over time, to be assembled into a <see cref="OrderedChangeSet{T}"/>, with correctness guarantees.
    /// </summary>
    public sealed class Builder
        : ChangeSetBuilderBase<OrderedChangeSet<T>, OrderedChange<T>, OrderedChangeType>
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

        protected override OrderedChangeSet<T> CreateChangeSet(
            ImmutableArray<OrderedChange<T>>    changes,
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
