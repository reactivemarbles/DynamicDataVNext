using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForReplacementTests
{
    public abstract class Base
    {
        [Test]
        public void Always_ResultIsUpdate()
        {
            var key = 1;
            var oldItem = 2;
            var newItem = 3;
            
            var result = InvokeUut(
                key:        key,
                oldItem:    oldItem,
                newItem:    newItem);
                    
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single replacement operation was given");
            result.Changes[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement operation was given");
            result.Changes[0].AsReplacement().Key.Should().Be(key, "a single replacement operation was given");
            result.Changes[0].AsReplacement().OldItem.Should().Be(oldItem, "a single replacement operation was given");
            result.Changes[0].AsReplacement().NewItem.Should().Be(newItem, "a single replacement operation was given");
        }
        
        protected abstract KeyedChangeSet<int, int> InvokeUut(
            int key,
            int oldItem,
            int newItem);
    }
}
