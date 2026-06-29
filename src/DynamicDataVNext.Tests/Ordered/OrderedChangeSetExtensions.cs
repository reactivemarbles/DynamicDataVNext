using AwesomeAssertions.Execution;

namespace DynamicDataVNext.Tests.Ordered;

public static class OrderedChangeSetExtensions
{
    public static ChangeSetAssertions<OrderedChangeSet<T>, OrderedChange<T>, OrderedChangeType> Should<T>(this OrderedChangeSet<T> subject)
        => new(
            subject:        subject,
            assertionChain: AssertionChain.GetOrCreate());
}
