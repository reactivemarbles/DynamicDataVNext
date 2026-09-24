namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    public partial class Set
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.DictionaryTestBases.IndexerTests.Set.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
    }
}
