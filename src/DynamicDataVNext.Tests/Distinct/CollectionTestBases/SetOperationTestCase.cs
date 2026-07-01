using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public class SetOperationTestCase
{
    public string? Because { get; init; }
    
    public required IReadOnlyList<int> Items { get; init; }

    public required IReadOnlyList<int> Other { get; init; }
}
