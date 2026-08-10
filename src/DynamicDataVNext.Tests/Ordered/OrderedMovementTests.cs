namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedMovementTests
{
    [Test]
    public void NewIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedMovement<int>()
                {
                    Item        = 1, 
                    NewIndex    = -1,
                    OldIndex    = 0
                };
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }

    [Test]
    public void OldIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedMovement<int>()
                {
                    Item        = 1, 
                    NewIndex    = 0,
                    OldIndex    = -1
                };
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }

    [TestCase(0,             0,             TestName = "{m}(Minimum indexes)")]
    [TestCase(int.MaxValue,  int.MaxValue,  TestName = "{m}(Maximum indexes)")]
    public void Otherwise_ResultIsValid(
        int oldIndex,
        int newIndex)
    {
        var item = 1;
    
        var result = new OrderedMovement<int>()
        {
            Item        = item,
            NewIndex    = newIndex,
            OldIndex    = oldIndex
        };
        
        result.Item.Should().Be(item);
        result.NewIndex.Should().Be(newIndex);
        result.OldIndex.Should().Be(oldIndex);
    }
}
