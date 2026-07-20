namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class RemoveItemMutation<TItem>
    : MutationBase<TItem>
{
    public required TItem Item { get; init; } 
    
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.Remove(Item);
}
