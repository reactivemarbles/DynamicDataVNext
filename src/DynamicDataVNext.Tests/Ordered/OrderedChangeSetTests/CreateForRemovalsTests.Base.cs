using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    public abstract class Base
    {
        [Test]
        public void WhenIndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = InvokeUut(
                        index:  -1,
                        items:  Array.Empty<int>());
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);
        }
    
        [Test]
        public void WhenItemsIsEmpty_ResultIsEmpty()
        {
            var result = InvokeUut(
                index:  0,
                items:  Array.Empty<int>());
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
        }

        public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
            = new[]
            {
                new TestCaseData(0,             1).SetName("{m}(Single item, Minimum index)"),
                new TestCaseData(int.MaxValue,  1).SetName("{m}(Single item, Maximum index)"),
                new TestCaseData(0,             5).SetName("{m}(Multiple items)")
            };
        [TestCaseSource(nameof(Otherwise_TestCases))]
        public void Otherwise_ResultIsClear(
            int index,
            int itemCount)
        {
            var items = Enumerable
                .Range(1, itemCount)
                .ToArray();

            var result = InvokeUut(
                index:  index,
                items:  items);
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Length.Should().Be(itemCount, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.AsRemoval().Index).Should().BeEquivalentTo(Enumerable.Range(index, itemCount).Reverse(), static options => options.WithStrictOrdering(), "the given index of the item range should have been embedded into the generated changes");
            result.Changes.Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(items.Reverse(), static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes, in reverse order");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(
            int                 index,
            IReadOnlyList<int>  items);
    }
}
