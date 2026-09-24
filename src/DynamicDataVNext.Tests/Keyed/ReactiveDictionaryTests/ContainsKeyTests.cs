namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

[TestFixture]
public class ContainsKeyTests
    : Keyed.DictionaryTestBases.ContainsKeyTests.Base<UutFixture, ReactiveDictionary<string, int>>;
