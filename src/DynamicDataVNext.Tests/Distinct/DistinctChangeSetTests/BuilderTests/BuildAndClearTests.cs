using NUnit.Framework;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests.BuilderTests;

[TestFixture]
public sealed class BuildAndClearTests
    : BuildAndClearTestsBase<UutAdapter, DistinctChangeSet<int>, DistinctChange<int>, DistinctChangeType>;
