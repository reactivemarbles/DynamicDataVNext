namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RefreshKeyTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(RefreshKeyTests), nameof(WhenCacheContainsKey_TestCases))]
        public void WhenCacheContainsKey_RefreshesItemAndReturnsTrue(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems,
                options:        new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUutKey(testCase.Key);
            
            result.Should().BeTrue("the collection contains the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertKeyWasRefreshed(testCase.Key);
        }
        
        [TestCaseSource(typeof(RefreshKeyTests), nameof(WhenCacheDoesNotContainKey_TestCases))]
        public void WhenCacheDoesNotContainKey_DoesNothingAndReturnsFalse(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems,
                options:        new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUutKey(testCase.Key);
            
            result.Should().BeFalse("the collection does not contain the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");

            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenKeyIsNull_DoesNothingAndThrowsException()
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          new[] { new TestItem() { Key = "1" } },
                options:        new()
                {
                    ItemsAreMutable = true
                });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.RefreshUutKey(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenItemsAreNotMutable_DoesNothingAndThrowsException()
        {
            var item = new TestItem() { Key = "1" };
        
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          new[] { item },
                options:        new()
                {
                    ItemsAreMutable = false
                });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.RefreshUutKey(item.Key);
                })
                .Should().Throw<ImmutableRefreshException>()
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
