namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

[TestFixture]
public static partial class ApplyToTests
{
    public class TargetShouldChangeTestCase
    {
        public required KeyedChangeSet<string, int> ChangeSet { get; init; }
        
        public required IReadOnlyList<KeyValuePair<string, int>> ExpectedItems { get; init; }
        
        public required IReadOnlyList<KeyValuePair<string, int>> TargetItems { get; init; }
    }

    public class TargetShouldNotChangeTestCase
    {
        public required KeyedChangeSet<string, int> ChangeSet { get; init; }
        
        public required IReadOnlyList<KeyValuePair<string, int>> TargetItems { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> TargetItems_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<KeyValuePair<string, int>>())
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetContainsOnlyRefreshmentsAndIsConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet   = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1)
                    }),
                    TargetItems = new [] { new KeyValuePair<string, int>("1", 1) }
                })
                .SetName("{m}(Single item refreshed, Multiple items in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet   = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("2", 2)
                    }),
                    TargetItems = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    } 
                })
                .SetName("{m}(Single item refreshed, Multiple items in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    } 
                })
                .SetName("{m}(Multiple items refreshed, Same items in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3),
                        KeyedChange.CreateRefreshment("4", 4)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3),
                        new ("4", 4),
                        new ("5", 5)
                    } 
                })
                .SetName("{m}(Multiple items refreshed, Same items in target)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetContainsOnlyRefreshmentsAndIsNotConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet   = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("2", 2)
                    }),
                    TargetItems = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single item refreshed, Empty target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet   = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("2", 2)
                    }),
                    TargetItems = new[] { new KeyValuePair<string, int>("1", 1) }
                })
                .SetName("{m}(Single item refreshed, Different item in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet   = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("4", 4)
                    }),
                    TargetItems = new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }
                })
                .SetName("{m}(Single item refreshed, Multiple items in target, Refreshed item is missing)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multiple items refreshed, Empty target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems = new[] { new KeyValuePair<string, int>("1", 1) }
                })
                .SetName("{m}(Multiple items refreshed, Single matching item in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems = new[] { new KeyValuePair<string, int>("4", 4) }
                })
                .SetName("{m}(Multiple items refreshed, Single different item in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems = new KeyValuePair<string, int>[]
                    {
                        new ("2", 2),
                        new ("3", 3),
                        new ("4", 4)
                    }
                })
                .SetName("{m}(Multiple items refreshed, Multiple items in target, Single refreshed item is missing)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRefreshment("1", 1),
                        KeyedChange.CreateRefreshment("2", 2),
                        KeyedChange.CreateRefreshment("3", 3)
                    }),
                    TargetItems = new KeyValuePair<string, int>[]
                    {
                        new ("4", 4),
                        new ("5", 5),
                        new ("6", 6)
                    }
                })
                .SetName("{m}(Multiple items refreshed, Multiple items in target, All refreshed items are missing)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsClearAndConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-item clear)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear)")
        }; 

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsClearAndNotConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-item clear, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("2", 2) },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-item clear, Different item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-item clear, Multiple items in target, No target items are missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-item clear, Multiple items in target, Single target item is missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Single matching item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("4", 4) },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Single extraneous item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Multiple items in target, Single target item is missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Multiple items in target, Single target item is extraneous)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new ("1", 1),
                        new ("2", 2),
                        new ("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("5", 5)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-item clear, Multiple items in target, Multiple target items are extraneous)")
        }; 

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsResetAndConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   Array.Empty<KeyValuePair<string, int>>(),
                        additions:  new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("1", 1) } 
                })
                .SetName("{m}(Single-item reset, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new[] { new KeyValuePair<string, int>("2", 2) }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("2", 2) } 
                })
                .SetName("{m}(Single-item reset, Single item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3),
                        },
                        additions:  new[] { new KeyValuePair<string, int>("4", 4) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("4", 4) } 
                })
                .SetName("{m}(Single-item reset, Multiple items in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   Array.Empty<KeyValuePair<string, int>>(),
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    } 
                })
                .SetName("{m}(Multi-item reset, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("2", 2),
                            new("3", 3),
                            new("4", 4)
                        }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    } 
                })
                .SetName("{m}(Multi-item reset, Single item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("4", 4),
                            new("5", 5),
                            new("6", 6)
                        }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    }
                })
                .SetName("{m}(Multi-item reset, Multiple items in target, No target items are missing or extraneous)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsResetAndNotConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new[] { new KeyValuePair<string, int>("2", 2) }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("2", 2) } 
                })
                .SetName("{m}(Single-item reset, Empty target, Target is missing single item)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3),
                        },
                        additions:  new[] { new KeyValuePair<string, int>("4", 4) }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("4", 4) } 
                })
                .SetName("{m}(Single-item reset, Empty target, Target is missing multiple items)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new[] { new KeyValuePair<string, int>("2", 2) }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("3", 3) },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("2", 2) } 
                })
                .SetName("{m}(Single-item reset, Different item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   Array.Empty<KeyValuePair<string, int>>(),
                        additions:  new[] { new KeyValuePair<string, int>("1", 1) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("1", 1) } 
                })
                .SetName("{m}(Single-item reset, Multiple items in target, All target items are extraneous)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new[] { new KeyValuePair<string, int>("2", 2) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("2", 2) } 
                })
                .SetName("{m}(Single-item reset, Multiple items in target, Some target items are extraneous)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3),
                            new("4", 4),
                        },
                        additions:  new[] { new KeyValuePair<string, int>("5", 5) }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("5", 5) } 
                })
                .SetName("{m}(Single-item reset, Multiple items in target, Some target items are missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new[] { new KeyValuePair<string, int>("1", 1) },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("2", 2),
                            new("3", 3),
                            new("4", 4)
                        }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    } 
                })
                .SetName("{m}(Multi-item reset, Empty target, Single target item is missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("4", 4),
                            new("5", 5),
                            new("6", 6)
                        }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    } 
                })
                .SetName("{m}(Multi-item reset, Empty target, Multiple target items are missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("3", 3),
                            new("4", 4),
                            new("5", 5)
                        }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("3", 3),
                        new("4", 4),
                        new("5", 5)
                    } 
                })
                .SetName("{m}(Multi-item reset, Single item in target, Single target item is missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("4", 4),
                            new("5", 5),
                            new("6", 6)
                        }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    } 
                })
                .SetName("{m}(Multi-item reset, Single item in target, Multiple target items are missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   Array.Empty<KeyValuePair<string, int>>(),
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("4", 4) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    } 
                })
                .SetName("{m}(Multi-item reset, Single item in target, Single target item is extraneous)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3),
                            new("4", 4)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("5", 5),
                            new("6", 6),
                            new("7", 7)
                        }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("5", 5),
                        new("6", 6),
                        new("7", 7)
                    }
                })
                .SetName("{m}(Multi-item reset, Multiple items in target, Single target item is missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3),
                            new("4", 4)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("5", 5),
                            new("6", 6),
                            new("7", 7)
                        }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("5", 5),
                        new("6", 6),
                        new("7", 7)
                    }
                })
                .SetName("{m}(Multi-item reset, Multiple items in target, Multiple target items are missing)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("3", 3),
                            new("4", 4),
                            new("5", 5)
                        }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("3", 3),
                        new("4", 4),
                        new("5", 5)
                    }
                })
                .SetName("{m}(Multi-item reset, Multiple items in target, Single target item is extraneous)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   Array.Empty<KeyValuePair<string, int>>(),
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Multi-item reset, Multiple items in target, All target items are extraneous)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsUpdateAndConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateAddition("1", 1)),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new [] { new KeyValuePair<string, int>("1", 1) }
                })
                .SetName("{m}(Single-change update, Item added, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateAddition("2", 2)),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2)
                    }
                })
                .SetName("{m}(Single-change update, Item added, Single item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateAddition("4", 4)),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Single-change update, Item added, Multiple items in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition("1", 1),
                        KeyedChange.CreateAddition("2", 2),
                        KeyedChange.CreateAddition("3", 3)
                    }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Multi-change update, Items added, Empty target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition("2", 2),
                        KeyedChange.CreateAddition("3", 3),
                        KeyedChange.CreateAddition("4", 4)
                    }),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Multi-change update, Items added, Single item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition("4", 4),
                        KeyedChange.CreateAddition("5", 5),
                        KeyedChange.CreateAddition("6", 6)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    }
                })
                .SetName("{m}(Multi-change update, Items added, Multiple items in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateRemoval("1", 1)),
                    TargetItems     = new[] { new KeyValuePair<string, int>("1", 1) },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Single-change update, Item removed, Single item in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateRemoval("1", 1)),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Single-change update, Item removed, Multiple items in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1),
                        KeyedChange.CreateRemoval("2", 2),
                        KeyedChange.CreateRemoval("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>()
                })
                .SetName("{m}(Multi-change update, Items removed, Same items in target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1),
                        KeyedChange.CreateRemoval("2", 2),
                        KeyedChange.CreateRemoval("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    },
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    }
                })
                .SetName("{m}(Multi-change update, Items removed, Additional items in target)")
        };

    // Intentionally not testing for inconsistent updates, as the behavior is stated in the API as undefined. Depending
    // on the operation, and the type of the target collection, sometimes you might get a partial update, sometimes
    // changes might be ignored, sometimes you might get an exception. None of that is in the testing scope of
    // .ApplyTo()
    public static readonly IReadOnlyList<TestCaseData> WhenChangeSetIsUpdateAndNotConsistentWithTarget_TestCases
        = new[]
        {
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition("1", 1)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Single-change update, Item added, Already in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition("1", 1),
                        KeyedChange.CreateAddition("2", 2),
                        KeyedChange.CreateAddition("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("3", 3),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Multi-change update, Items added, Overlaps with target)"),
            new TestCaseData(new TargetShouldChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1)
                    }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>(),
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>() 
                })
                .SetName("{m}(Single-change update, Item removed, Empty target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Single-change update, Item removed, Not in target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1),
                        KeyedChange.CreateRemoval("2", 2),
                        KeyedChange.CreateRemoval("3", 3)
                    }),
                    TargetItems     = Array.Empty<KeyValuePair<string, int>>() 
                })
                .SetName("{m}(Multi-change update, Items removed, Empty target)"),
            new TestCaseData(new TargetShouldNotChangeTestCase()
                {
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval("1", 1),
                        KeyedChange.CreateRemoval("2", 2),
                        KeyedChange.CreateRemoval("3", 3)
                    }),
                    TargetItems     = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Multi-change update, Items removed, Overlaps with target)")
        };
}
