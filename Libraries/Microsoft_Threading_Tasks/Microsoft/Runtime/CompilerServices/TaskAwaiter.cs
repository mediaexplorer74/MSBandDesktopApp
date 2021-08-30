// Decompiled with JetBrains decompiler
// Type: Microsoft.Runtime.CompilerServices.TaskAwaiter
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{
  public struct TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
  {
    internal const bool CONTINUE_ON_CAPTURED_CONTEXT_DEFAULT = true;
    private const string InvalidOperationException_TaskNotCompleted = "The task has not yet completed.";
    private readonly Task m_task;

    internal TaskAwaiter(Task task)
    {
      Contract.Assert(task != null);
      this.m_task = task;
    }

    public bool IsCompleted => this.m_task.IsCompleted;

    public void OnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal(this.m_task, continuation, true);

    public void UnsafeOnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal(this.m_task, continuation, true);

    public void GetResult() => TaskAwaiter.ValidateEnd(this.m_task);

    internal static void ValidateEnd(Task task)
    {
      if (task.Status == TaskStatus.RanToCompletion)
        return;
      TaskAwaiter.HandleNonSuccess(task);
    }

    private static void HandleNonSuccess(Task task)
    {
      if (!task.IsCompleted)
      {
        try
        {
          task.Wait();
        }
        catch
        {
        }
      }
      if (task.Status == TaskStatus.RanToCompletion)
        return;
      TaskAwaiter.ThrowForNonSuccess(task);
    }

    private static void ThrowForNonSuccess(Task task)
    {
      Contract.Assert(task.Status != TaskStatus.RanToCompletion);
      switch (task.Status)
      {
        case TaskStatus.Canceled:
          throw new TaskCanceledException(task);
        case TaskStatus.Faulted:
          throw TaskAwaiter.PrepareExceptionForRethrow(task.Exception.InnerException);
        default:
          throw new InvalidOperationException("The task has not yet completed.");
      }
    }

    internal static void OnCompletedInternal(
      Task task,
      Action continuation,
      bool continueOnCapturedContext)
    {
      if (continuation == null)
        throw new ArgumentNullException(nameof (continuation));
      SynchronizationContext sc = continueOnCapturedContext ? SynchronizationContext.Current : (SynchronizationContext) null;
      if (sc != null && (object) sc.GetType() != (object) typeof (SynchronizationContext))
      {
        task.ContinueWith((Action<Task>) (param0 =>
        {
          try
          {
            sc.Post((SendOrPostCallback) (state => ((Action) state)()), (object) continuation);
          }
          catch (Exception ex)
          {
            AsyncServices.ThrowAsync(ex, (SynchronizationContext) null);
          }
        }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
      }
      else
      {
        TaskScheduler scheduler = continueOnCapturedContext ? TaskScheduler.Current : TaskScheduler.Default;
        if (task.IsCompleted)
          Task.Factory.StartNew((Action<object>) (s => ((Action) s)()), (object) continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
        else if (scheduler != TaskScheduler.Default)
          task.ContinueWith((Action<Task>) (_ => TaskAwaiter.RunNoException(continuation)), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, scheduler);
        else
          task.ContinueWith((Action<Task>) (param0 =>
          {
            if (TaskAwaiter.IsValidLocationForInlining)
              TaskAwaiter.RunNoException(continuation);
            else
              Task.Factory.StartNew((Action<object>) (s => TaskAwaiter.RunNoException((Action) s)), (object) continuation, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
          }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
      }
    }

    private static void RunNoException(Action continuation)
    {
      try
      {
        continuation();
      }
      catch (Exception ex)
      {
        AsyncServices.ThrowAsync(ex, (SynchronizationContext) null);
      }
    }

    private static bool IsValidLocationForInlining
    {
      get
      {
        SynchronizationContext current = SynchronizationContext.Current;
        return (current == null || (object) current.GetType() == (object) typeof (SynchronizationContext)) && TaskScheduler.Current == TaskScheduler.Default;
      }
    }

    internal static Exception PrepareExceptionForRethrow(Exception exc) => exc;
  }
}
