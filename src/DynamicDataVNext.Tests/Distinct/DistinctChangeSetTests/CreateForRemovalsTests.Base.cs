using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForRemovalsTests
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
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Length.Should().Be(itemCount, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.Item).Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "a removal change should have been generated for each removed item");
        }
        
        protected abstract DistinctChangeSet<int> InvokeUut(IEnumerable<int> items);
    }
}
