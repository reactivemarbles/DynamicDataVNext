using AwesomeAssertions;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

public class UutAdapter
    : IUutAdapter<KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>
{
    public static AndConstraint<ChangeSetAssertions<KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>> AssertShouldBeValid(KeyedChangeSet<int, int> subject)
        => subject.Should().BeValid();

    public static KeyedChange<int, int> CreateAddition(int item)
        => KeyedChange.CreateAddition(
            key:    item + 10,
            item:   item);

    public static ChangeSetBuilderBase<KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType> CreateUut(
            int     initialCapacity,
            bool    isSourceEmpty)
        => new KeyedChangeSet<int, int>.Builder(
            initialCapacity:    initialCapacity,
            isSourceEmpty:      isSourceEmpty);

    public static ChangeSetBuilderBase<KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType> CreateUut(bool isSourceEmpty)
        => new KeyedChangeSet<int, int>.Builder(isSourceEmpty);

    public static KeyedChange<int, int> CreateNone()
        => default;

    public static KeyedChange<int, int> CreateRemoval(int item)
        => KeyedChange.CreateRemoval(
            key:    item + 10,
            item:   item);
}
