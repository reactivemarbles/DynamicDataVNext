using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.RemoveTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
