using System;
using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed;

public class ValueRangeOperationTestCase
{
    public required IReadOnlyList<KeyValuePair<string, int>> InitialItems { get; init; }

    public required Func<int, string> KeySelector { get; init; }

    public required IReadOnlyList<int> Values { get; init; }
}
