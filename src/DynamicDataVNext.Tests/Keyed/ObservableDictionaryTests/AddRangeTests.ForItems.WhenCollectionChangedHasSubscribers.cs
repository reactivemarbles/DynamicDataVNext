namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    public partial class ForItems
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.DictionaryTestBases.AddRangeTests.ForItemsBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
    }
}
