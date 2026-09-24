namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public class ContainsTests
    : Keyed.DictionaryTestBases.ContainsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
