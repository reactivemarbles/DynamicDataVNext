using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateRemovalTests
{
    public abstract class Base
    {
        [Test]
        public void InputsAreValid_ResultIsRemoval()
        {
            var index = 0;
            var item = 1;
        
            var result = InvokeUut(
                index:  index,
                item:   item);
        
            result.Category.Should().Be(ChangeCategory.Removal, "a removal change should have been generated");
            result.Type.Should().Be(OrderedChangeType.Removal, "a removal change should have been generated");
            result.AsRemoval().Index.Should().Be(index, "the given index should have been embedded into the generated change");
            result.AsRemoval().Item.Should().Be(item, "the given item should have been embedded into the generated change");
        }
        
        protected abstract OrderedChange<int> InvokeUut(
            int index,
            int item);
    }
}
