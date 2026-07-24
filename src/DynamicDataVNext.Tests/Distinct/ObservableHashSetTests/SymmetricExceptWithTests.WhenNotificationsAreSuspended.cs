using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class SymmetricExceptWithTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.SymmetricExceptWithTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
