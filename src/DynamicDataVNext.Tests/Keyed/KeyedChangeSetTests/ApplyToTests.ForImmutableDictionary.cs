using System.Diagnostics.Tracing;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class ApplyToTests
{
    [TestFixture]
    public class ForImmutableDictionary
    {
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshmentsAndIsNotConsistentWithTarget_TestCases))]
        public void WhenChangeSetContainsOnlyRefreshments_TestCases_ResultIsTarget(TargetShouldNotChangeTestCase testCase)
        {
            var target = ImmutableDictionary.CreateRange(items: testCase.TargetItems.ToArray());
            
            var result = testCase.ChangeSet.ApplyTo(target);
            
            result.Should().BeSameAs(target, "refreshment changes are not supported");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(TargetItems_TestCases))]
        public void WhenChangeSetIsEmpty_ResultIsTarget(IReadOnlyList<KeyValuePair<string, int>> targetItems)
        {
            var target = ImmutableDictionary.CreateRange(targetItems);
            
            var result = KeyedChangeSet<string, int>.Empty.ApplyTo(target);
            
            result.Should().BeSameAs(target, "no changes were applied");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsResetAndConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsResetAndNotConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsClearAndConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsClearAndNotConsistentWithTarget_TestCases))]
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsUpdateAndConsistentWithTarget_TestCases))]
        public void WhenChangeSetCanBeAppliedToTarget_ResultIsExpected(TargetShouldChangeTestCase testCase)
        {
            var target = ImmutableDictionary.CreateRange(testCase.TargetItems); 
        
            var result = testCase.ChangeSet.ApplyTo(target);
            
            result.Should().BeEquivalentTo(testCase.ExpectedItems, "the given changes should have been applied");
        }

        [Test]
        public void WhenTargetIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() => 
                {
                    _ = default(KeyedChangeSet<string, int>).ApplyTo(target: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("target")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
