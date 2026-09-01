namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class AddChangeTests
    : ChangeSetBuilderBaseTests.AddChangeTests.Base<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
