using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateRemovalTests
{
    [TestFixture]
    public sealed class FromKeyAndItem
        : Base
    {
        protected override KeyedChange<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChange.CreateRefreshment(
                key:    key,
                item:   item);
    }
}
