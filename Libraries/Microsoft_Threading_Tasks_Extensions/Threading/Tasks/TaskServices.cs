// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskServices
// Assembly: Microsoft.Threading.Tasks.Extensions, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 32DE64FC-4C1B-4762-B488-E8BFE502A0BB
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks_Extensions.dll

using System.ComponentModel;

namespace System.Threading.Tasks
{
  internal class TaskServices
  {
    public static Task FromCancellation(CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested ? new Task((Action) (() => { }), cancellationToken) : throw new ArgumentOutOfRangeException(nameof (cancellationToken));

    public static Task<TResult> FromCancellation<TResult>(CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested ? new Task<TResult>((Func<TResult>) (() => default (TResult)), cancellationToken) : throw new ArgumentOutOfRangeException(nameof (cancellationToken));

    public static void HandleEapCompletion<T>(
      TaskCompletionSource<T> tcs,
      bool requireMatch,
      AsyncCompletedEventArgs e,
      Func<T> getResult,
      Action unregisterHandler)
    {
      if (requireMatch)
      {
        if (e.UserState != tcs)
          return;
      }
      try
      {
        unregisterHandler();
      }
      finally
      {
        if (e.Cancelled)
          tcs.TrySetCanceled();
        else if (e.Error != null)
          tcs.TrySetException(e.Error);
        else
          tcs.TrySetResult(getResult());
      }
    }
  }
}
