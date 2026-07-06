using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests;

public abstract class EnsureCapacityTestsBase<T>
{
    [TestCase(-1,           TestName = "{m}(Max negative value)")]
    [TestCase(int.MinValue, TestName = "{m}(Min negative value)")]
    public void CapacityIsNegative_ThrowsException(int capacity)
    {
        var uut = CreateUut(initialCapacity: 0);
        
        var result = uut.Invoking(uut => EnsureCapacity(uut, capacity))
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }
    
    [TestCase(0, 0, "the current capacity was the same",        TestName = "{m}(Empty capacity)")]
    [TestCase(1, 1, "the current capacity was the same",        TestName = "{m}(Trivial capacity)")]
    [TestCase(1, 2, "the current capacity was insufficient",    TestName = "{m}(Capacity is extended)")]
    [TestCase(2, 1, "the current capacity was sufficient",      TestName = "{m}(Capacity is sufficient)")]
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
