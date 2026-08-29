namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class ContainsTests
    : Ordered.ListTestBases.ContainsTests.Base<UutFixture, ChangeTrackingList<string?>>;
