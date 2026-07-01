using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public class ItemOperationTestCase
{
    public required int Item { get; init; }

    public required IReadOnlyList<int> Items { get; init; }
}
