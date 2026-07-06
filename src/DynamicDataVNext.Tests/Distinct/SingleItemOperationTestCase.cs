using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public class SingleItemOperationTestCase
{
    public required int Item { get; init; }

    public required IReadOnlyList<int> Items { get; init; }
}
