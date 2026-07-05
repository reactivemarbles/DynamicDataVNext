using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class IntersectWithTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.IntersectWithTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
