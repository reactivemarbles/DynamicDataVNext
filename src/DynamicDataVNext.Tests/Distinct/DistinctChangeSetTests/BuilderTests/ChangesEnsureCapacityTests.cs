using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ChangesEnsureCapacityTests
    : ChangesEnsureCapacityTestsBase<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
