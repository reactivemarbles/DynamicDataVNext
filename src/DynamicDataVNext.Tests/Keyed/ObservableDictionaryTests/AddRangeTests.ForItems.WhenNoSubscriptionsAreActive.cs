namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    public partial class ForItems
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.DictionaryTestBases.AddRangeTests.ForItemsBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
    }
}
