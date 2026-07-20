using System.Collections.Immutable;

namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class IntersectWithMutation<TItem>
    : MutationBase<TItem>
{
    public required ImmutableArray<TItem> Other { get; init; }
    
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.IntersectWith(Other);
}
