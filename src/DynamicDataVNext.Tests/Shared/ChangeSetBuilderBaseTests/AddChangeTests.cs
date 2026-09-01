namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class AddChangeTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(No pending changes)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[] { ChangeCategory.Removal }
                })
                .SetName("{m}(Single pending removal)"),
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
                .SetName("{m}(Multiple pending removals)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Complex pending changes, Empty source)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 1,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(No pending changes, Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = Array.Empty<ChangeCategory>()
                })
                .SetName("{m}(No pending changes, Multiple items in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Single pending addition, Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 2,
                    ChangesCategories   = new[] { ChangeCategory.Addition }
                })
                .SetName("{m}(Single pending addition, Multiple items in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 2,
                    ChangesCategories   = new[] { ChangeCategory.Removal }
                })
                .SetName("{m}(Single pending removal, Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[] { ChangeCategory.Removal }
                })
                .SetName("{m}(Single pending removal, Multiple items in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Addition,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Multiple pending additions, Multiple items in source)"),
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
                .SetName("{m}(Multiple pending removals, Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 6,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Removal,
                        ChangeCategory.Removal,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Multiple pending removals, Multiple items in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 0,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition
                    }
                })
                .SetName("{m}(Complex Pending changes, Single item in source)"),
            new TestCaseData(new SetupTestCase()
                {
                    SourceCount         = 3,
                    ChangesCategories   = new[]
                    {
                        ChangeCategory.Addition,
                        ChangeCategory.Removal,
                        ChangeCategory.Addition,
                        ChangeCategory.Removal
                    }
                })
                .SetName("{m}(Complex pending changes, Multiple items in source)")
        };
}
