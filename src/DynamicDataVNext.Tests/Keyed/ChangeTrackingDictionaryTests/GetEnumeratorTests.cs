using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class GetEnumeratorTests
    : Keyed.GetEnumeratorTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
