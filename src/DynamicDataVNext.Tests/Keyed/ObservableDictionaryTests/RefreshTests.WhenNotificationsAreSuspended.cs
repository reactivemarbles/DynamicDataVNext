namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.DictionaryTestBases.RefreshTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
}
