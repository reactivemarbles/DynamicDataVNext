using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

[TestFixture]
public class AsClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotClear_TestCasea
        = new[]
        {
            new TestCaseData(KeyedChangeSet.Empty<int, int>())              .SetName("{m}(Empty Changeset)"),
            new TestCaseData(KeyedChangeSet.CreateForReset(
                removedItems:   new[] { 1 },
                addedItems:     new[] { 2 },
                keySelector:    static item => item + 10))                  .SetName("{m}(Reset Changeset)"),
            new TestCaseData(KeyedChangeSet.CreateForUpdate(changes: new[]
            {
                KeyedChange.CreateAddition(
                    key:    1,
                    item:   2)
            }))                                                             .SetName("{m}(Update Changeset)"),
        };
    [TestCaseSource(nameof(WhenTypeIsNotClear_TestCasea))]
    public void WhenTypeIsNotClear_ThrowsException(KeyedChangeSet<int, int> uut)
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
        var removals = Enumerable
            .Range(1, itemCount)
            .Select(item => new KeyedItem<int, int>()
            {
                Item    = item,
                Key     = item + 10
            })
            .ToArray();

        var uut = KeyedChangeSet.CreateForClear(removals);
        
        var result = uut.AsClear();

        result.Items.Should().BeEquivalentTo(removals, static config => config.WithStrictOrdering(), "all removed items should be listed");
    }
}
