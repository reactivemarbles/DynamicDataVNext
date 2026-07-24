using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddRangeTests
{
    [TestFixture]
    public sealed class ForValuesAndKeySelector
        : Keyed.DictionaryTestBases.AddRangeTests.ForValuesAndKeySelectorBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
