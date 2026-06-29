using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateReplacementTests
{
    public abstract class Base
    {
        [Test]
        public void InputsAreValid_ResultIsReplacement()
        {
            var index = 0;
            var oldItem = 1;
            var newItem = 2;
        
            var result = InvokeUut(
                index:      index,
                oldItem:    oldItem,
                newItem:    newItem);
        
            result.Category.Should().Be(ChangeCategory.Other, "a replacement change should have been generated");
            result.Type.Should().Be(OrderedChangeType.Replacement, "a replacement change should have been generated");
            result.AsReplacement().Index.Should().Be(index, "the given index should have been embedded into the generated change");
            result.AsReplacement().OldItem.Should().Be(oldItem, "the given items should have been embedded into the generated change");
            result.AsReplacement().NewItem.Should().Be(newItem, "the given items should have been embedded into the generated change");
        }
        
        protected abstract OrderedChange<int> InvokeUut(
            int index,
            int oldItem,
            int newItem);
    }
}
