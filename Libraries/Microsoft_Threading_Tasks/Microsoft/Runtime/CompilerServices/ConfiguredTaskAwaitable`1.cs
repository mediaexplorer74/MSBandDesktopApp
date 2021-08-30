// Decompiled with JetBrains decompiler
// Type: Microsoft.Runtime.CompilerServices.ConfiguredTaskAwaitable`1
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{
  public struct ConfiguredTaskAwaitable<TResult>
  {
    private readonly ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter m_configuredTaskAwaiter;

    internal ConfiguredTaskAwaitable(Task<TResult> task, bool continueOnCapturedContext) => this.m_configuredTaskAwaiter = new ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter(task, continueOnCapturedContext);

    public ConfiguredTaskAwaitable<TResult>.ConfiguredTaskAwaiter GetAwaiter() => this.m_configuredTaskAwaiter;

    public struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
      private readonly Task<TResult> m_task;
      private readonly bool m_continueOnCapturedContext;

      internal ConfiguredTaskAwaiter(Task<TResult> task, bool continueOnCapturedContext)
      {
        Contract.Assert(task != null);
        this.m_task = task;
        this.m_continueOnCapturedContext = continueOnCapturedContext;
      }

      public bool IsCompleted => this.m_task.IsCompleted;

      public void OnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal((Task) this.m_task, continuation, this.m_continueOnCapturedContext);

      public void UnsafeOnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal((Task) this.m_task, continuation, this.m_continueOnCapturedContext);

      public TResult GetResult()
      {
        TaskAwaiter.ValidateEnd((Task) this.m_task);
        return this.m_task.Result;
      }
    }
  }
}
