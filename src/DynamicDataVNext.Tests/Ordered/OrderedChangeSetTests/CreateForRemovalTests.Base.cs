using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForRemovalTests
{
    public abstract class Base
    {
        public static readonly IReadOnlyList<TestCaseData> InputsAreValid_TestCases
            = new[]
            {
                new TestCaseData(0)             .SetName("{m}(Minimum index)"),
                new TestCaseData(int.MaxValue)  .SetName("{m}(Maximum index)")
            };
        [TestCaseSource(nameof(InputsAreValid_TestCases))]
        public void InputsAreValid_ResultIsUpdate(int index)
        {
            var item = 1;
    
            var result = InvokeUut(
                index:  index,
                item:   item);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single removed item item was given");
            result.Changes[0].Type.Should().Be(OrderedChangeType.Removal, "a single removed item was given");
            result.Changes[0].AsRemoval().Index.Should().Be(index, "the given index should have been embedded in the generated change");
            result.Changes[0].AsRemoval().Item.Should().Be(item, "the given item should have been embedded in the generated change");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(
            int index,
            int item);
    }
}
