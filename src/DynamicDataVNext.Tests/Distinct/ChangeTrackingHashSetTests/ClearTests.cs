namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ClearTests
    : Distinct.SetTestBases.ClearTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
