namespace DynamicDataVNext.Benchmarks.Ordered.ChangeTrackingList;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class Move
{
    static Move()
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
                key:    (itemCount, MovementType.FrontToBack),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = 0,
                    NewIndex = itemCount - 1
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, MovementType.BackToFront),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = itemCount - 1,
                    NewIndex = 0
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, MovementType.MiddleToFront),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = itemCount / 2,
                    NewIndex = 0
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, MovementType.MiddleToBack),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = itemCount / 2,
                    NewIndex = itemCount - 1
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, MovementType.MiddleForwards),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = itemCount / 2,
                    NewIndex = itemCount * 3 / 4
                });

            _mutationsByItemCountAndType.Add(
                key:    (itemCount, MovementType.MiddleBackwards),
                value:  new MoveMutation<ChangeTrackingList<int>>()
                {
                    OldIndex = itemCount / 2,
                    NewIndex = itemCount / 4
                });
        }
    }

    [Params(10, 100, 1_000, 10_000)]
    public int ItemCount { get; set; }
    
    public enum MovementType
    {
        FrontToBack,
        BackToFront,
        MiddleToFront,
        MiddleToBack,
        MiddleForwards,
        MiddleBackwards
    }

    [ParamsAllValues]
    public MovementType Type { get; set; }
    
    [Benchmark(Baseline = true)]
    public void CurrentImplementation()
    {
        var list = new ChangeTrackingList<int>(items: _itemSetsByCount[ItemCount]);

        var mutation = _mutationsByItemCountAndType[(ItemCount, Type)];

        list.Move(
            oldIndex: mutation.OldIndex,
            newIndex: mutation.NewIndex);
    }
    
    private static readonly Dictionary<int, ImmutableArray<int>>                                                    _itemSetsByCount;
    private static readonly Dictionary<(int itemCount, MovementType type), MoveMutation<ChangeTrackingList<int>>>   _mutationsByItemCountAndType;
}
