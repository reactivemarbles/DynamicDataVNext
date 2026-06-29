using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests;

public abstract class EnsureCapacityTestsBase<T>
{
    public static readonly IReadOnlyList<TestCaseData> CapacityIsNegative_TestCases
        = new[]
        {
            new TestCaseData(-1)            .SetName("{m}(Max negative value)"),
            new TestCaseData(int.MinValue)  .SetName("{m}(Min negative value)")
        };
    [TestCaseSource(nameof(CapacityIsNegative_TestCases))]
    public void CapacityIsNegative_ThrowsException(int capacity)
    {
        var uut = CreateUut(initialCapacity: 0);
        
        var result = uut.Invoking(uut => EnsureCapacity(uut, capacity))
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
        = new[]
        {
            new TestCaseData(0, 0, "the current capacity was the same")     .SetName("{m}(Empty capacity)"),
            new TestCaseData(1, 1, "the current capacity was the same")     .SetName("{m}(Trivial capacity)"),
            new TestCaseData(1, 2, "the current capacity was insufficient") .SetName("{m}(Capacity is extended)"),
            new TestCaseData(2, 1, "the current capacity was sufficient")   .SetName("{m}(Capacity is sufficient)")
        };
    [TestCaseSource(nameof(Otherwise_TestCases))]
    public void Otherwise_CapacityIsExpected(
        int     initialCapacity,
        int     capacity,
        string  because)
    {
        var uut = CreateUut(initialCapacity: initialCapacity);
        
        EnsureCapacity(uut, capacity);
        
        GetCapacity(uut).Should().BeGreaterThanOrEqualTo(Math.Max(initialCapacity, capacity), because);
    }
    
    protected abstract T CreateUut(int initialCapacity);
    
    protected abstract void EnsureCapacity(
        T   uut,
        int capacity);
    
    protected abstract int GetCapacity(T uut);
}
