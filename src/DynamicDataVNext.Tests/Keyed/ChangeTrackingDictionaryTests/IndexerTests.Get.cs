namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class Get
        : Keyed.DictionaryTestBases.IndexerTests.Get.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
}
