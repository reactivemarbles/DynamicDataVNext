using NUnit.Framework;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ChangesEnsureCapacityTests
    : ChangesEnsureCapacityTestsBase<UutAdapter, KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>
{ }
