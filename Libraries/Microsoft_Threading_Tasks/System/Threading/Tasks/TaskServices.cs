// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskServices
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

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
