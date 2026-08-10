namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ResetTests
    : Distinct.SetTestBases.ResetTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
