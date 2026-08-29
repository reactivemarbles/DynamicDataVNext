namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public abstract class ClearTestsBase<TUutFixture, TUut>
    where TUutFixture : IListUutFixture<TUutFixture, TUut>
    where TUut : IList<string?>
{
    [Test]
    public void WhenListIsEmpty_DoesNothing()
    {
        using var fixture = TUutFixture.Create();
        
        fixture.Uut.Clear();
        
        fixture.Uut.Should().BeEmpty("the collection should not have been changed");
        
        fixture.AssertUutDidNothing();
    }
    
    [TestCase(1, TestName = "{m}(Single item in list)")]
    [TestCase(3, TestName = "{m}(Multiple items in list)")]
    public void WhenListIsNotEmpty_ClearsList(int itemsCount)
    {
        var items = Enumerable.Range(1, itemsCount)
            .Select(static item => item.ToString())
            .ToArray();
        
        using var fixture = TUutFixture.Create(items);
        
        fixture.Uut.Clear();
        
        fixture.Uut.Should().BeEmpty("all items in the collection should have been removed");
        
        fixture.AssertUutWasCleared(items);
    }
}
