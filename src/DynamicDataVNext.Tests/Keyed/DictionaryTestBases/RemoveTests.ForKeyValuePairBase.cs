namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class RemoveTests
{
    public abstract class ForKeyValuePairBase<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(RemoveTests), nameof(WhenDictionaryContainsItem_TestCases))]
        public void WhenDictionaryContainsItem_RemovesItemAndReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(new KeyValuePair<string, int>(testCase.Key, testCase.Value));
            
            result.Should().BeTrue("the collection contained the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Where(item => item.Key != testCase.Key), "the given item should have been removed from the collection");

            if (testCase.InitialItems.Count is 1)
                fixture.AssertUutWasCleared(testCase.InitialItems);
            else
                fixture.AssertItemWasRemoved(
                    removedKey:     testCase.Key,
                    removedValue:   testCase.Value);
        }
        
        [TestCaseSource(typeof(RemoveTests), nameof(WhenDictionaryDoesNotContainItem_TestCases))]
        public void WhenDictionaryDoesNotContainItem_DoesNothingAndReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(new KeyValuePair<string, int>(testCase.Key, testCase.Value));
            
            result.Should().BeFalse("the collection did not contain the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [Test]
        public void WhenItemKeyIsNull_DoesNothingAndThrowsException()
        {
            using var fixture = TUutFixture.Create(new[] { new KeyValuePair<string, int>("1", 1) });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.Uut.Remove(new KeyValuePair<string, int>(null!, 1));
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
