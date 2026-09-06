namespace DynamicDataVNext.Benchmarks.Ordered;

public sealed class InsertRangeMutation<TTarget, TItem>
        : MutationBase<TTarget>
    where TTarget : IRangeAwareList<TItem>
{
    public required int Index { get; init; }

    public required ImmutableArray<TItem> Items { get; init; }

    public override void ApplyTo(TTarget target)
        => target.InsertRange(
            index:  Index,
            items:  Items);
}
