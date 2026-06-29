using AwesomeAssertions.Execution;

namespace DynamicDataVNext.Tests.Distinct;

public static class DistinctChangeSetExtensions
{
    public static ChangeSetAssertions<DistinctChangeSet<T>, DistinctChange<T>, DistinctChangeType> Should<T>(this DistinctChangeSet<T> subject)
        => new(
            subject:        subject,
            assertionChain: AssertionChain.GetOrCreate());
}
