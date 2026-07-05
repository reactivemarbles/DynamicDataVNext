using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.ResetTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
