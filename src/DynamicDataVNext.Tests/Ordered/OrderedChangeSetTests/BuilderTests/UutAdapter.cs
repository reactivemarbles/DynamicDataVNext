using AwesomeAssertions;

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
            int     initialCapacity,
            bool    isSourceEmpty)
        => new OrderedChangeSet<int>.Builder(
            initialCapacity:    initialCapacity,
            isSourceEmpty:      isSourceEmpty);

    public static ChangeSetBuilderBase<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType> CreateUut(bool isSourceEmpty)
        => new OrderedChangeSet<int>.Builder(isSourceEmpty);

    public static OrderedChange<int> CreateNone()
        => default;

    public static OrderedChange<int> CreateRemoval(int item)
        => OrderedChange.CreateRemoval(
            index:  0,
            item:   item);
}
