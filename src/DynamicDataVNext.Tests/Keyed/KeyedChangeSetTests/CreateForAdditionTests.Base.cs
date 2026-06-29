using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionTests
{
    public abstract class Base
    {
        [Test]
        public void Always_ResultIsUpdate()
        {
            var key = 1;
            var item = 2;
            
            var result = InvokeUut(
                key:    key,
                item:   item);
                    
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().ContainSingle("a single added item was given");
            result.Changes[0].Type.Should().Be(KeyedChangeType.Addition, "a single added item was given");
            result.Changes[0].AsAddition().Key.Should().Be(key, "a single added item was given");
            result.Changes[0].AsAddition().Item.Should().Be(item, "a single added item was given");
        }
        
        protected abstract KeyedChangeSet<int, int> InvokeUut(
            int key,
            int item);
    }
}
