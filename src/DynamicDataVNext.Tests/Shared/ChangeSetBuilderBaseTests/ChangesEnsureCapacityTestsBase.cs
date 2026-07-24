using System;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public class ChangesEnsureCapacityTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : EnsureCapacityTestsBase<ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.ChangeCollection>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    protected override ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.ChangeCollection CreateUut(int initialCapacity)
        => TUutAdapter.CreateUut(
                initialCapacity:    initialCapacity,
                isSourceEmpty:      false)
            .Changes;
}
