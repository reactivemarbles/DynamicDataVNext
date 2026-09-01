namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class BuildAndClearTests
    : ChangeSetBuilderBaseTests.BuildAndClearTests.Base<UutAdapter, KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>;
