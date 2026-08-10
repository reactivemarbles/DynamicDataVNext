namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromEnumeraFromReadOnlySpanAndKeySelectorbleAndKeySelector
        : Base
    {
        [Test]
        public void WhenKeySelectorIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = KeyedChangeSet.CreateForRemovals<int, int>(
                        items:          Array.Empty<int>().AsSpan(),
                        keySelector:    null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("keySelector")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForRemovals(
                items:          items.ToArray().AsSpan(),
                keySelector:    SelectKey);
    }
}
