using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class IntersectWithTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.SetTestBases.IntersectWithTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
