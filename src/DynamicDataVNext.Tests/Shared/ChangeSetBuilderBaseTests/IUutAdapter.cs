namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public interface IUutAdapter<TChangeSet, TChange, TChangeType>
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    static abstract AndConstraint<ChangeSetAssertions<TChangeSet, TChange, TChangeType>> AssertShouldBeValid(TChangeSet subject);

    static abstract TChange CreateAddition(int item); 

    static abstract ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> CreateUut(
        int initialCapacity,
        int sourceCount);

    static abstract ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> CreateUut(int sourceCount);

    static abstract TChange CreateNone(); 

    static abstract TChange CreateRemoval(
        int sourceCount,
        int item);
}
