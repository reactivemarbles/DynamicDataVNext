using System;
using System.Reactive.Concurrency;

namespace DynamicDataVNext.Tests;

public static class ObservableExtensions
{
    public static IDisposable RecordValues<T>(
        this    IObservable<T>              source,
        out     ValueRecordingObserver<T>   observer,
                IScheduler?                 scheduler = null)
    {
        observer = new ValueRecordingObserver<T>(scheduler ?? DefaultScheduler.Instance);

        return source.Subscribe(observer);
    }
}
