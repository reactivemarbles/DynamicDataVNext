using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.ExceptWithTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
