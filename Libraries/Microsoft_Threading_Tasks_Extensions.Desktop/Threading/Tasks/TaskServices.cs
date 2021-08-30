// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskServices
// Assembly: Microsoft.Threading.Tasks.Extensions.Desktop, Version=1.0.168.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5E74C701-7BC8-493E-B2E6-765635FA6670
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks_Extensions.Desktop.dll

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
