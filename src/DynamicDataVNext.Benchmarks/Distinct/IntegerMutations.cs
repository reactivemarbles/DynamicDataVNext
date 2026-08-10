namespace DynamicDataVNext.Benchmarks.Distinct;

public static class IntegerMutations
{
    public static ImmutableArray<MutationBase<int>> Generate(
        int seed                = 1234567,
        int initialItemCount    = 1_000,
        int mutationCount       = 1_000)
    {
        const int maxItem           = 50;
        const int maxItemRangeSize  = 25;

        var mutations = ImmutableArray.CreateBuilder<MutationBase<int>>(initialCapacity: mutationCount);

        var items       = new HashSet<int>(capacity: initialItemCount);
        var itemsBuffer = ImmutableArray.CreateBuilder<int>(initialCapacity: maxItemRangeSize);

        var faker = new Faker()
        {
            Random = new Randomizer(seed)
        };
        
        mutations.Add(new ResetMutation<int>()
        {
            Items = Enumerable.Repeat(RxVoid.Default, maxItemRangeSize)
                .Select(_ => faker.Random.Int(1,  maxItem))
                .Distinct()
                .ToImmutableArray()
        });
            
        var mutationTypes       = _mutationTypeWeights.Keys.ToArray();
        var mutationTypeWeights = _mutationTypeWeights.Values.ToArray();
        
        while (mutations.Count < (mutationCount - 1))
            mutations.Add(faker.Random.WeightedRandom(
                    items:      mutationTypes,
                    weights:    mutationTypeWeights)
                switch
            {
                MutationType.AddItem               => new AddItemMutation<int>()
                {
                    Item = faker.Random.Int(1, maxItem)
                },
                MutationType.Clear                 => new ClearMutation<int>(),
                MutationType.ExceptWith            => new ExceptWithMutation<int>()
                {
                    Other = Enumerable.Repeat(RxVoid.Default, faker.Random.Int(1, maxItemRangeSize))
                        .Select(_ => faker.Random.Int(1, maxItem))
                        .ToImmutableArray()
                },
                MutationType.IntersectWith         => new IntersectWithMutation<int>()
                {
                    Other = Enumerable.Repeat(RxVoid.Default, faker.Random.Int(1, maxItemRangeSize))
                        .Select(_ => faker.Random.Int(1, maxItem))
                        .ToImmutableArray()
                },
                MutationType.RemoveItem            => new RemoveItemMutation<int>()
                {
                    Item = faker.Random.Int(1, maxItem)
                },
                MutationType.Reset                 => new ResetMutation<int>()
                {
                    Items = Enumerable.Repeat(RxVoid.Default, faker.Random.Int(1, maxItemRangeSize))
                        .Select(_ => faker.Random.Int(1, maxItem))
                        .ToImmutableArray()
                },
                MutationType.SymmetricExceptWith   => new SymmetricExceptWithMutation<int>()
                {
                    Other = Enumerable.Repeat(RxVoid.Default, faker.Random.Int(1, maxItemRangeSize))
                        .Select(_ => faker.Random.Int(1, maxItem))
                        .ToImmutableArray()
                },
                MutationType.UnionWith             => new UnionWithMutation<int>()
                {
                    Other = Enumerable.Repeat(RxVoid.Default, faker.Random.Int(1, maxItemRangeSize))
                        .Select(_ => faker.Random.Int(1, maxItem))
                        .ToImmutableArray()
                },
                _                                       => throw new InvalidOperationException()
            });

        mutations.Add(new ClearMutation<int>());

        return mutations.MoveToImmutable();
    }

    private static readonly IReadOnlyDictionary<MutationType, float> _mutationTypeWeights
        = new Dictionary<MutationType, float>{
            [MutationType.AddItem]             = 0.28f,
            [MutationType.Clear]               = 0.02f,
            [MutationType.ExceptWith]          = 0.10f,
            [MutationType.IntersectWith]       = 0.10f,
            [MutationType.RemoveItem]          = 0.10f,
            [MutationType.Reset]               = 0.02f,
            [MutationType.SymmetricExceptWith] = 0.10f,
            [MutationType.UnionWith]           = 0.28f
        };
}
