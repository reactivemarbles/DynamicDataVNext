using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ClearTests
    : Keyed.ClearTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
