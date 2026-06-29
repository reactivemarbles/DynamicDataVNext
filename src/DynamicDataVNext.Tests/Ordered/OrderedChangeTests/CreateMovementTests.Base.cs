using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateMovementTests
{
    public abstract class Base
    {
        [Test]
        public void InputsAreValid_ResultIsMovement()
        {
            var oldIndex = 0;
            var newIndex = 1;
            var item = 2;
        
            var result = InvokeUut(
                oldIndex:   oldIndex,
                newIndex:   newIndex,
                item:       item);
        
            result.Category.Should().Be(ChangeCategory.Other, "a movement change should have been generated");
            result.Type.Should().Be(OrderedChangeType.Movement, "a movement change should have been generated");
            result.AsMovement().OldIndex.Should().Be(oldIndex, "the given indexes should have been embedded into the generated change");
            result.AsMovement().NewIndex.Should().Be(newIndex, "the given indexes should have been embedded into the generated change");
            result.AsMovement().Item.Should().Be(item, "the given item should have been embedded into the generated change");
        }
        
        protected abstract OrderedChange<int> InvokeUut(
            int oldIndex,
            int newIndex,
            int item);
    }
}
