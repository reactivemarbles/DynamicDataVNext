namespace System.Collections.Immutable;

internal static class ImmutableArrayBuilderExtensions
{
    public static ImmutableArray<T> MoveToOrCreateImmutable<T>(this ImmutableArray<T>.Builder builder)
        => (builder.Count == builder.Capacity)
            ? builder.MoveToImmutable()
            : builder.ToImmutable();
}
