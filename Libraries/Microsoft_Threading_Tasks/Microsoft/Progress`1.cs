// Decompiled with JetBrains decompiler
// Type: Microsoft.Progress`1
// Assembly: Microsoft.Threading.Tasks, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 1C7D529D-87EC-4BDC-BDE0-2E9494F3B5AE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks.dll

using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Microsoft
{
  public class Progress<T> : IProgress<T>
  {
    private readonly SynchronizationContext m_synchronizationContext;
    private readonly Action<T> m_handler;
    private readonly SendOrPostCallback m_invokeHandlers;

    public Progress()
    {
      this.m_synchronizationContext = SynchronizationContext.Current ?? ProgressStatics.DefaultContext;
      Contract.Assert(this.m_synchronizationContext != null);
      this.m_invokeHandlers = new SendOrPostCallback(this.InvokeHandlers);
    }

    public Progress(Action<T> handler)
      : this()
    {
      this.m_handler = handler != null ? handler : throw new ArgumentNullException(nameof (handler));
    }

    public event ProgressEventHandler<T> ProgressChanged;

    protected virtual void OnReport(T value)
    {
      Action<T> handler = this.m_handler;
      ProgressEventHandler<T> progressChanged = this.ProgressChanged;
      if (handler == null && progressChanged == null)
        return;
      this.m_synchronizationContext.Post(this.m_invokeHandlers, (object) value);
    }

    void IProgress<T>.Report(T value) => this.OnReport(value);

    private void InvokeHandlers(object state)
    {
      T obj = (T) state;
      Action<T> handler = this.m_handler;
      ProgressEventHandler<T> progressChanged = this.ProgressChanged;
      if (handler != null)
        handler(obj);
      if (progressChanged == null)
        return;
      progressChanged((object) this, obj);
    }
  }
}
