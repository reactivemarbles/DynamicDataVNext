namespace DynamicDataVNext.Tests;

public static class ObservableExtensions
{
    public static IDisposable RecordValues<T>(
        this    IObservable<T>              source,
        out     ValueRecordingObserver<T>   observer,
                ISequencer?                 scheduler = null)
    {
        observer = new ValueRecordingObserver<T>(scheduler ?? Sequencer.Default);

        return source.Subscribe(observer);
    }
}
