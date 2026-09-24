namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    public partial class Set
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.DictionaryTestBases.IndexerTests.Set.Base<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
    }
}
