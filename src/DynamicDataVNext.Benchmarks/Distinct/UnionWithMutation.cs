using System.Collections.Immutable;

namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class UnionWithMutation<TItem>
    : MutationBase<TItem>
{
    public required ImmutableArray<TItem> Other { get; init; }
    
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.UnionWith(Other);
}
