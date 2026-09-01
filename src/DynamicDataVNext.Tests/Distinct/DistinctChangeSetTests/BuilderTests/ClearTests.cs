namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ClearTests
    : ChangeSetBuilderBaseTests.ClearTests.Base<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
