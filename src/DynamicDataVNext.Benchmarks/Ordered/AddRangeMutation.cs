namespace DynamicDataVNext.Benchmarks.Ordered;

public sealed class AddRangeMutation<TTarget, TItem>
        : MutationBase<TTarget>
    where TTarget : IRangeAwareList<TItem>
{
    public required ImmutableArray<TItem> Items { get; init; }

    public override void ApplyTo(TTarget target)
        => target.AddRange(Items);
}
