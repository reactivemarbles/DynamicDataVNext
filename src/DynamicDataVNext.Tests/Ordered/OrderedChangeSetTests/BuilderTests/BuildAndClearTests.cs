namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class BuildAndClearTests
    : ChangeSetBuilderBaseTests.BuildAndClearTests.Base<UutAdapter, OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>;
