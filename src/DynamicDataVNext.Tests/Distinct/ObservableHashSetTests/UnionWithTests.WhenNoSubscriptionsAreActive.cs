using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class UnionWithTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.UnionWithTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
