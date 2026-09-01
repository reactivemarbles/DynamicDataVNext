namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ClearTests
    : ChangeSetBuilderBaseTests.ClearTests.Base<UutAdapter, KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>;
