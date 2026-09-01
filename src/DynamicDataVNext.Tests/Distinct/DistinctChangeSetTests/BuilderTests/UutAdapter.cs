using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

public class UutAdapter
    : IUutAdapter<DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>
{
    public static AndConstraint<ChangeSetAssertions<DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>> AssertShouldBeValid(DistinctChangeSet<int> subject)
        => subject.Should().BeValid();

    public static DistinctChange<int> CreateAddition(int item)
        => DistinctChange.CreateAddition(item);

    public static ChangeSetBuilderBase<DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType> CreateUut(
            int initialCapacity,
            int sourceCount)
        => new DistinctChangeSet<int>.Builder(
            initialCapacity:    initialCapacity,
            sourceCount:        sourceCount);

    public static ChangeSetBuilderBase<DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType> CreateUut(int sourceCount)
        => new DistinctChangeSet<int>.Builder(sourceCount);

    public static DistinctChange<int> CreateNone()
        => default;

    public static DistinctChange<int> CreateRemoval(
            int sourceCount,
            int item)
        => DistinctChange.CreateRemoval(item);
}
