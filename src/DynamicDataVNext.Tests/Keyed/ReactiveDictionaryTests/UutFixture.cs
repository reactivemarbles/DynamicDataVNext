using DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

public sealed class UutFixture
    : IReadOnlyDictionaryUutFixture<UutFixture, ReactiveDictionary<string, int>>
{
    public static UutFixture Create(
            IEnumerable<KeyValuePair<string, int>>  items,
            IEqualityComparer<string>?              comparer    = null,
            KeyedItemOptions                        options     = default)
        => new(
            items:      items,
            comparer:   comparer,
            options:    options);
            
    private UutFixture(
        IEnumerable<KeyValuePair<string, int>>  items,
        IEqualityComparer<string>?              comparer    = null,
        KeyedItemOptions                        options     = default)
    {
        var initialItems = items.ToArray();
        
        _uut = new(
            source:     (initialItems.Length is 0)
                ? Signal.Empty<KeyedChangeSet<string, int>>()
                : Signal.Return(KeyedChangeSet.CreateForReset(additions: initialItems)),
            comparer:   comparer,
            options:    options);
    }
    
    public ReactiveDictionary<string, int> Uut
        => _uut;
    
    public IEqualityComparer<string> UutComparer
        => _uut.ChangeStream.KeyComparer;
    
    public KeyedItemOptions UutOptions
        => _uut.ChangeStream.Options;
    
    public void Dispose()
        => _uut.Dispose();
    
    private readonly ReactiveDictionary<string, int> _uut;
}
