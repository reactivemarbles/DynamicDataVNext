namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

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
                var uut = new ObservableHashSet<int>();
                
                uut.Dispose();
                
                var result = uut.Invoking(uut => uut.ChangeStream.Source.Subscribe())
                    .Should().Throw<ObjectDisposedException>()
                    .Which;
                
                Console.WriteLine(result);
            }

            [Test]
            public void WhenSetIsEmpty_DoesNothing()
            {
                using var uut = new ObservableHashSet<int>();
                
                var observer = new DistinctItemRecordingObserver<int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.RecordedNotifications.Should().BeEmpty("no notifications should have been published");
                
                uut.Should().BeEmpty("the set should not have been changed");
            }

            [Test]
            public void WhenNotificationsAreSuspended_SuspendsInitialReset()
            {
                var items = new[] { 1, 2, 3 };
                
                using var uut = new ObservableHashSet<int>(items: items);
                
                var suspension = uut.SuspendNotifications();
                
                var observer = new DistinctItemRecordingObserver<int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Should().BeEmpty("the initial reset should have been suspended");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
                
                uut.Should().BeEquivalentTo(items, "the set should not have been changed");

                suspension.Dispose();
                
                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Count.Should().Be(1, "the suspended initial reset should have been published");
                observer.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "the suspended initial reset should have been published");
                observer.RecordedChangeSets[0].AsReset().Removals.Should().BeEmpty("the initial reset should contain only initial items");
                observer.RecordedChangeSets[0].AsReset().Additions.Should().BeEquivalentTo(items, "the initial reset should contain all initial items");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
            }
        }
    }
}
