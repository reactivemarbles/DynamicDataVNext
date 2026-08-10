namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class SymmetricExceptWithTests
    : Distinct.SetTestBases.SymmetricExceptWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
