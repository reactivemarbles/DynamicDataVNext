using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRefreshmentTests
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
            result.Changes.Should().ContainSingle("a single refreshed item was given");
            result.Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshed item was given");
            result.Changes[0].AsRefreshment().Key.Should().Be(key, "a single refreshed item was given");
            result.Changes[0].AsRefreshment().Item.Should().Be(item, "a single refreshed item was given");
        }
        
        protected abstract KeyedChangeSet<int, int> InvokeUut(
            int key,
            int item);
    }
}
