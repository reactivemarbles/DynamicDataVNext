using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.AddTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
