using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public static partial class ChangeStreamTests
{
    public static partial class SourceTests
    {
        [TestFixture]
        public partial class SubscribeTests
        {
            [Test]
            public void WhenSetHasBeenDisposed_ThrowsException()
            {
                var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
                
                uut.Dispose();
                
                var result = uut.Invoking(uut => uut.ChangeStream.Source.Subscribe())
                    .Should().Throw<ObjectDisposedException>()
                    .Which;
                
                Console.WriteLine(result);
            }

            [Test]
            public void WhenSetIsEmpty_DoesNothing()
            {
                using var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
                
                var observer = new KeyedItemRecordingObserver<string, TestItem>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.RecordedNotifications.Should().BeEmpty("no notifications should have been published");
                
                uut.Should().BeEmpty("the set should not have been changed");
            }

            [Test]
            public void WhenNotificationsAreSuspended_SuspendsInitialReset()
            {
                var items = new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                };
                
                using var uut = new ObservableCache<string, TestItem>(
                    items:          items,
                    keySelector:    TestItem.SelectKey);
                
                var suspension = uut.SuspendNotifications();
                
                var observer = new KeyedItemRecordingObserver<string, TestItem>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Should().BeEmpty("the initial reset should have been suspended");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
                
                uut.Should().BeEquivalentTo(items, "the set should not have been changed");

                suspension.Dispose();
                
                var additions = items
                    .Select(static item => new KeyedItem<string, TestItem>()
                    {
                        Key     = item.Key,
                        Item    = item
                    })
                    .ToArray();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Count.Should().Be(1, "the suspended initial reset should have been published");
                observer.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "the suspended initial reset should have been published");
                observer.RecordedChangeSets[0].AsReset().Removals.Should().BeEmpty("the initial reset should contain only initial items");
                observer.RecordedChangeSets[0].AsReset().Additions.Should().BeEquivalentTo(additions, "the initial reset should contain all initial items");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
            }
        }
    }
}
