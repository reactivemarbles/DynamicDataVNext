using System.Collections.Immutable;

namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class ResetMutation<TItem>
    : MutationBase<TItem>
{
    public required ImmutableArray<TItem> Items { get; init; }
    
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.Reset(Items);
}
