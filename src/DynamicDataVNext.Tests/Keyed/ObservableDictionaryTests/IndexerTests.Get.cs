namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    [TestFixture]
    public class Get
        : Keyed.DictionaryTestBases.IndexerTests.Get.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
}
