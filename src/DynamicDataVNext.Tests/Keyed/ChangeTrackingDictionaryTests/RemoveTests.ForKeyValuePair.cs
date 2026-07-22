using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class RemoveTests
{
    [TestFixture]
    public class ForKeyValuePair
        : Keyed.RemoveTests.ForKeyValuePairBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
