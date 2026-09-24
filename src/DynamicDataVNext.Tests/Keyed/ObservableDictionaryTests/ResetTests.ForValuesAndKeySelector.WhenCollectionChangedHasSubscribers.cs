namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    public partial class ForValuesAndKeySelector
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.DictionaryTestBases.ResetTests.ForValuesAndKeySelectorBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
    }
}
