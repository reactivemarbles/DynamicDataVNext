using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

namespace DynamicDataVNext.Benchmarks;

[MemoryDiagnoser]
public class DictionaryIteration
{
    public DictionaryIteration()
    {
        var values = Enumerable.Range(1, 100_00).ToArray();

        _itemSetsByLength = new()
        {
            [100] = values[0..100].ToDictionary(x => x),
            [1_000] = values[0..1_000].ToDictionary(x => x),
            [10_000] = values[0..10_000].ToDictionary(x => x)
        };
    }

    [Params(100, 1_000, 10_000)]
    public int ItemCount{ get; set; }

    [Benchmark(Baseline = true)]
    public int ItemsAsEnumerable()
        => SumEnumerable(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlyDictionaryOverItems()
        => SumReadOnlyDictionaryOverItems(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlyDictionaryOverKeys()
        => SumReadOnlyDictionaryOverKeys(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlyDictionaryOverValues()
        => SumReadOnlyDictionaryOverValues(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsDictionaryOverItems()
        => SumDictionaryOverItems(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsDictionaryOverKeys()
        => SumDictionaryOverKeys(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsDictionaryOverValues()
        => SumDictionaryOverValues(_itemSetsByLength[ItemCount]);

    private static int SumEnumerable(IEnumerable<KeyValuePair<int, int>> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item.Value;
        return result;
    }

    private static int SumReadOnlyDictionaryOverItems(IReadOnlyDictionary<int, int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item.Value;
        return result;
    }

    private static int SumReadOnlyDictionaryOverKeys(IReadOnlyDictionary<int, int> items)
    {
        var result = 0;
        foreach (var key in items.Keys)
            result += items[key];
        return result;
    }

    private static int SumReadOnlyDictionaryOverValues(IReadOnlyDictionary<int, int> items)
    {
        var result = 0;
        foreach (var value in items.Values)
            result += value;
        return result;
    }

    private static int SumDictionaryOverItems(Dictionary<int, int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item.Value;
        return result;
    }

    private static int SumDictionaryOverKeys(Dictionary<int, int> items)
    {
        var result = 0;
        foreach (var key in items.Keys)
            result += items[key];
        return result;
    }

    private static int SumDictionaryOverValues(Dictionary<int, int> items)
    {
        var result = 0;
        foreach (var value in items.Values)
            result += value;
        return result;
    }

    private readonly Dictionary<int, Dictionary<int, int>> _itemSetsByLength;
}
