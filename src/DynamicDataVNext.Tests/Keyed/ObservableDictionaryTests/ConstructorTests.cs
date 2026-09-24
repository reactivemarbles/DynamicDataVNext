namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public class ConstructorTests
    : Keyed.DictionaryTestBases.ConstructorTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
