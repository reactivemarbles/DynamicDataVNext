using System;

namespace DynamicDataVNext.Tests;

public static class RawAnonymousObservable
{
    public static RawAnonymousObservable<T> Create<T>(Func<IObserver<T>, IDisposable> onSubscribe)
        => new(onSubscribe);
}

// Thin implementation of IObservable<T> with no built-in safeguards, to facilitate correctness testing.
public class RawAnonymousObservable<T>
    : IObservable<T>
{
    private readonly Func<IObserver<T>, IDisposable> _onSubscribe;

    public RawAnonymousObservable(Func<IObserver<T>, IDisposable> onSubscribe)
        => _onSubscribe = onSubscribe;

    public IDisposable Subscribe(IObserver<T> observer)
        => _onSubscribe.Invoke(observer);
}
