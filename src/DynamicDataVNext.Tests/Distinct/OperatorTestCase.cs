using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public class OperatorTestCase
{
    public required IReadOnlyList<int> Items { get; init; }
    
    public required DistinctChangeSet<int> ChangeSet { get; init; }
}
