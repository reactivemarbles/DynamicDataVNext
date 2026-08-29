namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class CopyToTests
    : Distinct.SetTestBases.CopyToTestsBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
