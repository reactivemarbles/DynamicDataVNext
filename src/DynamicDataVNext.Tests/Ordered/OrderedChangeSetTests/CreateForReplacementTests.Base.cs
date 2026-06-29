using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForReplacementTests
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
            var oldItem = 1;
            var newItem = 2;
    
            var result = InvokeUut(
                index:      index,
                oldItem:    oldItem,
                newItem:    newItem);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single replacement was given");
            result.Changes[0].Type.Should().Be(OrderedChangeType.Replacement, "a single replacement was given");
            result.Changes[0].AsReplacement().Index.Should().Be(index, "the given index should have been embedded in the generated change");
            result.Changes[0].AsReplacement().OldItem.Should().Be(oldItem, "the given items should have been embedded in the generated change");
            result.Changes[0].AsReplacement().NewItem.Should().Be(newItem, "the given items should have been embedded in the generated change");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(
            int index,
            int oldItem,
            int newItem);
    }
}
