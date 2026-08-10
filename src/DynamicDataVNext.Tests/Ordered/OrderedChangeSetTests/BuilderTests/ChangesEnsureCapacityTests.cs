using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ChangesEnsureCapacityTests
    : ChangesEnsureCapacityTestsBase<UutAdapter, OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>;
