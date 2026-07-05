using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.OverlapsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
