using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public partial class SelectTests
{
    [Test]
    public void WhenComparerIsGiven_ComparerPropagates()
    {
        var stream = new DistinctChangeStream<string>()
        {
            Comparer    = EqualityComparer<string>.Default,
            Source      = Observable.Never<DistinctChangeSet<string>>()
        };
        
        var result = stream.Select(
            selector:   static item => item,
            comparer:   StringComparer.OrdinalIgnoreCase);
        
        result.Comparer.Should().BeSameAs(StringComparer.OrdinalIgnoreCase, "a given comparer should propagate downstream");
    }
    
    [Test]
    public void WhenComparerIsNotGiven_DefaultComparerPropagates()
    {
        var stream = new DistinctChangeStream<string>()
        {
            Comparer    = StringComparer.OrdinalIgnoreCase,
            Source      = Observable.Never<DistinctChangeSet<string>>()
        };
        
        var result = stream.Select(static item => item);
        
        result.Comparer.Should().BeSameAs(EqualityComparer<string>.Default, "when no comparer is given, the default comparer should be used");
    }
    
    private static TestCaseData WhenOptionsIsGiven_CreateTestCase(
            DistinctItemSelectionOptions    options,
            DistinctItemOptions             resultOptions)
        => new(options, resultOptions);
    
    public static readonly IReadOnlyList<TestCaseData> WhenOptionsIsGiven_TestCases
        = new[]
        {
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    resultOptions:  new() { ItemsAreMutable = false })
                .SetName("{m}(Deterministic Selection)"),
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    resultOptions:  new() { ItemsAreMutable = false })
                .SetName("{m}(Non-Deterministic Selection)"),
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Mutable },
                    resultOptions:  new() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Selection)")
        };
    [TestCaseSource(nameof(WhenOptionsIsGiven_TestCases))]
    public void WhenOptionsIsGiven_OptionsPropagates(
        DistinctItemSelectionOptions    options,
        DistinctItemOptions             resultOptions)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Never<DistinctChangeSet<int>>()
        };
        
        var result = stream.Select(
            selector:   static item => item,
            options:    options);
        
        result.Options.Should().Be(resultOptions, "options given for selection should propagate downstream");
    }

    private static TestCaseData WhenOptionsIsNotGiven_CreateTestCase(
            Type                tOut,
            DistinctItemOptions streamOptions,
            DistinctItemOptions resultOptions,
            string              because)
        => new(tOut, streamOptions, resultOptions, because);
    
    public static readonly IReadOnlyList<TestCaseData> WhenOptionsIsNotGiven_TestCases
        = new[]
        {
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(object),
                    streamOptions:  new() { ItemsAreMutable = true },
                    resultOptions:  new() { ItemsAreMutable = true },
                    because:        "we assume that mutability propagates, when it can")
                .SetName("{m}(Mutable Inputs, Potentially Mutable Outputs)"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(int),
                    streamOptions:  new() { ItemsAreMutable = true },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "value types are always immutable")
                .SetName("{m}(Mutable Inputs, Immutable Outputs"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(int),
                    streamOptions:  new() { ItemsAreMutable = false },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "value types are always immutable")
                .SetName("{m}(Immutable Inputs, Immutable Outputs"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(object),
                    streamOptions:  new() { ItemsAreMutable = false },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "we assume that immutability propagates")
                .SetName("{m}(Mutable Inputs, Potentially Mutable Outputs")
        };
    [TestCaseSource(nameof(WhenOptionsIsNotGiven_TestCases))]
    public void WhenOptionsIsNotGiven_ResultOptionsAreInferred(
            Type                tOut,
            DistinctItemOptions streamOptions,
            DistinctItemOptions resultOptions,
            string              because)
        => GetType()
            .GetMethod(nameof(WhenOptionsIsNotGiven_ResultOptionsAreInferred), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(tOut)
            .Invoke(this, new object[] { streamOptions, resultOptions, because });

    private static void WhenOptionsIsNotGiven_ResultOptionsAreInferred<TOut>(
        DistinctItemOptions streamOptions,
        DistinctItemOptions resultOptions,
        string              because)
    {
        var stream = new DistinctChangeStream<TOut>()
        {
            Comparer    = EqualityComparer<TOut>.Default,
            Options     = streamOptions,
            Source      = Observable.Never<DistinctChangeSet<TOut>>()
        };
        
        var result = stream.Select(static item => item);
        
        result.Options.Should().Be(resultOptions, because);
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSelectionIsNotDeterministic_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Mutable })
                .SetName("{m}(Mutable Selection)")
        };
    [TestCaseSource(nameof(WhenSelectionIsNotDeterministic_TestCases))]
    public void WhenSelectionIsNotDeterministic_SelectorIsOnlyInvokedOncePerItem(DistinctItemSelectionOptions options)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(items)),
                source)
        };
        
        var selectorInvocations = new List<int>();

        using var subscription = stream.Select(
                selector:   item =>
                {
                    selectorInvocations.Add(item);
                    return item;
                },
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("there were initial items in the collection");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes should propagate downstream, after having the selector applied");
        results.ClearNotifications();

        selectorInvocations.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "the selector should have been invoked for each added item");
        selectorInvocations.Clear();
        
        source.OnNext(DistinctChangeSet.CreateForClear(items));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEmpty("all changes should propagate downstream, after having the selector applied");
        
        selectorInvocations.Should().BeEmpty("non-deterministic selectors should only be invoked once, per-item");
    }

    public class WhenSelectorThrows_Item
    {
        public required int Id { get; init; }
        
        public TestException? Error { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSelectorThrows_TestCases
        = new[]
        {
            new TestCaseData(
                    new[] {  new WhenSelectorThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m})(Single item, Deterministic selection)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenSelectorThrows_Item() { Id = 1 },
                        new WhenSelectorThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenSelectorThrows_Item() { Id = 3 },
                    },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m})(Multiple items, Deterministic selection)"),
            new TestCaseData(
                    new[] {  new WhenSelectorThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m})(Single item, Non-deterministic selection)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenSelectorThrows_Item() { Id = 1 },
                        new WhenSelectorThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenSelectorThrows_Item() { Id = 3 },
                    },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m})(Multiple items, Non-deterministic selection)"),
        };
    [TestCaseSource(nameof(WhenSelectorThrows_TestCases))]
    public void WhenSelectorThrows_ErrorPropagates(
        IReadOnlyList<WhenSelectorThrows_Item>  items,
        DistinctItemSelectionOptions            options)
    {
        using var source = new Subject<DistinctChangeSet<WhenSelectorThrows_Item>>(); 
        
        var stream = new DistinctChangeStream<WhenSelectorThrows_Item>()
        {
            Comparer    = EqualityComparer<WhenSelectorThrows_Item>.Default,
            Source      = source
        };
        
        using var subscription = stream.Select(
                selector:   static item => (item.Error is null)
                    ? item
                    : throw item.Error,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");

        source.OnNext(DistinctChangeSet.CreateForReset(items));

        var expectedError = items
            .Select(static item => item.Error)
            .First(static error => error is not null);
        results.Error.Should().Be(expectedError, "consumer errors should propagate downstream");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during processing of changes");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourceCompletesAsynchronously_TestCases))]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates(DistinctItemSelectionOptions options)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        
        streamSource.OnCompleted();
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("no change operations were performed");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourceCompletesImmediately_TestCases))]
    public void WhenSourceCompletesImmediately_CompletionPropagates(DistinctItemSelectionOptions options)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourceFailsAsynchronously_TestCases))]
    public void WhenSourceFailsAsynchronously_ErrorPropagates(DistinctItemSelectionOptions options)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        
        var error = new TestException();
        
        streamSource.OnError(error);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("no change operation were performed");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourceFailsImmediately_TestCases))]
    public void WhenSourceFailsImmediately_ErrorPropagates(DistinctItemSelectionOptions options)
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during initial subscription");
    }

    private static TestCaseData WhenSourcePublishesChanges_CreateTestCase(
            DistinctItemSelectionOptions    options,
            IReadOnlyList<int>              initialItems,
            DistinctChangeSet<int>          changeSet,
            IReadOnlyList<int>              finalItems)
        => new(options, initialItems, changeSet, finalItems);
    
    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChanges_TestCases
        = new[]
        {
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }),
                    finalItems:     new[] { 1 })
                .SetName("{m}(Initial Reset, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 1, 2, 3 })
                .SetName("{m}(Initial Reset, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Subsequent Reset, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3 },
                        addedItems:     new[] { 4, 5, 6 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Subsequent Reset, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForAddition(2),
                    finalItems:     new[] { 1, 2 })
                .SetName("{m}(Add, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 1, 2, 3, 4, 5, 6 })
                .SetName("{m}(Add, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Remove, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Remove, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }),
                    finalItems:     new[] { 1 })
                .SetName("{m}(Initial Reset, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 1, 2, 3 })
                .SetName("{m}(Initial Reset, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Subsequent Reset, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Subsequent Reset, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForAddition(2),
                    finalItems:     new[] { 1, 2 })
                .SetName("{m}(Add, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 1, 2, 3, 4, 5, 6 })
                .SetName("{m}(Add, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Remove, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Remove, Multiple items, Non-deterministic selection)")
        };
    [TestCaseSource(nameof(WhenSourcePublishesChanges_TestCases))]
    public void WhenSourcePublishesChanges_NotificationPropagates(
        DistinctItemSelectionOptions    options,
        IReadOnlyList<int>              initialItems,
        DistinctChangeSet<int>          changeSet,
        IReadOnlyList<int>              finalItems)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                (initialItems.Count is 0)
                    ? Observable.Empty<DistinctChangeSet<int>>()
                    : Observable.Return(DistinctChangeSet.CreateForReset(initialItems)),
                source)
        };
        
        using var subscription = stream.Select(
                selector:   static item => item.ToString(),
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        if (initialItems.Count is 0)
            results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        else
        {
            results.RecordedChangeSets.Should().ContainSingle("there were initial items in the collection");
            results.RecordedItems.Should().BeEquivalentTo(
                expectation:    initialItems.Select(static item => item.ToString()),
                config:         options => options.WithoutStrictOrdering(),
                because:        "all changes should propagate downstream, after having the selector applied");
            results.ClearNotifications();
        }

        source.OnNext(changeSet);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation occurred");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    finalItems.Select(static item => item.ToString()),
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes should propagate downstream, after having the selector applied");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesEmpty_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourcePublishesEmpty_TestCases))]
    public void WhenSourcePublishesEmpty_NotificationDoesNotPropagate(DistinctItemSelectionOptions options)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = source
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");

        source.OnNext(DistinctChangeSet.Empty<int>());
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("empty changesets should not propagate");
    }    
}
