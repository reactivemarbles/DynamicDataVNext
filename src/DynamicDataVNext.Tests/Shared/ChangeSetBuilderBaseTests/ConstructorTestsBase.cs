namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract class ConstructorTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    [TestCase(0,    0, TestName = "{m}(Empty capacity)")]
    [TestCase(0,    1, TestName = "{m}(Single item in source)")]
    [TestCase(0,    3, TestName = "{m}(Multiple items in source)")]
    [TestCase(1,    0, TestName = "{m}(Trivial capacity)")]
    [TestCase(10,   0, TestName = "{m}(Non-trivial capacity)")]
    public void WhenCapacityIsGiven_ResultIsEmpty(
        int initialCapacity,
        int sourceCount)
    {
        var result = TUutAdapter.CreateUut(
            initialCapacity:    initialCapacity,
            sourceCount:        sourceCount);
        
        result.Changes.Capacity.Should().Be(initialCapacity);
        result.Changes.Count.Should().Be(0);
        result.CurrentType.Should().Be(ChangeSetType.Empty);
        result.SourceCount.Should().Be(sourceCount);
    }

    [TestCase(0, TestName = "{m}(Empty source)")]
    [TestCase(1, TestName = "{m}(Single item in source)")]
    [TestCase(3, TestName = "{m}(Multiple items in source)")]
    public void WhenCapacityIsNotGiven_ResultIsEmpty(int sourceCount)
    {
        var result = TUutAdapter.CreateUut(sourceCount: sourceCount);
        
        result.Changes.Capacity.Should().BePositive();
        result.Changes.Count.Should().Be(0);
        result.CurrentType.Should().Be(ChangeSetType.Empty);
        result.SourceCount.Should().Be(sourceCount);
    }
}
