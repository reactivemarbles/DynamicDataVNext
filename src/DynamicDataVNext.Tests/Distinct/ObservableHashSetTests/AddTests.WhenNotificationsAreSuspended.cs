namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.AddTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
