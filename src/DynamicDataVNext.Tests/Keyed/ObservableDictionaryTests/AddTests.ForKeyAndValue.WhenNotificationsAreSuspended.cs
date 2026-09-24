namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    public partial class ForKeyAndValue
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.AddTests.ForKeyAndValueBase<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
