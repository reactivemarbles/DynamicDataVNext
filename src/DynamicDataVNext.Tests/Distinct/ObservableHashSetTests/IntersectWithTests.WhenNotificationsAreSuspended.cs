using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class IntersectWithTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.IntersectWithTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
