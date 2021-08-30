// Decompiled with JetBrains decompiler
// Type: AwaitExtensions
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using Microsoft.Runtime.CompilerServices;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

public static class AwaitExtensions
{
  public static void CancelAfter(this CancellationTokenSource source, int dueTime)
  {
    if (source == null)
      throw new NullReferenceException();
    if (dueTime < -1)
      throw new ArgumentOutOfRangeException(nameof (dueTime));
    Contract.EndContractBlock();
    Timer timer = (Timer) null;
    timer = new Timer((TimerCallback) (state =>
    {
      timer.Dispose();
      TimerManager.Remove(timer);
      try
      {
        source.Cancel();
      }
      catch (ObjectDisposedException ex)
      {
      }
    }), (object) null, -1, -1);
    TimerManager.Add(timer);
    timer.Change(dueTime, -1);
  }

  public static void CancelAfter(this CancellationTokenSource source, TimeSpan dueTime)
  {
    long totalMilliseconds = (long) dueTime.TotalMilliseconds;
    if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
      throw new ArgumentOutOfRangeException(nameof (dueTime));
    AwaitExtensions.CancelAfter(source, (int) totalMilliseconds);
  }

  public static TaskAwaiter GetAwaiter(this Task task) => task != null ? new TaskAwaiter(task) : throw new ArgumentNullException(nameof (task));

  public static TaskAwaiter<TResult> GetAwaiter<TResult>(this Task<TResult> task) => task != null ? new TaskAwaiter<TResult>(task) : throw new ArgumentNullException(nameof (task));

  public static ConfiguredTaskAwaitable ConfigureAwait(
    this Task task,
    bool continueOnCapturedContext)
  {
    return task != null ? new ConfiguredTaskAwaitable(task, continueOnCapturedContext) : throw new ArgumentNullException(nameof (task));
  }

  public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(
    this Task<TResult> task,
    bool continueOnCapturedContext)
  {
    return task != null ? new ConfiguredTaskAwaitable<TResult>(task, continueOnCapturedContext) : throw new ArgumentNullException(nameof (task));
  }
}
