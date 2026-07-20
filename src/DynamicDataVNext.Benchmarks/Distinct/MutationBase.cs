namespace DynamicDataVNext.Benchmarks.Distinct;

public abstract class MutationBase<TItem>
{
    public abstract void ApplyTo(ObservableHashSet<TItem> target);
}
