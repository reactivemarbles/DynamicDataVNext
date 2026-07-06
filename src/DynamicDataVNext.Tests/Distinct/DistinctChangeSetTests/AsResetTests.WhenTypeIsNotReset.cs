using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public partial class AsResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotReset_TestCases
        = new[]
        {
            new TestCaseData(DistinctChangeSet.CreateForClear(new[] { 1 }))    .SetName("{m}(Clear Changeset)"),
            new TestCaseData(DistinctChangeSet.Empty<int>())                   .SetName("{m}(Empty Changeset)"),
            new TestCaseData(DistinctChangeSet.CreateForUpdate(new[]
            {
                new DistinctChange<int>()
                {
                    Item = 1,
                    Type = DistinctChangeType.Addition
                }
            }))                                                             .SetName("{m}(Update Changeset)"),
        };

    [TestCaseSource(nameof(WhenTypeIsNotReset_TestCases))]
    public void WhenTypeIsNotReset_ThrowsException(DistinctChangeSet<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsReset();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
        
        result.Message.Should().Contain(nameof(ChangeSetType.Reset));
        
        Console.WriteLine(result);
    }
}
