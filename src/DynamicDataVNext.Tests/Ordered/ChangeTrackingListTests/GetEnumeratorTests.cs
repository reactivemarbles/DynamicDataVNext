namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class GetEnumeratorTests
    : Ordered.ListTestBases.GetEnumeratorTests.Base<UutFixture, ChangeTrackingList<string?>>;
