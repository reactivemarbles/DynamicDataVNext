using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static class CopyToTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCase(1,    0,  0,  TestName = "{m}(Empty array)")]
        [TestCase(5,    4,  0,  TestName = "{m}(Array too small)")]
        [TestCase(5,    5,  1,  TestName = "{m}(Start offset too large)")]
        public void WhenArrayBoundsAreExceeded_ThrowsException(
            int itemCount,
            int arrayLength,
            int arrayIndex)
        {
            using var fixture = TUutFixture.Create(items: Enumerable.Range(1, itemCount));

            var array = new int[arrayLength];
            
            var result = fixture.Uut.Invoking(uut => uut.CopyTo(
                    array:      array,
                    arrayIndex: arrayIndex))
                .Should().Throw<ArgumentException>()
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCase(-1,           TestName = "{m}(Max Value)")]
        [TestCase(int.MinValue, TestName = "{m}(Min Value)")]
        public void WhenArrayIndexIsNegative_ThrowsException(int arrayIndex)
        {
            using var fixture = TUutFixture.Create();

            var array = new int[1];
            
            var result = fixture.Uut.Invoking(uut => uut.CopyTo(
                    array:      array,
                    arrayIndex: arrayIndex))
                .Should().Throw<ArgumentException>()
                .WithParameterName(nameof(arrayIndex))
                .Which;
            
            Console.WriteLine(result);
        }
        
        [Test]
        public void WhenArrayIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create();

            var result = fixture.Uut.Invoking(uut => uut.CopyTo(
                    array:      null!,
                    arrayIndex: 0))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("array")
                .Which;
            
            Console.WriteLine(result);
        }
        
        [TestCase(0,    0,  0,  TestName = "{m}(Empty set and array)")]
        [TestCase(0,    1,  0,  TestName = "{m}(Empty set)")]
        [TestCase(1,    1,  0,  TestName = "{m}(Single item in set)")]
        [TestCase(5,    5,  0,  TestName = "{m}(Multiple items in set)")]
        [TestCase(5,    10, 5,  TestName = "{m}(Unwritten space before items)")]
        [TestCase(5,    10, 0,  TestName = "{m}(Unwritten space after items)")]
        public void WhenInputsAreAcceptable_CopiesItemsIntoArrayStartingAtArrayIndex(
            int itemCount,
            int arrayLength,
            int arrayIndex)
        {
            var items = Enumerable.Range(1, itemCount).ToArray();
            
            using var fixture = TUutFixture.Create(items: items);
            
            var array = new int[arrayLength];
            Array.Fill(array, 0);
            
            fixture.Uut.CopyTo(
                array:      array,
                arrayIndex: arrayIndex);
            
            array[arrayIndex..(arrayIndex + items.Length)].Should().BeEquivalentTo(items, "all items in the set should have been copied to the array");
            array[..arrayIndex].Should().BeEquivalentTo(Enumerable.Repeat(0, arrayIndex), "array elements outside of the copy bounds should not have been changed");
            array[(arrayIndex + items.Length)..].Should().BeEquivalentTo(Enumerable.Repeat(0, array.Length - items.Length - arrayIndex), "array elements outside of the copy bounds should not have been changed");
        }
    }
}
