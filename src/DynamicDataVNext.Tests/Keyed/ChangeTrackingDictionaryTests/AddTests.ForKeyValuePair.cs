using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddTests
{
    [TestFixture]
    public sealed class ForKeyValuePair
        : Keyed.AddTests.ForKeyValuePairBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
