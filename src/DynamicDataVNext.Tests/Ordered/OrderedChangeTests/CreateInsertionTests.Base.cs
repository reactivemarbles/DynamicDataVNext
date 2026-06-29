using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateInsertionTests
{
    public abstract class Base
    {
        [Test]
        public void InputsAreValid_ResultIsInsertion()
        {
            var index = 0;
            var item = 1;
        
            var result = InvokeUut(
                index:  index,
                item:   item);
        
            result.Category.Should().Be(ChangeCategory.Addition, "an insertion change should have been generated");
            result.Type.Should().Be(OrderedChangeType.Insertion, "an insertion change should have been generated");
            result.AsInsertion().Index.Should().Be(index, "the given index should have been embedded into the generated change");
            result.AsInsertion().Item.Should().Be(item, "the given item should have been embedded into the generated change");
        }
        
        protected abstract OrderedChange<int> InvokeUut(
            int index,
            int item);
    }
}
