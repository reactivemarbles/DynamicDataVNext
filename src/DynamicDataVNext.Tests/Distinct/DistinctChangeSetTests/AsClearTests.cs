using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public class AsClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotClear_TestCasea
        = new[]
        {
            new TestCaseData(DistinctChangeSet.Empty<int>())                    .SetName("{m}(Empty Changeset)"),
            new TestCaseData(DistinctChangeSet.CreateForReset(
                removedItems:   new[] { 1 },
                addedItems:     new[] { 2 }))                                   .SetName("{m}(Reset Changeset)"),
            new TestCaseData(DistinctChangeSet.CreateForUpdate(changes: new[]
            {
                new DistinctChange<int>()
                {
                    Item = 1,
                    Type = DistinctChangeType.Addition
                }
            }))                                                                 .SetName("{m}(Update Changeset)"),
        };
    [TestCaseSource(nameof(WhenTypeIsNotClear_TestCasea))]
    public void WhenTypeIsNotClear_ThrowsException(DistinctChangeSet<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsClear();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
        
        result.Message.Should().Contain(nameof(ChangeSetType.Clear));
        
        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsClear_TestCases
        = new[]
        {
            new TestCaseData(1).SetName("{m}(Single item)"),
            new TestCaseData(5).SetName("{m}(Multiple items)")
        };
    [TestCaseSource(nameof(WhenTypeIsClear_TestCases))]
    public void WhenTypeIsClear_ResultMatchesChanges(int itemCount)
    {
        var items = Enumerable
            .Range(1, itemCount)
            .ToArray();

        var uut = DistinctChangeSet.CreateForClear(items: items);
        
        var result = uut.AsClear();

        result.Items.Should().BeEquivalentTo(items, static config => config.WithStrictOrdering(), "all removed items should be listed");
    }
}
