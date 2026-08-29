namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class GetTests
        : Ordered.ListTestBases.IndexerTests.GetTestsBase<UutFixture, ChangeTrackingList<string?>>;
}
