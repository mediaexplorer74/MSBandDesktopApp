// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TraceListenerBase
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  public abstract class TraceListenerBase
  {
    public virtual void Log(LogLevel level, string message, params object[] args)
    {
    }

    public virtual void LogException(
      LogLevel level,
      Exception e,
      string message,
      params object[] args)
    {
    }

    public virtual void PerfStart(string eventName)
    {
    }

    public virtual void PerfEnd(string eventName)
    {
    }

    public virtual void TelemetryEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
    }

    public virtual void TelemetryPageView(string pagePath)
    {
    }

    public virtual ICancellableTransaction TelemetryTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      return (ICancellableTransaction) null;
    }
  }
}
