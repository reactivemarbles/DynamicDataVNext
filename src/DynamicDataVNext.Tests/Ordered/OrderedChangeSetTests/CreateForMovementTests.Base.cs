using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForMovementTests
{
    public abstract class Base
    {
        public static readonly IReadOnlyList<TestCaseData> InputsAreValid_TestCases
            = new[]
            {
                new TestCaseData(0,             0)              .SetName("{m}(Minimum indexes)"),
                new TestCaseData(int.MaxValue,  int.MaxValue)   .SetName("{m}(Maximum indexes)"),
                new TestCaseData(1,             2)              .SetName("{m}(Unique indexes)")
            };
        [TestCaseSource(nameof(InputsAreValid_TestCases))]
        public void InputsAreValid_ResultIsUpdate(
            int oldIndex,
            int newIndex)
        {
            var item = 1;
    
            var result = InvokeUut(
                oldIndex:   oldIndex,
                newIndex:   newIndex,
                item:       item);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single movement was given");
            result.Changes[0].Type.Should().Be(OrderedChangeType.Movement, "a single movement was given");
            result.Changes[0].AsMovement().OldIndex.Should().Be(oldIndex, "the given indexes should have been embedded in the generated change");
            result.Changes[0].AsMovement().NewIndex.Should().Be(newIndex, "the given indexes should have been embedded in the generated change");
            result.Changes[0].AsMovement().Item.Should().Be(item, "the given item should have been embedded in the generated change");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(
            int oldIndex,
            int newIndex,
            int item);
    }
}
