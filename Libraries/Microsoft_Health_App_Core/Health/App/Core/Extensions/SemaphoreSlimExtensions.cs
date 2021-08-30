// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.SemaphoreSlimExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class SemaphoreSlimExtensions
  {
    public static void RunSynchronized(
      this SemaphoreSlim semaphore,
      Action action,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      bool flag = false;
      try
      {
        semaphore.Wait(cancellationToken);
        flag = true;
        action();
      }
      finally
      {
        if (flag)
          semaphore.Release();
      }
    }

    public static async Task<T> RunSynchronizedAsync<T>(
      this SemaphoreSlim semaphore,
      Func<Task<T>> func,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
      T obj;
      try
      {
        cancellationToken.ThrowIfCancellationRequested();
        obj = await func().ConfigureAwait(false);
      }
      finally
      {
        semaphore.Release();
      }
      return obj;
    }

    public static Task RunSynchronizedAsync(
      this SemaphoreSlim semaphore,
      Func<Task> func,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return (Task) semaphore.RunSynchronizedAsync<bool>((Func<Task<bool>>) (async () =>
      {
        await func().ConfigureAwait(false);
        return true;
      }), cancellationToken);
    }
  }
}
