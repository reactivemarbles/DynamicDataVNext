namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public class CopyToTests
    : Keyed.DictionaryTestBases.CopyToTestsBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
