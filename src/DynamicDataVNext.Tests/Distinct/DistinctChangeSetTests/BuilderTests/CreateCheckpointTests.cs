using NUnit.Framework;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class CreateCheckpointTests
    : CreateCheckpointTestsBase<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
