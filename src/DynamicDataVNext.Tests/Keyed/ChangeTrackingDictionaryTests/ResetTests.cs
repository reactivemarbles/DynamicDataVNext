using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ResetTests
    : Keyed.ResetTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
