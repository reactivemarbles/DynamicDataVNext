using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed;

public class SingleKeyOperationTestCase
{
    public required string Key { get; init; }

    public required IReadOnlyList<KeyValuePair<string, int>> InitialItems { get; init; }
}
