using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public partial class CreateForClearTests
{
    public abstract class Base
    {
        [Test]
        public void WhenItemsIsEmpty_ResultIsEmpty()
        {
            var result = InvokeUut(items: Array.Empty<int>());
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
        }

        public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
            = new[]
            {
                new TestCaseData(1).SetName("{m}(Single item)"),
                new TestCaseData(5).SetName("{m}(Multiple items)")
            };
        [TestCaseSource(nameof(Otherwise_TestCases))]
        public void Otherwise_ResultIsClear(int itemCount)
        {
            var items = Enumerable
                .Range(1, itemCount)
                .ToArray();

            var result = InvokeUut(items: items);
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Clear, "a clear operation should have been constructed");
            result.Changes.Length.Should().Be(itemCount, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.AsRemoval().Index).Should().BeEquivalentTo(Enumerable.Range(0, itemCount).Reverse(), static options => options.WithStrictOrdering(), "the changes should have been sequenced by index, in reverse order");
            result.Changes.Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(items.Reverse(), static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes, in reverse order");
            result.AsClear().ReversedItems.Should().BeEquivalentTo(items.Reverse(), static options => options.WithStrictOrdering(), "all removed items should be listed, in reverse order");
        }
        
        protected abstract OrderedChangeSet<int> InvokeUut(IReadOnlyList<int> items);
    }
}
