using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.SetTestBases.ExceptWithTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
