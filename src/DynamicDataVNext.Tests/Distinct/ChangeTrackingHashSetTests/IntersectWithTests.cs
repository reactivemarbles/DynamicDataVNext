using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IntersectWithTests
    : Distinct.IntersectWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
