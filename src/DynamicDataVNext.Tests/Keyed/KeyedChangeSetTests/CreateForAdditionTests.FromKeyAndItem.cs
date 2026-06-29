using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionTests
{
    [TestFixture]
    public sealed class FromKeyAndItem
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForAddition(
                key:    key,
                item:   item);
    }
}
