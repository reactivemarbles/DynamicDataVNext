namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class AddItemMutation<TItem>
    : MutationBase<TItem>
{
    public required TItem Item { get; init; } 
    
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.Add(Item);
}
