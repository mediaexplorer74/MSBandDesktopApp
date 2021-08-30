// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.MutexExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class MutexExtensions
  {
    public static async Task<T> RunSynchronizedAsync<T>(
      this Mutex mutex,
      Func<Task<T>> func,
      CancellationToken token)
    {
      ManualResetEvent releaseMutexEvent = new ManualResetEvent(false);
      T obj;
      try
      {
        await MutexExtensions.StartMutexTaskAsync(mutex, token, releaseMutexEvent).ConfigureAwait(false);
        obj = await func().ConfigureAwait(false);
      }
      finally
      {
        releaseMutexEvent.Set();
      }
      return obj;
    }

    public static Task RunSynchronizedAsync(
      this Mutex mutex,
      Func<Task> func,
      CancellationToken token)
    {
      return (Task) mutex.RunSynchronizedAsync<bool>((Func<Task<bool>>) (async () =>
      {
        await func().ConfigureAwait(false);
        return true;
      }), token);
    }

    private static Task StartMutexTaskAsync(
      Mutex mutex,
      CancellationToken token,
      ManualResetEvent releaseMutexEvent)
    {
      TaskCompletionSource<object> mutexAcquireCompletionSource = new TaskCompletionSource<object>();
      Task.Run((Action) (() =>
      {
        bool flag = false;
        try
        {
          mutex.Wait(token);
          flag = true;
          mutexAcquireCompletionSource.SetResult((object) null);
        }
        catch (Exception ex)
        {
          mutexAcquireCompletionSource.SetException(ex);
        }
        releaseMutexEvent.WaitOne();
        if (!flag)
          return;
        mutex.ReleaseMutex();
      }));
      return (Task) mutexAcquireCompletionSource.Task;
    }
  }
}
