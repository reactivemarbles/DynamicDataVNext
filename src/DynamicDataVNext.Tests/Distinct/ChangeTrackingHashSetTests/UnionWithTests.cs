using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class UnionWithTests
    : Distinct.UnionWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
