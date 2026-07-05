using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.RemoveTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
