using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class IntersectWithTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.IntersectWithTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
