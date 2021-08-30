// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnoisticsEventThrottlingScheduler
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnoisticsEventThrottlingScheduler : 
    IDiagnoisticsEventThrottlingScheduler,
    IDisposable
  {
    private readonly IList<TaskTimer> timers = (IList<TaskTimer>) new List<TaskTimer>();
    private volatile bool disposed;

    ~DiagnoisticsEventThrottlingScheduler() => this.Dispose(false);

    public ICollection<object> Tokens => (ICollection<object>) new ReadOnlyCollection<object>((IList<object>) Enumerable.Cast<object>(this.timers).ToList<object>());

    public object ScheduleToRunEveryTimeIntervalInMilliseconds(int interval, Action actionToExecute)
    {
      if (interval <= 0)
        throw new ArgumentOutOfRangeException(nameof (interval));
      TaskTimer taskTimer = actionToExecute != null ? DiagnoisticsEventThrottlingScheduler.InternalCreateAndStartTimer(interval, actionToExecute) : throw new ArgumentNullException(nameof (actionToExecute));
      this.timers.Add(taskTimer);
      CoreEventSource.Log.DiagnoisticsEventThrottlingSchedulerTimerWasCreated(interval);
      return (object) taskTimer;
    }

    public void RemoveScheduledRoutine(object token)
    {
      if (token == null)
        throw new ArgumentNullException(nameof (token));
      if (!(token is TaskTimer taskTimer))
        throw new ArgumentException(nameof (token));
      if (!this.timers.Remove(taskTimer))
        return;
      DiagnoisticsEventThrottlingScheduler.DisposeTimer((IDisposable) taskTimer);
      CoreEventSource.Log.DiagnoisticsEventThrottlingSchedulerTimerWasRemoved();
    }

    public void Dispose() => this.Dispose(true);

    private static void DisposeTimer(IDisposable timer)
    {
      try
      {
        timer.Dispose();
      }
      catch (Exception ex)
      {
        CoreEventSource.Log.DiagnoisticsEventThrottlingSchedulerDisposeTimerFailure(ex.ToInvariantString());
      }
    }

    private static TaskTimer InternalCreateAndStartTimer(
      int intervalInMilliseconds,
      Action action)
    {
      TaskTimer timer = new TaskTimer()
      {
        Delay = TimeSpan.FromMilliseconds((double) intervalInMilliseconds)
      };
      Func<Task> task = (Func<Task>) null;
      task = (Func<Task>) (() =>
      {
        timer.Start(task);
        action();
        return (Task) TaskEx.FromResult<object>((object) null);
      });
      timer.Start(task);
      return timer;
    }

    private void Dispose(bool managed)
    {
      if (managed && !this.disposed)
      {
        this.DisposeAllTimers();
        GC.SuppressFinalize((object) this);
      }
      this.disposed = true;
    }

    private void DisposeAllTimers()
    {
      foreach (IDisposable timer in (IEnumerable<TaskTimer>) this.timers)
        DiagnoisticsEventThrottlingScheduler.DisposeTimer(timer);
      this.timers.Clear();
    }
  }
}
