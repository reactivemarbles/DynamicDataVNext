using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class RemoveTests
{
    [TestFixture]
    public class ForKey
        : Keyed.DictionaryTestBases.RemoveTests.ForKeyBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
