namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

[TestFixture]
public class IndexerTests
    : Keyed.DictionaryTestBases.IndexerTests.Get.Base<UutFixture, ReactiveDictionary<string, int>>;
