namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

public static partial class ChangeStreamTests
{
    public static partial class SourceTests
    {
        [TestFixture]
        public partial class SubscribeTests
        {
            [Test]
            public void WhenDictionaryHasBeenDisposed_ThrowsException()
            {
                var uut = new ReactiveDictionary<string, int>(source: Signal.Silent<KeyedChangeSet<string, int>>());
                
                uut.Dispose();
                
                var result = uut.Invoking(uut => uut.ChangeStream.Source.Subscribe())
                    .Should().Throw<ObjectDisposedException>()
                    .Which;
                
                Console.WriteLine(result);
            }

            [Test]
            public void WhenDictionaryIsEmpty_DoesNothing()
            {
                using var uut = new ReactiveDictionary<string, int>(source: Signal.Silent<KeyedChangeSet<string, int>>());
                
                var observer = new KeyedItemRecordingObserver<string, int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.RecordedNotifications.Should().BeEmpty("no notifications should have been published");
                
                uut.Should().BeEmpty("the set should not have been changed");
            }
        }
    }
}
