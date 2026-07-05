using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.AddTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
