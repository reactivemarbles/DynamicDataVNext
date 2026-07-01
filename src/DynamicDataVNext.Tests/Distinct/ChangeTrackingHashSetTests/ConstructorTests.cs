using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ConstructorTests
    : Distinct.ConstructorTests.Base<UutFixture, ChangeTrackingHashSet<int>>;

