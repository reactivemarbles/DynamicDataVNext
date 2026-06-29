using NUnit.Framework;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class EnsureCapacityTests
    : EnsureCapacityTestsBase<UutAdapter, OrderedChangeSet<int>, OrderedChange<int>, OrderedChangeType>;
