using NUnit.Framework;

using DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests.BuilderTests;

[TestFixture]
public sealed class ConstructorTests
    : ConstructorTestsBase<UutAdapter, KeyedChangeSet<int, int>, KeyedChange<int, int>, KeyedChangeType>
{ }
