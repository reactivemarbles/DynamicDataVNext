using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddRangeTests
{
    [TestFixture]
    public sealed class ForValuesAndKeySelector
        : Keyed.AddRangeTests.ForValuesAndKeySelectorBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
