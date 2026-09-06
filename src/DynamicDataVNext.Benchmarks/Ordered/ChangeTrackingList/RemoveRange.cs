namespace DynamicDataVNext.Benchmarks.Ordered.ChangeTrackingList;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class RemoveRange
{
    static RemoveRange()
    {
        _itemSetsByCount                = new();
        _mutationsByItemCountAndType    = new();

        var randomizer = new Randomizer(1234567);

        foreach (var itemCount in new[] { 10, 100, 1_000, 10_000 })
        {
            _itemSetsByCount.Add(
                key:    itemCount,
                value:  Enumerable.Repeat(randomizer, itemCount)
                    .Select(static randomizer => randomizer.Int())
                    .ToImmutableArray());

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, RangeType.EntireList),
                value:  new RemoveRangeMutation<ChangeTrackingList<int>, int>()
                {
                    Index = 0,
                    Count = itemCount
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, RangeType.FrontHalf),
                value:  new RemoveRangeMutation<ChangeTrackingList<int>, int>()
                {
                    Index = 0,
                    Count = itemCount / 2
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, RangeType.BackHalf),
                value:  new RemoveRangeMutation<ChangeTrackingList<int>, int>()
                {
                    Index = itemCount / 2,
                    Count = itemCount / 2
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, RangeType.MiddleHalf),
                value:  new RemoveRangeMutation<ChangeTrackingList<int>, int>()
                {
                    Index = itemCount / 4,
                    Count = itemCount / 2
                });
        }
    }

    [Params(10, 100, 1_000, 10_000)]
    public int ItemCount { get; set; }
    
    public enum RangeType
    {
        EntireList,
        FrontHalf,
        BackHalf,
        MiddleHalf
    }
    
    [ParamsAllValues]
    public RangeType Type { get; set; }
    
    [Benchmark(Baseline = true)]
    public void CurrentImplementation()
    {
        var list = new ChangeTrackingList<int>(items: _itemSetsByCount[ItemCount]);

        var mutation = _mutationsByItemCountAndType[(ItemCount, Type)];

        list.RemoveRange(
            index: mutation.Index,
            count: mutation.Count);
    }
    
    private static readonly Dictionary<int, ImmutableArray<int>>                                                            _itemSetsByCount;
    private static readonly Dictionary<(int itemCount, RangeType type), RemoveRangeMutation<ChangeTrackingList<int>, int>>  _mutationsByItemCountAndType;
}
