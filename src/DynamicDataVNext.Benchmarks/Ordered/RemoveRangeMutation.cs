namespace DynamicDataVNext.Benchmarks.Ordered;

public sealed class RemoveRangeMutation<TTarget, TItem>
        : MutationBase<TTarget>
    where TTarget : IRangeAwareList<TItem>
{
    public required int Index { get; init; }
    
    public required int Count { get; init; }

    public override void ApplyTo(TTarget target)
        => target.RemoveRange(
            index:  Index,
            count:  Count);
}
