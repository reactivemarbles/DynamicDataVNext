using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ConstructorTests
    : Keyed.ConstructorTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;

