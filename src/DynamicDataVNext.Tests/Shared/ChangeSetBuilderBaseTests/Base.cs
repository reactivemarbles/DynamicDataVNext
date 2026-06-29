using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public class Base
{
    public static readonly IReadOnlyList<TestCaseData> IsSourceEmpty_TestCases
        = new[]
        {
            new TestCaseData(true)  .SetName("{m}(Source is empty)"),
            new TestCaseData(false) .SetName("{m}(Source is not empty)")
        };
}
