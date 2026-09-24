namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    public partial class ForKeyValuePair
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.AddTests.ForKeyValuePairBase<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
