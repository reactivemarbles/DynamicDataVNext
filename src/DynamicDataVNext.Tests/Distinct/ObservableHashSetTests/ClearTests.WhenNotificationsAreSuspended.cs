using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.ClearTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
