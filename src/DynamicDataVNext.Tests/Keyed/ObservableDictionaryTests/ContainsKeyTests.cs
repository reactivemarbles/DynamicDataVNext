namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public class ContainsKeyTests
    : Keyed.DictionaryTestBases.ContainsKeyTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
