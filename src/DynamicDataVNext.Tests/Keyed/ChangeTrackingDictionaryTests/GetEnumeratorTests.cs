using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class GetEnumeratorTests
    : Keyed.DictionaryTestBases.GetEnumeratorTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
