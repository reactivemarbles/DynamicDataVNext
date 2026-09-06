namespace DynamicDataVNext.Benchmarks.Ordered;

public sealed class MoveMutation<TTarget>
        : MutationBase<TTarget>
    where TTarget : IMovementAwareList
{
    public required int OldIndex { get; init; }
    
    public required int NewIndex { get; init; }

    public override void ApplyTo(TTarget target)
        => target.Move(
            oldIndex: OldIndex,
            newIndex: NewIndex);
}
