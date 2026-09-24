namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    public partial class ForItems
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.ResetTests.ForItemsBase<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
