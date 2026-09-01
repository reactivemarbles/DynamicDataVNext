namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class BuildAndClearTests
    : ChangeSetBuilderBaseTests.BuildAndClearTests.Base<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
