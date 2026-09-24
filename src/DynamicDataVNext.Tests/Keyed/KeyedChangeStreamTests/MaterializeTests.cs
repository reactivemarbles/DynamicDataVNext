namespace DynamicDataVNext.Tests.Keyed.KeyedChangeStreamTests;

[TestFixture]
public class MaterializeTests
{
    [Test]
    public void Always_ResultIsConstructedFromStream()
    {
        using var source = new Signal<KeyedChangeSet<string, int>>(); 

        var stream = new KeyedChangeStream<string, int>()
        {
            KeyComparer = StringComparer.OrdinalIgnoreCase,
            Options     = new KeyedItemOptions()
            {
                ItemsAreMutable = true
            },
            Source      = source
        };
        
        var result = stream.Materialize();
        
        result.Should().NotBeNull();
        result.ChangeStream.KeyComparer.Should().BeSameAs(stream.KeyComparer);
        result.ChangeStream.Options.Should().Be(stream.Options);
        
        result.Should().BeEmpty("no items have been added to the collection");
        
        var items = new KeyValuePair<string, int>[]
        {
            new("1", 1),
            new("2", 2),
            new("3", 3)
        };
        source.OnNext(KeyedChangeSet.CreateForReset(items));
        
        result.Should().BeEquivalentTo(items, "items were added to the collection");
    }
}
