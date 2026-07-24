using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ConstructorTests
    : Distinct.SetTestBases.ConstructorTests.Base<UutFixture, ChangeTrackingHashSet<int>>;

