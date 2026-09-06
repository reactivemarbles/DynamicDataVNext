namespace DynamicDataVNext.Benchmarks.Ordered;

public abstract class MutationBase<TTarget>
{
    public abstract void ApplyTo(TTarget target);
}
