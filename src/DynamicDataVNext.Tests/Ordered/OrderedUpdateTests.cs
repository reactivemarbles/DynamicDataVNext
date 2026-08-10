namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedUpdateTests
{
    [Test]
    public void NewIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedUpdate<int>()
                {
                    NewIndex    = -1,
                    NewItem     = 2, 
                    OldIndex    = 0,
                    OldItem     = 3
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
                _ = new OrderedUpdate<int>()
                {
                    NewIndex    = 1,
                    NewItem     = 2, 
                    OldIndex    = -1,
                    OldItem     = 3
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
        var oldItem = 1;
        var newItem = 2;
    
        var result = new OrderedUpdate<int>()
        {
            NewIndex    = newIndex,
            NewItem     = newItem, 
            OldIndex    = oldIndex,
            OldItem     = oldItem
        };
        
        result.NewIndex.Should().Be(newIndex);
        result.NewItem.Should().Be(newItem);
        result.OldIndex.Should().Be(oldIndex);
        result.OldItem.Should().Be(oldItem);
    }
}
