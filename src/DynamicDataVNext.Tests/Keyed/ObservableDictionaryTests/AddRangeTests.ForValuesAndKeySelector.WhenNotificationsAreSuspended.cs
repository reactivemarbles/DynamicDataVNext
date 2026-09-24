namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    public partial class ForValuesAndKeySelector
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.AddRangeTests.ForValuesAndKeySelectorBase<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
