// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ApplicationLifecycleService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class ApplicationLifecycleService : IApplicationLifecycleService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ApplicationLifecycleService.cs");
    private static readonly TimeSpan SuspendDeadlineBuffer = TimeSpan.FromMilliseconds(200.0);
    private static readonly TimeSpan LogFlushDedicatedTime = TimeSpan.FromMilliseconds(200.0);
    private readonly List<Func<CancellationToken, Task>> onSuspendingFuncs = new List<Func<CancellationToken, Task>>();
    private readonly object suspendLock = new object();
    private Func<CancellationToken, Task> foregroundLoggerFunc;

    public virtual void Initialize()
    {
    }

    public void RegisterSuspending(Func<CancellationToken, Task> onSuspendingFunc)
    {
      lock (this.suspendLock)
        this.onSuspendingFuncs.Add(onSuspendingFunc);
    }

    public void RegisterSuspendingLogFlush(Func<CancellationToken, Task> onSuspendingFunc) => this.foregroundLoggerFunc = onSuspendingFunc;

    public void UnregisterSuspending(Func<CancellationToken, Task> onSuspendingFunc)
    {
      lock (this.suspendLock)
        this.onSuspendingFuncs.Remove(onSuspendingFunc);
    }

    public event EventHandler<object> Resuming;

    public void OnResuming(object sender)
    {
      EventHandler<object> resuming = this.Resuming;
      if (resuming == null)
        return;
      resuming(sender, (object) null);
    }

    public async Task OnSuspendingAsync(DateTimeOffset deadline)
    {
      if (deadline - DateTimeOffset.Now <= ApplicationLifecycleService.SuspendDeadlineBuffer)
      {
        ApplicationLifecycleService.Logger.Warn((object) "Unable to notify components of suspension or flush logs as there is insufficient time.");
      }
      else
      {
        try
        {
          TimeSpan delay = deadline - DateTimeOffset.Now - ApplicationLifecycleService.SuspendDeadlineBuffer - ApplicationLifecycleService.LogFlushDedicatedTime;
          if (delay > TimeSpan.Zero)
          {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource(delay))
            {
              List<Task> list;
              lock (this.suspendLock)
                list = this.onSuspendingFuncs.Select<Func<CancellationToken, Task>, Task>((Func<Func<CancellationToken, Task>, Task>) (f => f(tokenSource.Token))).ToList<Task>();
              await Task.WhenAll((IEnumerable<Task>) list);
            }
          }
          else
            ApplicationLifecycleService.Logger.Warn((object) "Unable to notify components of suspension as there is insufficient time. Flushing logs.");
        }
        catch (OperationCanceledException ex)
        {
          ApplicationLifecycleService.Logger.Warn((object) "Not all components were able to handle suspension within the allotted time.", (Exception) ex);
        }
        finally
        {
          if (this.foregroundLoggerFunc != null)
          {
            TimeSpan delay = deadline - DateTimeOffset.Now - ApplicationLifecycleService.SuspendDeadlineBuffer;
            if (delay > TimeSpan.Zero)
            {
              using (CancellationTokenSource tokenSource = new CancellationTokenSource(delay))
              {
                try
                {
                  await this.foregroundLoggerFunc(tokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                }
              }
            }
          }
        }
      }
    }
  }
}
