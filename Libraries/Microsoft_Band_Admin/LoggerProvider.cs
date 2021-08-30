// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LoggerProvider
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;
using System.Net;

namespace Microsoft.Band.Admin
{
  internal class LoggerProvider : ILoggerProvider
  {
    public void Log(ProviderLogLevel level, string message, params object[] args) => Logger.Log(level.ToLogLevel(), message, args);

    public void LogException(ProviderLogLevel level, Exception e) => Logger.LogException(level.ToLogLevel(), e);

    public void LogWebException(ProviderLogLevel level, WebException e) => Logger.LogWebException(level.ToLogLevel(), e);

    public void LogException(
      ProviderLogLevel level,
      Exception e,
      string message,
      params object[] args)
    {
      Logger.LogException(level.ToLogLevel(), e, message, args);
    }

    public void PerfStart(string eventName) => Logger.PerfStart(eventName);

    public void PerfEnd(string eventName) => Logger.PerfEnd(eventName);

    public void TelemetryEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      Logger.TelemetryEvent(eventName, properties, metrics);
    }
  }
}
