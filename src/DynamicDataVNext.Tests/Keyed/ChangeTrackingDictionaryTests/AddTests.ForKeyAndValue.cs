using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddTests
{
    [TestFixture]
    public sealed class ForKeyAndValue
        : Keyed.AddTests.ForKeyAndValueBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
