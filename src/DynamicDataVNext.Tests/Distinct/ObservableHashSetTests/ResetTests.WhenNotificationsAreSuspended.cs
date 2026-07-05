using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.ResetTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
