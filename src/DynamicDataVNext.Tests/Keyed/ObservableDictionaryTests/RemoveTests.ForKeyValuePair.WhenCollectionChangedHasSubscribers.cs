namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RemoveTests
{
    public partial class ForKeyValuePair
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.DictionaryTestBases.RemoveTests.ForKeyValuePairBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
    }
}
