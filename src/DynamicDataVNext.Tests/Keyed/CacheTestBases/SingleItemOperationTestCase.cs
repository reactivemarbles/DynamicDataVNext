using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public class SingleItemOperationTestCase
{
    public required TestItem Item { get; init; }

    public required IReadOnlyList<TestItem> InitialItems { get; init; }
}
