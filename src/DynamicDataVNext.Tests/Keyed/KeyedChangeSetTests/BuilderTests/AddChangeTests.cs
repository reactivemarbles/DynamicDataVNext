namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class AddChangeTests
    : ChangeSetBuilderBaseTests.AddChangeTests.Base<UutAdapter, KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>;
