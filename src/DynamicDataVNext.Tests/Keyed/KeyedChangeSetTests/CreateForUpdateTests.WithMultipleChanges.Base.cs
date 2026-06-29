using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        public abstract class Base
            : KeyedChangeSetTests.Base
        {
            [Test]
            public void WhenItemsIsEmpty_ResultIsEmpty()
            {
                var result = InvokeUut(changes: Array.Empty<KeyedChange<int, int>>());
            
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
            }

            [TestCase(1, TestName = "{m}(Single change)")]
            [TestCase(5, TestName = "{m}(Multiple changes)")]
            public void Otherwise_ResultContainsChanges(int changeCount)
            {
                var changes = Enumerable
                    .Range(1, changeCount)
                    .Select(static item => KeyedChange.CreateAddition(
                        key:    SelectKey(item),
                        item:   item))
                    .ToArray();

                var result = InvokeUut(changes: changes);
            
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
                result.Changes.Should().BeEquivalentTo(changes, static config => config.WithStrictOrdering(), "all changes should be listed");
            }
            
            protected abstract KeyedChangeSet<int, int> InvokeUut(IEnumerable<KeyedChange<int, int>> changes); 
        }
    }
}
