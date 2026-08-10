namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class ApplyToTests
{
    [TestFixture]
    public class ForISet
    {
        [Test]
        public void WhenChangeSetIsEmpty_TargetIsUnchanged()
        {
            var items = new[] { 1, 2, 3 };
            
            var target = new HashSet<int>(items);

            DistinctChangeSet<int>.Empty.ApplyTo(target);
            
            target.Should().BeEquivalentTo(items, static config => config.WithoutStrictOrdering(), "no changes were applied");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsNotEmpty_TestCases))]
        public void WhenChangeSetIsNotEmpty_TargetIsExpected(TestCase testCase)
        {
            var target = new HashSet<int>(testCase.TargetItems);

            testCase.ChangeSet.ApplyTo(target: target);
            
            target.Should().BeEquivalentTo(testCase.ExpectedItems, static config => config.WithoutStrictOrdering(), "the given changes should have been applied");
        }

        [Test]
        public void WhenTargetDoesNotSupportRefreshment_RefreshmentIsIgnored()
        {
            var changeSet = DistinctChangeSet.CreateForRefreshment(2);
            
            var target = new HashSet<int>() { 1, 2, 3 };
            
            var items = target.ToArray();
            
            changeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "refreshment changes are not supported");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshments_TestCases))]
        public void WhenChangeSetContainsOnlyRefreshmentsAndTargetIsChangeTrackingHashSet_RefreshmentIsApplied(TestCase testCase)
        {
            var target = new ChangeTrackingHashSet<int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            var items = target.ToArray();

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(items, "no mutations should have been made");
            
            target.BufferedChanges.Should().BeEquivalentTo(testCase.ChangeSet.Changes, options => options.WithStrictOrdering(), "The refreshment changes should have been applied and captured");
        }

        [Test]
        public void WhenTargetIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() => 
                {
                    default(DistinctChangeSet<int>).ApplyTo(target: (null as ISet<int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("target")
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshments_TestCases))]
        public void WhenChangeSetContainsOnlyRefreshmentsAndTargetIsObservableHashSet_RefreshmentIsApplied(TestCase testCase)
        {
            using var target = new ObservableHashSet<int>(
                items:      testCase.TargetItems,
                options:    new() { ItemsAreMutable = true });

            using var subscription = target.ChangeStream
                .RecordItems(out var results);
            results.ClearNotifications();

            testCase.ChangeSet.ApplyTo(target);
            
            target.Should().BeEquivalentTo(testCase.ExpectedItems, "no mutations should have been made");
            results.RecordedChangeSets.Should().ContainSingle("a single refreshment operation should have been performed");
            results.RecordedChangeSets[0].Changes.Should().BeEquivalentTo(testCase.ChangeSet.Changes, options => options.WithStrictOrdering(), "The refreshment changes should have been applied and replicated");
        }
    }
}
