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

        protected override ChangeSetType OnChangeAdded(OrderedChange<T> change)
        {
            // List-land has an extra requirement for clears, that changes have to be listed in reverse order.
            // We could maybe make BuildAndClear() overridable and override it to reorder and rebuild clears properly,
            // but I doubt it'd be worth the performance cost. Maybe someday someone can benchmark it and see.
 
            if (Changes.Count is 1)
                _areRemovalsInReverseOrder = true;
        
            if (        (change.Type is not OrderedChangeType.Removal)
                    ||  (change.AsRemoval().Index != SourceCount))
                _areRemovalsInReverseOrder = false;

            var baseResult = base.OnChangeAdded(change);
            
            return (        (baseResult is ChangeSetType.Clear)
                        &&  !_areRemovalsInReverseOrder)
                ? ChangeSetType.Update
                : baseResult;
        }
        
        private bool _areRemovalsInReverseOrder;
    }
}
