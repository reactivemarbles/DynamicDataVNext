namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.SetTestBases.OverlapsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
