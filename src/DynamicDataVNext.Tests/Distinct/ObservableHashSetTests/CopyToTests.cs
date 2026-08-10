namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class CopyToTests
    : Distinct.SetTestBases.CopyToTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
