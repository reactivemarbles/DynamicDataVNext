using System;

namespace DynamicDataVNext.Tests;

internal static class RawAnonymousObserver
{
    public static RawAnonymousObserver<T> Create<T>(
            Action<T>           onNext,
            Action<Exception>   onError,
            Action              onCompleted)
        => new(
            onNext:         onNext,
            onError:        onError,
            onCompleted:    onCompleted);
}

// Thin implementation of IObserver<T> with no built-in safeguards, to facilitate correctness testing.
internal class RawAnonymousObserver<T>
    : IObserver<T>
{
    private readonly Action             _onCompleted;
    private readonly Action<Exception>  _onError;
    private readonly Action<T>          _onNext;

    public RawAnonymousObserver(
        Action<T>           onNext,
        Action<Exception>   onError,
        Action              onCompleted)
    {
        _onNext         = onNext;
        _onError        = onError;
        _onCompleted    = onCompleted;
    }

    public void OnCompleted()
        => _onCompleted.Invoke();
    
    public void OnError(Exception error)
        => _onError.Invoke(error);
    
    public void OnNext(T value)
        => _onNext.Invoke(value);
}
