namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract class Base<TUutAdapter, TChangeSet, TChange, TChangeType>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    protected static ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> PerformSetup(SetupTestCase testCase)
    {
        var uut = TUutAdapter.CreateUut(testCase.SourceCount);
            
        for (var i = 0; i < testCase.ChangesCategories.Count; ++i)
            uut.AddChange(testCase.ChangesCategories[i] switch
            {
                ChangeCategory.Addition => TUutAdapter.CreateAddition(i),
                ChangeCategory.Removal  => TUutAdapter.CreateRemoval(
                    sourceCount:    uut.SourceCount,
                    item:           i),
                var category            => throw new NotSupportedException($"Test does not support changes of category {category}")
            });
            
        return uut;
    }
}
