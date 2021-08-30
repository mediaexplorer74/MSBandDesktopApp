// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.TaskTimer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class TaskTimer : IDisposable
  {
    public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);
    private TimeSpan delay = TimeSpan.FromMinutes(1.0);
    private CancellationTokenSource tokenSource;

    public TimeSpan Delay
    {
      get => this.delay;
      set
      {
        if ((value <= TimeSpan.Zero || value.TotalMilliseconds > (double) int.MaxValue) && value != TaskTimer.InfiniteTimeSpan)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.delay = value;
      }
    }

    public bool IsStarted => this.tokenSource != null;

    public void Start(Func<Task> elapsed)
    {
      CancellationTokenSource newTokenSource = new CancellationTokenSource();
      TaskEx.Delay((TimeSpan) this.Delay, newTokenSource.Token).ContinueWith<Task>((Func<Task, Task>) (async previousTask =>
      {
        TaskTimer.CancelAndDispose(Interlocked.CompareExchange<CancellationTokenSource>(ref this.tokenSource, (CancellationTokenSource) null, newTokenSource));
        try
        {
          await elapsed();
        }
        catch (Exception ex)
        {
          if (ex is AggregateException)
          {
            foreach (Exception innerException in ((AggregateException) ex).InnerExceptions)
              CoreEventSource.Log.LogError(innerException.ToString());
          }
          CoreEventSource.Log.LogError(ex.ToString());
        }
      }), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
      TaskTimer.CancelAndDispose(Interlocked.Exchange<CancellationTokenSource>(ref this.tokenSource, newTokenSource));
    }

    public void Cancel() => TaskTimer.CancelAndDispose(Interlocked.Exchange<CancellationTokenSource>(ref this.tokenSource, (CancellationTokenSource) null));

    public void Dispose() => this.Cancel();

    private static void CancelAndDispose(CancellationTokenSource tokenSource)
    {
      if (tokenSource == null)
        return;
      tokenSource.Cancel();
      tokenSource.Dispose();
    }
  }
}
