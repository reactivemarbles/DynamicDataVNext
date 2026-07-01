using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public class ConstructorTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : Base
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    public static readonly IReadOnlyList<TestCaseData> WhenCapacityIsGiven_TestCases
        = new[]
        {
            new TestCaseData(0,     true)   .SetName("{m}(Empty capacity)"),
            new TestCaseData(0,     false)  .SetName("{m}(Populated source)"),
            new TestCaseData(1,     true)   .SetName("{m}(Trivial capacity)"),
            new TestCaseData(10,    true)   .SetName("{m}(Non-trivial capacity)")
        };
    [TestCaseSource(nameof(WhenCapacityIsGiven_TestCases))]
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
