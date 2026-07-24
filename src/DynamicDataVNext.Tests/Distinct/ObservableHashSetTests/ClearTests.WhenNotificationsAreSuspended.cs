using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.ClearTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
