namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public static partial class ApplyToTests
{
    public class TestCase
    {
        public required DistinctChangeSet<int> ChangeSet { get; init; }
        
        public required IReadOnlyList<int> ExpectedItems { get; init; }
        
        public required IReadOnlyList<int> TargetItems { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetContainsOnlyRefreshments_TestCases
        = new[]
        {
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Refreshment }
                    }),
                    TargetItems     = new[] { 1, 2, 3 },
                    ExpectedItems   = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Single item refreshed)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Refreshment },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Refreshment },
                        new DistinctChange<int>() { Item = 4, Type = DistinctChangeType.Refreshment }
                    }),
                    TargetItems     = new[] { 1, 2, 3, 4, 5 },
                    ExpectedItems   = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Multiple items refreshed)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Addition }
                    }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = new[] { 1 } 
                })
                .SetName("{m}(Single item added, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Addition }
                    }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Multiple items added, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Addition }
                    }),
                    TargetItems     = new[] { 2, 3 },
                    ExpectedItems   = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Single item added, Not in target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Addition }
                    }),
                    TargetItems     = new[] { 1, 2, 3 },
                    ExpectedItems   = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Single item added, Already in target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Addition }
                    }),
                    TargetItems     = new[] { 3, 4 },
                    ExpectedItems   = new[] { 1, 2, 3, 4 },
                })
                .SetName("{m}(Multiple items added, Overlaps with target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal }
                    }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = Array.Empty<int>() 
                })
                .SetName("{m}(Single item removed, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Removal }
                    }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = Array.Empty<int>() 
                })
                .SetName("{m}(Multiple items removed, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal }
                    }),
                    TargetItems     = new[] { 2, 3 },
                    ExpectedItems   = new[] { 2, 3 }
                })
                .SetName("{m}(Single item removed, Not in target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal }
                    }),
                    TargetItems     = new[] { 1, 2, 3 },
                    ExpectedItems   = new[] { 2, 3 }
                })
                .SetName("{m}(Single item removed, Present in target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Removal }
                    }),
                    TargetItems     = new[] { 2, 3, 4 },
                    ExpectedItems   = new[] { 4 } 
                })
                .SetName("{m}(Multiple items removed, Overlaps with target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = Array.Empty<int>(),
                })
                .SetName("{m}(Clear, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    TargetItems     = new[] { 2, 3, 4 },
                    ExpectedItems   = Array.Empty<int>() 
                })
                .SetName("{m}(Clear, Non-empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3 },
                        addedItems:     new[] { 4, 5, 6 }),
                    TargetItems     = Array.Empty<int>(),
                    ExpectedItems   = new[] { 4, 5, 6 } 
                })
                .SetName("{m}(Reset, Empty target)"),
            new TestCaseData(new TestCase()
                {
                    ChangeSet       = DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3 },
                        addedItems:     new[] { 4, 5, 6 }),
                    TargetItems     = new[] { 2, 3, 4 },
                    ExpectedItems   = new[] { 4, 5, 6 }
                })
                .SetName("{m}(Reset, Non-empty target)")
        };
}
