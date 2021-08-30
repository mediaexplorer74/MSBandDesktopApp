// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.KdkTraceListener
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public class KdkTraceListener : TraceListenerBase
  {
    private static readonly ILog Logger = LogManager.GetLogger("Band Software Development Kit");

    public override void Log(LogLevel level, string message, params object[] args)
    {
      switch (level)
      {
        case LogLevel.Off:
          break;
        case LogLevel.Fatal:
          KdkTraceListener.Logger.Fatal(message, args);
          break;
        case LogLevel.Error:
          KdkTraceListener.Logger.Error(message, args);
          break;
        case LogLevel.Warning:
          KdkTraceListener.Logger.Warn(message, args);
          break;
        case LogLevel.Info:
          KdkTraceListener.Logger.Info(message, args);
          break;
        case LogLevel.Verbose:
          KdkTraceListener.Logger.Debug(message, args);
          break;
        default:
          Debugger.Break();
          KdkTraceListener.Logger.Debug(message, args);
          break;
      }
    }

    public override void LogException(
      LogLevel level,
      Exception e,
      string message,
      params object[] args)
    {
      switch (level)
      {
        case LogLevel.Off:
          break;
        case LogLevel.Fatal:
          KdkTraceListener.Logger.Fatal(e, message, args);
          break;
        case LogLevel.Error:
          KdkTraceListener.Logger.Error(e, message, args);
          break;
        case LogLevel.Warning:
          KdkTraceListener.Logger.Warn(e, message, args);
          break;
        case LogLevel.Info:
          KdkTraceListener.Logger.Info(message + Environment.NewLine + (object) e, args);
          break;
        case LogLevel.Verbose:
          KdkTraceListener.Logger.Debug(message + Environment.NewLine + (object) e, args);
          break;
        default:
          Debugger.Break();
          KdkTraceListener.Logger.Debug(message + Environment.NewLine + (object) e, args);
          break;
      }
    }

    public override void TelemetryEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      Telemetry.LogEvent(eventName, properties, metrics);
    }

    public override ICancellableTransaction TelemetryTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      ITimedTelemetryEvent telemetryEvent = Telemetry.StartTimedEvent(eventName, properties, metrics);
      return telemetryEvent == null ? (ICancellableTransaction) null : (ICancellableTransaction) new KdkTraceListener.CancellabeTrasactionAdapter(telemetryEvent);
    }

    public override void TelemetryPageView(string pagePath) => Telemetry.LogPageView(pagePath);

    private class CancellabeTrasactionAdapter : ICancellableTransaction, IDisposable
    {
      private readonly ITimedTelemetryEvent telemetryEvent;

      public CancellabeTrasactionAdapter(ITimedTelemetryEvent telemetryEvent) => this.telemetryEvent = telemetryEvent;

      public void Cancel() => this.telemetryEvent.Cancel();

      public void End() => this.telemetryEvent.End();

      public void Dispose() => this.telemetryEvent.Dispose();
    }
  }
}
