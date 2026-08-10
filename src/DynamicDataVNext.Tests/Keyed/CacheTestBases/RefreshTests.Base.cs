namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RefreshTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(RefreshTests), nameof(WhenCacheContainsItem_TestCases))]
        public void WhenCacheContainsItem_RefreshesItemAndReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems,
                options:        new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUutItem(testCase.Item);
            
            result.Should().BeTrue("the collection contains the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertItemWasRefreshed(testCase.Item);
        }
        
        [TestCaseSource(typeof(RefreshTests), nameof(WhenCacheDoesNotContainItem_TestCases))]
        public void WhenCacheDoesNotContainItem_DoesNothingAndReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems,
                options:        new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUutItem(testCase.Item);
            
            result.Should().BeFalse("the collection does not contain the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");

            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenItemKeyIsNull_DoesNothingAndThrowsException()
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
                    _ = fixture.RefreshUutItem(new() { Key = null! });
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
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
                    _ = fixture.RefreshUutItem(item);
                })
                .Should().Throw<ImmutableRefreshException>()
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
