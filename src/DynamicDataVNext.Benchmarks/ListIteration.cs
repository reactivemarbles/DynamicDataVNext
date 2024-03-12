using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using BenchmarkDotNet.Attributes;

namespace DynamicDataVNext.Benchmarks;

[MemoryDiagnoser]
public class ListIteration
{
    public ListIteration()
    {
        var items = Enumerable.Range(1, 100_00).ToArray();

        _itemSetsByLength = new()
        {
            [100] = items[0..100].ToList(),
            [1_000] = items[0..1_000].ToList(),
            [10_000] = items[0..10_000].ToList()
        };
    }

    [Params(100, 1_000, 10_000)]
    public int ItemCount{ get; set; }

    [Benchmark(Baseline = true)]
    public int ItemsAsEnumerable()
        => SumEnumerable(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlyListWithForEach()
        => SumReadOnlyListWithForEach(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlyListWithFor()
        => SumReadOnlyListWithFor(_itemSetsByLength[ItemCount]);

    [Benchmark]
    public int ItemsAsReadOnlySpan()
        => SumReadOnlySpan(CollectionsMarshal.AsSpan(_itemSetsByLength[ItemCount]));

    [Benchmark]
    public int ItemsAsList()
        => SumList(_itemSetsByLength[ItemCount]);

    private static int SumEnumerable(IEnumerable<int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item;
        return result;
    }

    private static int SumReadOnlyListWithForEach(IReadOnlyList<int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item;
        return result;
    }

    private static int SumReadOnlyListWithFor(IReadOnlyList<int> items)
    {
        var result = 0;
        for (var index = 0; index < items.Count; ++index)
            result += items[index];
        return result;
    }

    private static int SumReadOnlySpan(ReadOnlySpan<int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item;
        return result;
    }

    private static int SumList(List<int> items)
    {
        var result = 0;
        foreach (var item in items)
            result += item;
        return result;
    }

    private readonly Dictionary<int, List<int>> _itemSetsByLength;
}
