using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.ExceptWithTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
