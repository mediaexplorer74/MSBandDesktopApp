// Decompiled with JetBrains decompiler
// Type: Microsoft.Runtime.CompilerServices.TaskAwaiter`1
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{
  public struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
  {
    private readonly Task<TResult> m_task;

    internal TaskAwaiter(Task<TResult> task)
    {
      Contract.Assert(task != null);
      this.m_task = task;
    }

    public bool IsCompleted => this.m_task.IsCompleted;

    public void OnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal((Task) this.m_task, continuation, true);

    public void UnsafeOnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal((Task) this.m_task, continuation, true);

    public TResult GetResult()
    {
      TaskAwaiter.ValidateEnd((Task) this.m_task);
      return this.m_task.Result;
    }
  }
}
