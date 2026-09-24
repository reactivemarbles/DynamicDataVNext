namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

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
                var uut = new ReactiveHashSet<int>(source: Signal.Silent<DistinctChangeSet<int>>());
                
                uut.Dispose();
                
                var result = uut.Invoking(uut => uut.ChangeStream.Source.Subscribe())
                    .Should().Throw<ObjectDisposedException>()
                    .Which;
                
                Console.WriteLine(result);
            }

            [Test]
            public void WhenSetIsEmpty_DoesNothing()
            {
                using var uut = new ReactiveHashSet<int>(source: Signal.Silent<DistinctChangeSet<int>>());
                
                var observer = new DistinctItemRecordingObserver<int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.RecordedNotifications.Should().BeEmpty("no notifications should have been published");
                
                uut.Should().BeEmpty("the set should not have been changed");
            }
        }
    }
}
