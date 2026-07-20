namespace DynamicDataVNext.Benchmarks.Distinct;

public sealed class ClearMutation<TItem>
    : MutationBase<TItem>
{
    public override void ApplyTo(ObservableHashSet<TItem> target)
        => target.Clear();
}
