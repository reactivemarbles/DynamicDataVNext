namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public abstract class MoveTestsBase<TUutFixture, TUut>
    where TUutFixture : IListUutFixture<TUutFixture, TUut>
    where TUut : IList<string?>,
        IMovementAwareList
{
    [TestCase(1, int.MinValue,  TestName = "{m}(Min negative value)")]
    [TestCase(1, -1,            TestName = "{m}(Max negative value)")]
    [TestCase(1, 1,             TestName = "{m}(Single item in list, Upper bound exceeded)")]
    [TestCase(3, 3,             TestName = "{m}(Multiple items in list, Upper bound exceeded)")]
    [TestCase(1, int.MaxValue,  TestName = "{m}(Max positive value)")]
    public void WhenNewIndexIsOutOfRange_DoesNothingAndThrowsException(
        int itemsCount,
        int newIndex)
    {
        var items = Enumerable.Range(1, itemsCount)
            .Select(static item => item.ToString())
            .ToArray();

        using var fixture = TUutFixture.Create(items);
        
        var result = fixture.Invoking(fixture => 
            {
                fixture.Uut.Move(
                    oldIndex:   0,
                    newIndex:   newIndex);
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(newIndex))
            .Which;
            
        Console.WriteLine(result);
        
        fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should not have been changed");
        
        fixture.AssertUutDidNothing();
    }
    
    [TestCase(0, 0,             TestName = "{m}(Empty list)")]
    [TestCase(1, int.MinValue,  TestName = "{m}(Min negative value)")]
    [TestCase(1, -1,            TestName = "{m}(Max negative value)")]
    [TestCase(1, 1,             TestName = "{m}(Single item in list, Upper bound exceeded)")]
    [TestCase(3, 3,             TestName = "{m}(Multiple items in list, Upper bound exceeded)")]
    [TestCase(1, int.MaxValue,  TestName = "{m}(Max positive value)")]
    public void WhenOldIndexIsOutOfRange_DoesNothingAndThrowsException(
        int itemsCount,
        int oldIndex)
    {
        var items = Enumerable.Range(1, itemsCount)
            .Select(static item => item.ToString())
            .ToArray();

        using var fixture = TUutFixture.Create(items);
        
        var result = fixture.Invoking(fixture => 
            {
                fixture.Uut.Move(
                    oldIndex:   oldIndex,
                    newIndex:   0);
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName(nameof(oldIndex))
            .Which;
            
        Console.WriteLine(result);
        
        fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should not have been changed");
        
        fixture.AssertUutDidNothing();
    }
    
    [TestCase(3, 0, 2, TestName = "{m}(Move from front to back)")]
    [TestCase(3, 2, 0, TestName = "{m}(Move from back to front)")]
    [TestCase(3, 1, 2, TestName = "{m}(Move from middle to back)")]
    [TestCase(3, 1, 0, TestName = "{m}(Move from middle to front)")]
    [TestCase(5, 2, 3, TestName = "{m}(Move forward in middle, One position)")]
    [TestCase(5, 2, 1, TestName = "{m}(Move backward in middle, One position)")]
    [TestCase(5, 1, 3, TestName = "{m}(Move forward in middle, Multiple positions)")]
    [TestCase(5, 3, 1, TestName = "{m}(Move backward in middle, Multiple positions)")]
    public void WhenIndexesAreInRange_MovesItem(
        int itemsCount,
        int oldIndex,
        int newIndex)
    {
        var items = Enumerable.Range(1, itemsCount)
            .Select(static item => item.ToString())
            .ToArray();

        using var fixture = TUutFixture.Create(items);
        
        fixture.Uut.Move(
            oldIndex:   oldIndex,
            newIndex:   newIndex);
            
        var finalItems = (oldIndex < newIndex)
            ? Enumerable.Empty<string?>()
                .Concat(items.Take(oldIndex))
                .Concat(items
                    .Skip(oldIndex + 1)
                    .Take(newIndex - oldIndex))
                .Append(items[oldIndex])
                .Concat(items.Skip(newIndex + 1))
            : Enumerable.Empty<string?>()
                .Concat(items.Take(newIndex))
                .Append(items[oldIndex])
                .Concat(items
                    .Skip(newIndex)
                    .Take(oldIndex - newIndex))
                .Concat(items.Skip(oldIndex + 1));
            
        fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the specified move operation should have been performed");
        
        fixture.AssertItemWasMoved(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            movedItem:  items[oldIndex]);
    }

    [TestCase(1, 0, TestName = "{m}(Single item in list)")]
    [TestCase(3, 0, TestName = "{m}(Multiple items in list, First item referenced)")]
    [TestCase(3, 1, TestName = "{m}(Multiple items in list, Median item referenced)")]
    [TestCase(3, 2, TestName = "{m}(Multiple items in list, Last item referenced)")]
    public void WhenIndexesAreTheSame_DoesNothing(
        int itemsCount,
        int index)
    {
        var items = Enumerable.Range(1, itemsCount)
            .Select(static item => item.ToString())
            .ToArray();

        using var fixture = TUutFixture.Create(items);
        
        fixture.Uut.Move(
            oldIndex:   index,
            newIndex:   index);
            
        fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should not have been changed");
        
        fixture.AssertUutDidNothing();
    }
}    
