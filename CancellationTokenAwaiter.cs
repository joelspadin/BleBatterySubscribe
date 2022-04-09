namespace BleBatterySubscribe;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

public static class AsyncExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken token)
    {
        return new CancellationTokenAwaiter { CancellationToken = token };
    }

    /// <summary>
    /// Implements an awaiter for CancellationToken objects.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct CancellationTokenAwaiter : INotifyCompletion, ICriticalNotifyCompletion
    {
        internal CancellationToken CancellationToken;

        public CancellationTokenAwaiter(CancellationToken token) => CancellationToken = token;

        public object GetResult()
        {
            if (IsCompleted)
            {
                throw new OperationCanceledException();
            }

            throw new InvalidOperationException("token has not yet been canceled");
        }

        public bool IsCompleted => CancellationToken.IsCancellationRequested;

        public void OnCompleted(Action continuation) => CancellationToken.Register(continuation);

        public void UnsafeOnCompleted(Action continuation) => OnCompleted(continuation);
    }
}