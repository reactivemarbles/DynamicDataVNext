using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateUpdateTests
{
    public abstract class Base
    {
        [Test]
        public void InputsAreValid_ResultIsUpdate()
        {
            var oldIndex    = 0;
            var oldItem     = 1;
            var newIndex    = 2;
            var newItem     = 3;
        
            var result = InvokeUut(
                oldIndex:   oldIndex,
                oldItem:    oldItem,
                newIndex:   newIndex,
                newItem:    newItem);
        
            result.Category.Should().Be(ChangeCategory.Other, "an update change should have been generated");
            result.Type.Should().Be(OrderedChangeType.Update, "an update change should have been generated");
            result.AsUpdate().OldIndex.Should().Be(oldIndex, "the given indexes should have been embedded into the generated change");
            result.AsUpdate().OldItem.Should().Be(oldItem, "the given items should have been embedded into the generated change");
            result.AsUpdate().NewIndex.Should().Be(newIndex, "the given indexes should have been embedded into the generated change");
            result.AsUpdate().NewItem.Should().Be(newItem, "the given items should have been embedded into the generated change");
        }
        
        protected abstract OrderedChange<int> InvokeUut(
            int oldIndex,
            int oldItem,
            int newIndex,
            int newItem);
    }
}
