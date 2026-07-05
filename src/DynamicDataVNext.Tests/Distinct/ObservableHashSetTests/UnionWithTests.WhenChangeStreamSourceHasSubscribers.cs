using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class UnionWithTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.UnionWithTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
