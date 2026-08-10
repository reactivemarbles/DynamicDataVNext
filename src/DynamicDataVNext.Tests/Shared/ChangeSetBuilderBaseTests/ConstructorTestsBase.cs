namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract class ConstructorTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : Base
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    [TestCase(0,    true,   TestName = "{m}(Empty capacity)")]
    [TestCase(0,    false,  TestName = "{m}(Populated source)")]
    [TestCase(1,    true,   TestName = "{m}(Trivial capacity)")]
    [TestCase(10,   true,   TestName = "{m}(Non-trivial capacity)")]
    public void WhenCapacityIsGiven_ResultIsEmpty(
        int     initialCapacity,
        bool    isSourceEmpty)
    {
        var result = TUutAdapter.CreateUut(
            initialCapacity:    initialCapacity,
            isSourceEmpty:      isSourceEmpty);
        
        result.Changes.Capacity.Should().Be(initialCapacity);
        result.Changes.Count.Should().Be(0);
        result.CurrentType.Should().Be(ChangeSetType.Empty);
        result.IsSourceEmpty.Should().Be(isSourceEmpty);
    }

    [TestCaseSource(nameof(IsSourceEmpty_TestCases))]
    public void WhenCapacityIsNotGiven_ResultIsEmpty(bool isSourceEmpty)
    {
        var result = TUutAdapter.CreateUut(
            isSourceEmpty:  isSourceEmpty);
        
        result.Changes.Capacity.Should().BePositive();
        result.Changes.Count.Should().Be(0);
        result.CurrentType.Should().Be(ChangeSetType.Empty);
        result.IsSourceEmpty.Should().Be(isSourceEmpty);
    }
}
