namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class RefreshTests
    : Distinct.SetTestBases.RefreshTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
