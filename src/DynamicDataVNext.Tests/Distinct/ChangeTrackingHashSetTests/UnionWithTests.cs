namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class UnionWithTests
    : Distinct.SetTestBases.UnionWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
