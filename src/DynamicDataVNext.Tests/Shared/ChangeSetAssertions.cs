using System;
using System.Linq;

using AwesomeAssertions;
using AwesomeAssertions.Execution;

namespace DynamicDataVNext.Tests;

public class ChangeSetAssertions<TChangeSet, TChange, TChangeType>
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    public ChangeSetAssertions(
        TChangeSet      subject,
        AssertionChain  assertionChain)
    {
        _assertionChain = assertionChain;
        _subject        = subject;
    }

    public TChangeSet Subject
        => _subject;
    
    [CustomAssertion]
    public AndConstraint<ChangeSetAssertions<TChangeSet, TChange, TChangeType>> BeValid()
    {
        var continuation = _assertionChain
            .ForCondition(Enum.IsDefined(_subject.Type))
            .FailWith("Expected {context:a changeset} type {0} to be defined in {1}, but it is not.",
                _subject.Type,
                typeof(ChangeSetType));
            
        switch (_subject.Type)
        {
            case ChangeSetType.Clear:
                continuation
                    .Then
                    .ForCondition(_subject.Changes.Length is not 0)
                    .FailWith("Expected {context:a changeset} of type {0} to contain at least one change, but found none.",
                        _subject.Type)
                    .Then
                    .Given(() => _subject.Changes.Any(static change => change.Category is not ChangeCategory.Removal))
                    .ForCondition(static hasInvalidChanges => !hasInvalidChanges)
                    .FailWith("Expected {context:a changeset} of type {0} to contain only {1} changes, but found a {2} change.",
                        _ => _subject.Type,
                        _ => ChangeCategory.Removal,
                        // Using .FirstOrDefault() as a workaround for https://github.com/AwesomeAssertions/AwesomeAssertions/issues/521
                        _ => _subject.Changes.FirstOrDefault(static change => change.Category is not ChangeCategory.Removal).Type);
                break;

            case ChangeSetType.Empty:
                continuation
                    .Then
                    .ForCondition(_subject.Changes.IsDefaultOrEmpty)
                    .FailWith("Expected {context:a changeset} of type {0} to have an empty list of changes, but found {1} changes instead",
                        () => _subject.Type,
                        () => _subject.Changes.Length);
                break;
            
            case ChangeSetType.Reset:
                continuation
                    .Then
                    .ForCondition(!_subject.Changes.IsDefaultOrEmpty)
                    .FailWith("Expected {context:a changeset} of type {0} to contain at least one change, but found none.",
                        _subject.Type)
                    .Then
                    .Given(() => _subject.Changes.Any(static change => change.Category is not ChangeCategory.Addition and not ChangeCategory.Removal))
                    .ForCondition(static hasInvalidChanges => !hasInvalidChanges)
                    .FailWith("Expected {context:changeset} of type {0} to contain only {1} or {2} changes, but found a {3} change.",
                        _ => _subject.Type,
                        _ => ChangeCategory.Addition,
                        _ => ChangeCategory.Removal,
                        // Using .FirstOrDefault() as a workaround for https://github.com/AwesomeAssertions/AwesomeAssertions/issues/521
                        _ => _subject.Changes.FirstOrDefault(static change => change.Category is not ChangeCategory.Addition and not ChangeCategory.Removal).Type)
                    .Then
                    .Given(_ => _subject.Changes.Any(static change => change.Category is ChangeCategory.Addition))
                    .ForCondition(static hasAdditions => hasAdditions)
                    .FailWith("Expected {context:a changeset} of type {0} to contain at least 1 {1} change, but found none.",
                        _subject.Type,
                        ChangeCategory.Addition)
                    .Then
                    .Given(_ => _subject.Changes
                        .SkipWhile(static change => change.Category is ChangeCategory.Removal)
                        .SkipWhile(static change => change.Category is ChangeCategory.Addition)
                        .Any())
                    .ForCondition(static hasOutOfOrderChanges => !hasOutOfOrderChanges)
                    .FailWith("Expected {context:a changeset} of type {0} to have all {1} changes preceding all {2} changes, but found and out-of-order {1} at index {3}.",
                        _ => _subject.Type,
                        _ => ChangeCategory.Removal,
                        _ => ChangeCategory.Addition,
                        _ => _subject.Changes
                                .SkipWhile(static change => change.Category is ChangeCategory.Removal)
                                .Index()
                                // Using .FirstOrDefault() as a workaround for https://github.com/AwesomeAssertions/AwesomeAssertions/issues/521
                                .FirstOrDefault(static pair => pair.Item.Category is ChangeCategory.Removal)
                                .Index
                            + _subject.Changes
                                .TakeWhile(static change => change.Category is ChangeCategory.Removal)
                                .Count());
                break;
            
            case ChangeSetType.Update:
                continuation
                    .Then
                    .ForCondition(!_subject.Changes.IsDefaultOrEmpty)
                    .FailWith("Expected {context:a changeset} of type {0} to contain at least one change, but found none.",
                        _subject.Type)
                    .Then
                    .Given(() => _subject.Changes.Any(static change => change.Category is ChangeCategory.None))
                    .ForCondition(static hasInvalidChanges => !hasInvalidChanges)
                    .FailWith("Expected {context:a changeset} of type {0} to contain no changes of type {1}, but found {2}.",
                        _ => _subject.Type,
                        _ => ChangeCategory.None,
                        _ => _subject.Changes.Count(static change => change.Category is ChangeCategory.None));
                break;
        }
        
        return new(this);
    }
    
    private readonly AssertionChain _assertionChain;
    private readonly TChangeSet     _subject;
}
