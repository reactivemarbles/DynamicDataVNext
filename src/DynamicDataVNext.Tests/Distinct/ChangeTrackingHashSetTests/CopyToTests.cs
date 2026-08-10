namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class CopyToTests
    : Distinct.SetTestBases.CopyToTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
