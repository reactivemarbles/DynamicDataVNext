namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ClearTests
    : ChangeSetBuilderBaseTests.ClearTests.Base<UutAdapter, OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>;
