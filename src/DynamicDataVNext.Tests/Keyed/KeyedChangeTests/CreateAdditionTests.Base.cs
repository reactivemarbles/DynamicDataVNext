using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateAdditionTests
{
    public abstract class Base
    {
        [Test]
        public void Always_ResultIsAddition()
        {
            var key = 1;
            var item = 2;
            
            var result = InvokeUut(key, item);
            
            result.Category.Should().Be(ChangeCategory.Addition, "an addition change should have been generated");
            result.Type.Should().Be(KeyedChangeType.Addition, "an addition change should have been generated");
            result.AsAddition().Key.Should().Be(key, "the given key should have been embedded in the generated change");
            result.AsAddition().Item.Should().Be(item, "the given item should have been embedded in the generated change");
        }
        
        protected abstract KeyedChange<int, int> InvokeUut(
            int key,
            int item);
    }
}
