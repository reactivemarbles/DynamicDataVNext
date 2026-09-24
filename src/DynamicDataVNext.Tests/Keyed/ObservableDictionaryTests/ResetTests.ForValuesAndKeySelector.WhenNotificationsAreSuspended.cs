namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    public partial class ForValuesAndKeySelector
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.ResetTests.ForValuesAndKeySelectorBase<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
