namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.RemoveTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
