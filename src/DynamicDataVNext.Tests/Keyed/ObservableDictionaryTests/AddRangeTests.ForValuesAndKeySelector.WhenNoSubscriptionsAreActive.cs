namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    public partial class ForValuesAndKeySelector
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.DictionaryTestBases.AddRangeTests.ForValuesAndKeySelectorBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
    }
}
