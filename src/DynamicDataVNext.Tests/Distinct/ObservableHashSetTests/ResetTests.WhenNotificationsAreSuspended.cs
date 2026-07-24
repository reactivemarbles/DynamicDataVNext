using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.ResetTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
