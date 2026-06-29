using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRefreshmentTests
{
    [TestFixture]
    public sealed class FromKeyAndItem
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForRefreshment(
                key:    key,
                item:   item);
    }
}
