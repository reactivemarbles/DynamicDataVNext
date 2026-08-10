namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class GetEnumeratorTests
    : Distinct.SetTestBases.GetEnumeratorTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
