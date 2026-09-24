namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

[TestFixture]
public class GetEnumeratorTests
    : Keyed.DictionaryTestBases.GetEnumeratorTests.Base<UutFixture, ReactiveDictionary<string, int>>;
