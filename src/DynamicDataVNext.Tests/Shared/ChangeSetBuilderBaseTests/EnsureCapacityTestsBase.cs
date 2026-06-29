using System;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public class EnsureCapacityTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : EnsureCapacityTestsBase<ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    protected override ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> CreateUut(int initialCapacity)
        => TUutAdapter.CreateUut(
            initialCapacity:    initialCapacity,
            isSourceEmpty:      false);

    protected override void EnsureCapacity(
            ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>  uut,
            int                                                     capacity)
        => uut.Changes.EnsureCapacity(capacity);

    protected override int GetCapacity(ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> uut)
        => uut.Changes.Capacity;
}
