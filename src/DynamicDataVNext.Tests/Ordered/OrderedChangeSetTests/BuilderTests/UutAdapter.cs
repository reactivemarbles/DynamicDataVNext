using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

public class UutAdapter
    : IUutAdapter<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>
{
    public static AndConstraint<ChangeSetAssertions<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>> AssertShouldBeValid(OrderedChangeSet<int> subject)
        => subject.Should().BeValid();

    public static OrderedChange<int> CreateAddition(int item)
        => OrderedChange.CreateInsertion(
            index:  0,
            item:   item);

    public static ChangeSetBuilderBase<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType> CreateUut(
            int initialCapacity,
            int sourceCount)
        => new OrderedChangeSet<int>.Builder(
            initialCapacity:    initialCapacity,
            sourceCount:        sourceCount);

    public static ChangeSetBuilderBase<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType> CreateUut(int sourceCount)
        => new OrderedChangeSet<int>.Builder(sourceCount);

    public static OrderedChange<int> CreateNone()
        => default;

    public static OrderedChange<int> CreateRemoval(
            int sourceCount,
            int item)
        => OrderedChange.CreateRemoval(
            index:  sourceCount - 1,
            item:   item);
}
