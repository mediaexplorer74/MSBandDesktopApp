// Decompiled with JetBrains decompiler
// Type: Microsoft.Runtime.CompilerServices.ConfiguredTaskAwaitable
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{
  public struct ConfiguredTaskAwaitable
  {
    private readonly ConfiguredTaskAwaitable.ConfiguredTaskAwaiter m_configuredTaskAwaiter;

    internal ConfiguredTaskAwaitable(Task task, bool continueOnCapturedContext)
    {
      Contract.Assert(task != null);
      this.m_configuredTaskAwaiter = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter(task, continueOnCapturedContext);
    }

    public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter() => this.m_configuredTaskAwaiter;

    public struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
      private readonly Task m_task;
      private readonly bool m_continueOnCapturedContext;

      internal ConfiguredTaskAwaiter(Task task, bool continueOnCapturedContext)
      {
        Contract.Assert(task != null);
        this.m_task = task;
        this.m_continueOnCapturedContext = continueOnCapturedContext;
      }

      public bool IsCompleted => this.m_task.IsCompleted;

      public void OnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext);

      public void UnsafeOnCompleted(Action continuation) => TaskAwaiter.OnCompletedInternal(this.m_task, continuation, this.m_continueOnCapturedContext);

      public void GetResult() => TaskAwaiter.ValidateEnd(this.m_task);
    }
  }
}
