namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    public partial class ForKeyValuePair
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.DictionaryTestBases.AddTests.ForKeyValuePairBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
    }
}
