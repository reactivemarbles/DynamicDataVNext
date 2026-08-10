namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForInsertionTests
{
    public abstract class Base
    {
        [TestCase(0,            TestName = "{m}(Minimum index)")]
        [TestCase(int.MaxValue, TestName = "{m}(Maximum index)")]
        public void InputsAreValid_ResultIsUpdate(int index)
        {
            var item = 1;
    
            var result = InvokeUut(
                index:  index,
                item:   item);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single added item was given");
            result.Changes[0].Type.Should().Be(OrderedChangeType.Insertion, "a single added item was given");
            result.Changes[0].AsInsertion().Index.Should().Be(index, "the given index should have been embedded in the generated change");
            result.Changes[0].AsInsertion().Item.Should().Be(item, "the given item should have been embedded in the generated change");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(
            int index,
            int item);
    }
}
