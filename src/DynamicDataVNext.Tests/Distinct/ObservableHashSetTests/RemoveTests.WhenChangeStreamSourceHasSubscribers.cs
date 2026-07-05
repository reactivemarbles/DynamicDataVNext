using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.RemoveTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
