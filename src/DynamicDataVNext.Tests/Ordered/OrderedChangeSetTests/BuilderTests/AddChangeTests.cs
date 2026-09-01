using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class AddChangeTests
    : ChangeSetBuilderBaseTests.AddChangeTests.Base<UutAdapter, OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>
{
    protected override void WhenChangeIsAddition_AssertCurrentTypeIsCorrect(
        SetupTestCase                                                                       testCase,
        ChangeSetBuilderBase<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>  uut,
        IReadOnlyList<OrderedChange<int>>                                                   priorChanges)
    {
        if (        (testCase.SourceCount is 0)
                &&  priorChanges.All(static change => change.Category is ChangeCategory.Addition))
            uut.CurrentType.Should().Be(ChangeSetType.Reset, "additions to an empty collection should be considered a reset operation");
        else if (   (testCase.SourceCount == priorChanges.Count(static change => change.Category is ChangeCategory.Removal))
                &&  priorChanges
                        .SkipWhile(static change => change.Category is ChangeCategory.Removal)
                        .All(static change => change.Category is ChangeCategory.Addition)
                &&  priorChanges
                        .TakeWhile(static change => change.Category is ChangeCategory.Removal)
                        .Select(static change => change.AsRemoval().Index)
                        .SequenceEqual(Enumerable.Range(0, testCase.SourceCount)
                            .Reverse()))
            uut.CurrentType.Should().Be(ChangeSetType.Reset, "a sequence of reverse-order removals, which removes every item in a collection, followed by a sequence of additions, should be considered a reset operation");
        else
            uut.CurrentType.Should().Be(ChangeSetType.Update, "an addition that is not part of a reset operation should be considered part of an update");
    }

    protected override void WhenChangeIsRemoval_AssertCurrentTypeIsCorrect(
        ChangeSetBuilderBase<OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>  uut,
        int                                                                                 priorSourceCount,
        IReadOnlyList<OrderedChange<int>>                                                   priorChanges,
        OrderedChange<int>                                                                  change)
    {
        if (        (priorSourceCount is 1)
                &&  priorChanges.All(static change => change.Category is ChangeCategory.Removal)
                &&  priorChanges
                        .Select(static change => change.AsRemoval().Index)
                        .SequenceEqual(Enumerable.Range(1, priorChanges.Count)
                            .Reverse())
                &&  (change.AsRemoval().Index is 0))
            uut.CurrentType.Should().Be(ChangeSetType.Clear, "a sequence of changes consisting of only removals, in reverse order, that leaves the collection empty, should be considered a clear operation");
        else
            uut.CurrentType.Should().Be(ChangeSetType.Update, "a removal that is not part of a clear operation should be considered part of an update operation");
    }
}
