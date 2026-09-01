namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public class SetupTestCase
{
    public required IReadOnlyList<ChangeCategory> ChangesCategories { get; init; }

    public required int SourceCount { get; init; }
}
