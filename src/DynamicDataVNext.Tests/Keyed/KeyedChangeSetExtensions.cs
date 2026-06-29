using AwesomeAssertions.Execution;

namespace DynamicDataVNext.Tests.Keyed;

public static class KeyedChangeSetExtensions
{
    public static ChangeSetAssertions<KeyedChangeSet<TKey, TItem>, KeyedChange<TKey, TItem>, KeyedChangeType> Should<TKey, TItem>(this KeyedChangeSet<TKey, TItem> subject)
        => new(
            subject:        subject,
            assertionChain: AssertionChain.GetOrCreate());
}
