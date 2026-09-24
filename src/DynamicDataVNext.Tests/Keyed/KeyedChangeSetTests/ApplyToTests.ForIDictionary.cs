namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class ApplyToTests
{
    [TestFixture]
    public class ForIDictionary
    {
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsResetAndConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsResetAndNotConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsClearAndConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsClearAndNotConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsUpdateAndConsistentWithTarget_TestCases))]
        public void WhenChangeSetCanBeAppliedToTarget_TargetIsExpected(TargetShouldChangeTestCase testCase)
        {
            var target = new Dictionary<string, int>(testCase.TargetItems);
        
            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.ExpectedItems, "the given changes should have been applied");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(TargetItems_TestCases))]
        public void WhenChangeSetIsEmpty_TargetIsUnchanged(IReadOnlyList<KeyValuePair<string, int>> targetItems)
        {
            var target = new Dictionary<string, int>(targetItems);

            KeyedChangeSet<string, int>.Empty.ApplyTo(target);
            
            target.Should().BeEquivalentTo(targetItems, "no changes were applied");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsNotConsistentWithTarget_TestCases))]
        public void WhenTargetDoesNotSupportRefreshment_RefreshmentIsIgnored(TargetShouldNotChangeTestCase testCase)
        {
            var target = new Dictionary<string, int>(testCase.TargetItems);
            
            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.TargetItems, "refreshment changes are not supported");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsConsistentWithTarget_TestCases))]
        public void WhenTargetIsChangeTrackingDictionary_ConsistentRefreshmentsAreBuffered(TargetShouldNotChangeTestCase testCase)
        {
            var target = new ChangeTrackingDictionary<string, int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.TargetItems, "no mutations should have been made");
            
            target.BufferedChanges.Should().BeEquivalentTo(testCase.ChangeSet.Changes, "The refreshment changes should have been applied and captured");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsNotConsistentWithTarget_TestCases))]
        public void WhenTargetIsChangeTrackingDictionary_InconsistentRefreshmentsAreIgnored(TargetShouldNotChangeTestCase testCase)
        {
            var target = new ChangeTrackingDictionary<string, int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.TargetItems, "no mutations should have been made");
            
            // Not checking target.BufferedChanges, because refreshes are applied one-at-a-time, not atomically.
            // Some of them might actually persist, even if others fail.
        }

        [Test]
        public void WhenTargetIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() => 
                {
                    default(KeyedChangeSet<string, int>).ApplyTo(target: (null as IDictionary<string, int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("target")
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsConsistentWithTarget_TestCases))]
        public void WhenTargetIsObservableDictionary_ConsistentRefreshmentsArePublished(TargetShouldNotChangeTestCase testCase)
        {
            using var target = new ObservableDictionary<string, int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            using var subscription = target.ChangeStream
                .RecordItems(out var results);
            results.ClearNotifications();

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.TargetItems, "no mutations should have been made");
            results.RecordedChangeSets.Should().ContainSingle("a single update operation should have been performed");
            results.RecordedChangeSets[0].Changes.Should().BeEquivalentTo(testCase.ChangeSet.Changes, options => options.WithStrictOrdering(), "The refreshment changes should have been applied and replicated");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsNotConsistentWithTarget_TestCases))]
        public void WhenTargetIsObservableDictionary_InconsistentRefreshmentsAreIgnored(TargetShouldNotChangeTestCase testCase)
        {
            using var target = new ObservableDictionary<string, int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            using var subscription = target.ChangeStream
                .RecordItems(out var results);
            results.ClearNotifications();

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.TargetItems, "no mutations should have been made");

            // Not checking for captured changes, because refreshes are applied one-at-a-time, not atomically.
            // Some of them might actually complete, and be published, even if others fail.
        }
    }
}
