namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class ApplyToTests
{
    [TestFixture]
    public class ForImmutableHashSet
    {
        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetContainsOnlyRefreshments_TestCases))]
        public void WhenChangeSetContainsOnlyRefreshments_TestCases_RefreshmentIsIgnored(TestCase testCase)
        {
            var target = ImmutableHashSet.Create(items: testCase.TargetItems.ToArray());
            
            var result = testCase.ChangeSet.ApplyTo(target);
            
            result.Should().BeSameAs(target, "refreshment changes are not supported");
        }

        [Test]
        public void WhenChangeSetIsEmpty_ResultIsTarget()
        {
            var target = ImmutableHashSet.Create<int>();
            
            var result = DistinctChangeSet<int>.Empty.ApplyTo(target);
            
            result.Should().BeSameAs(target, "no changes were applied");
        }

        [TestCaseSource(typeof(ApplyToTests), nameof(WhenChangeSetIsNotEmpty_TestCases))]
        public void WhenChangeSetIsNotEmpty_ResultIsExpected(TestCase testCase)
        {
            var result = testCase.ChangeSet.ApplyTo(target: ImmutableHashSet.CreateRange(testCase.TargetItems));
            
            result.Should().BeEquivalentTo(testCase.ExpectedItems, static config => config.WithoutStrictOrdering(), "the given changes should have been applied");
        }

        [Test]
        public void WhenTargetIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() => 
                {
                    _ = default(DistinctChangeSet<int>).ApplyTo(target: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("target")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
