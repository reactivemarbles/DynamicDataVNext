namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.RefreshTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
