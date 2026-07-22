using System;
using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed;

public interface IReadOnlyDictionaryUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IReadOnlyDictionaryUutFixture<TUutFixture, TUut>
    where TUut : IReadOnlyDictionary<string, int>
{
    static abstract TUutFixture Create(
        IEnumerable<KeyValuePair<string, int>>  items,
        IEqualityComparer<string>?              comparer    = null,
        KeyedItemOptions                        options     = default);
    
    TUut Uut { get; }
    
    IEqualityComparer<string> UutComparer { get; }
    
    KeyedItemOptions UutOptions { get; }
}
