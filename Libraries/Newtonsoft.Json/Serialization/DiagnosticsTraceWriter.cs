// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DiagnosticsTraceWriter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
  public class DiagnosticsTraceWriter : ITraceWriter
  {
    public TraceLevel LevelFilter { get; set; }

    private TraceEventType GetTraceEventType(TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Error:
          return TraceEventType.Error;
        case TraceLevel.Warning:
          return TraceEventType.Warning;
        case TraceLevel.Info:
          return TraceEventType.Information;
        case TraceLevel.Verbose:
          return TraceEventType.Verbose;
        default:
          throw new ArgumentOutOfRangeException(nameof (level));
      }
    }

    public void Trace(TraceLevel level, string message, Exception ex)
    {
      if (level == TraceLevel.Off)
        return;
      TraceEventCache eventCache = new TraceEventCache();
      TraceEventType traceEventType = this.GetTraceEventType(level);
      foreach (TraceListener listener in System.Diagnostics.Trace.Listeners)
      {
        if (!listener.IsThreadSafe)
        {
          lock (listener)
            listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        }
        else
          listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        if (System.Diagnostics.Trace.AutoFlush)
          listener.Flush();
      }
    }
  }
}
