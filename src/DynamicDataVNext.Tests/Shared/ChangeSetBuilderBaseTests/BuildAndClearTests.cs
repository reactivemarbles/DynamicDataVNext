namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class BuildAndClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenAdditionsDoNotFollowClearOrInitiallyEmptySource_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Single item in source, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Single item in source, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 2,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Single removal, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 2,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Single removal, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple removals, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple removals, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Multiple items in source, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple additions)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenAdditionsFollowClearOrInitiallyEmptySource_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Empty source, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Empty source, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Single item in source, Single removal, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Single item in source, Single removal, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple removals, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple removals, Multiple additions)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangesIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(Empty source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(Multiple items in source)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenRemovalsAloneDoNotLeaveSourceEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 2,
                    ChangesCategories   = new[] { ChangeCategory.Removal }
                })
                .SetName("{m}(Multiple items in source, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 4,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple removals)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenRemovalsAloneLeaveSourceEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[] { ChangeCategory.Removal }
                })
                .SetName("{m}(Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenRemovalsFollowReset_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Empty Source, Single item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Empty Source, Multiple item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Empty Source, Multiple item reset, Multiple removals)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Single item in source, Single item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Single item in source, Multiple item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Single item in source, Multiple item reset, Multiple removals)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Single item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple item reset, Single removal)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple item reset, Multiple removals)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenRemovalsLeaveSourceEmptyAfterAdditions_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Single item in source, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Single item in source, Multiple additions)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Single addition)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple items in source, Multiple additions)")
        };
}
