using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddTests
{
    [TestFixture]
    public sealed class ForKeyValuePair
        : Keyed.DictionaryTestBases.AddTests.ForKeyValuePairBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
