namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsProperSupersetOfTests
    : Distinct.SetTestBases.IsProperSupersetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
