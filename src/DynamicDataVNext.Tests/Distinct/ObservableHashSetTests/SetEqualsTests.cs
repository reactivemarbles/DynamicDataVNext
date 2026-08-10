namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class SetEqualsTests
    : Distinct.SetTestBases.SetEqualsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
