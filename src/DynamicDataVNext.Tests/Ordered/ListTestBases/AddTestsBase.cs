namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public abstract class AddTestsBase<TUutFixture, TUut>
    where TUutFixture : IListUutFixture<TUutFixture, TUut>
    where TUut : IList<string?>
{
    [Test]
    public void WhenListIsEmpty_ResetsListToItem()
    {
        using var fixture = TUutFixture.Create();
        
        var item = "1";
        fixture.Uut.Add(item);
        
        fixture.Uut.Should().BeEquivalentTo(new[] { item }, "the given item should have been added to the collection");

        fixture.AssertUutWasReset(
            removedItems:   Array.Empty<string?>(),
            insertedItems:  new[] { item });
    }

    [TestCase(1, "2", TestName = "{m}(Single item in list)")]
    [TestCase(3, "4", TestName = "{m}(Multiple items in list)")]
    [TestCase(3, "2", TestName = "{m}(Duplicate item added to list)")]
    public void WhenListIsNotEmpty_AppendsItem(
        int     initialItemCount,
        string? item)
    {
        var initialItems = Enumerable.Range(1, initialItemCount)
            .Select(static item => item.ToString())
            .ToArray();
    
        using var fixture = TUutFixture.Create(items: initialItems);
        
        fixture.Uut.Add(item);
        
        var finalItems = initialItems
            .Append(item)
            .ToArray();
            
        fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the given item should have been appended to the collection");

        fixture.AssertItemWasInserted(
            insertionIndex: initialItemCount,
            insertedItem:   item);
    }
}
