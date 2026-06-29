using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateRemovalTests
{
    public abstract class Base
    {
        [Test]
        public void Always_ResultIsRemoval()
        {
            var key = 1;
            var item = 2;
            
            var result = InvokeUut(key, item);
            
            result.Category.Should().Be(ChangeCategory.Other, "a removal change should have been generated");
            result.Type.Should().Be(KeyedChangeType.Refreshment, "a removal change should have been generated");
            result.AsRefreshment().Key.Should().Be(key, "the given key should have been embedded in the generated change");
            result.AsRefreshment().Item.Should().Be(item, "the given item should have been embedded in the generated change");
        }
        
        protected abstract KeyedChange<int, int> InvokeUut(
            int key,
            int item);
    }
}
