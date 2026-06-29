using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateReplacementTests
{
    public abstract class Base
    {
        [Test]
        public void Always_ResultIsReplacement()
        {
            var key = 1;
            var oldItem = 2;
            var newItem = 3;
            
            var result = InvokeUut(key, oldItem, newItem);
            
            result.Category.Should().Be(ChangeCategory.Other, "a removal change should have been generated");
            result.Type.Should().Be(KeyedChangeType.Replacement, "a removal change should have been generated");
            result.AsReplacement().Key.Should().Be(key, "the given key should have been embedded in the generated change");
            result.AsReplacement().OldItem.Should().Be(oldItem, "the given replaced item should have been embedded in the generated change");
            result.AsReplacement().NewItem.Should().Be(newItem, "the given replacement item should have been embedded in the generated change");
        }
        
        protected abstract KeyedChange<int, int> InvokeUut(
            int key,
            int oldItem,
            int newItem);
    }
}
