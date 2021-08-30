// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.AsyncLocal`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class AsyncLocal<T>
  {
    public AsyncLocal()
    {
    }

    public AsyncLocal(
      Action<AsyncLocalValueChangedArgs<T>> valueChangedHandler)
    {
    }

    public T Value
    {
      get
      {
        object obj = (object) null;
        return obj != null ? (T) obj : default (T);
      }
      set
      {
      }
    }
  }
}
