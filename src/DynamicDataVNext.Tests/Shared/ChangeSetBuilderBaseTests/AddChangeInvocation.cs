using System;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public readonly struct AddChangeInvocation<TChange, TChangeType>
    where TChange : IChange<TChangeType>
    where TChangeType : Enum
{
    public required TChange Change { get; init; }

    public bool IsSourceEmpty { get; init; }
}
