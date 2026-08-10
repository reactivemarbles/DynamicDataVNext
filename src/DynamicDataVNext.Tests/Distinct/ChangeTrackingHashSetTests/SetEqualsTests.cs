namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class SetEqualsTests
    : Distinct.SetTestBases.SetEqualsTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
