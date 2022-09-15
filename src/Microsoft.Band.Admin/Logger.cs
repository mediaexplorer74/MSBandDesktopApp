// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Logger
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Microsoft.Band.Admin
{
  public class Logger
  {
    private static TraceListenerBase traceListenerInternal;

    private Logger()
    {
    }

    public static void SetInstance(TraceListenerBase traceListenerPassed) => Logger.traceListenerInternal = traceListenerPassed;

    public static void Log(LogLevel level, string message, params object[] args)
    {
      if (Logger.traceListenerInternal != null)
      {
        Logger.traceListenerInternal.Log(level, message, args);
      }
      else
      {
        if (level == LogLevel.Verbose)
          return;
        int length = args.Length;
      }
    }

    public static void LogException(LogLevel level, Exception e)
    {
      if (Logger.traceListenerInternal != null)
        Logger.traceListenerInternal.LogException(level, e, string.Empty);
    }

    public static void LogWebException(LogLevel level, WebException e)
    {
      if (Logger.traceListenerInternal != null)
      {
        string message = (string) null;
        try
        {
          using (StreamReader streamReader = new StreamReader(e.Response.GetResponseStream()))
            message = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
          Logger.traceListenerInternal.LogException(LogLevel.Warning, ex, "Unable to obtain WebException stream details. Only logging WebException.");
        }
        finally
        {
          Logger.traceListenerInternal.LogException(level, (Exception) e, message);
        }
      }
    }

    public static void LogException(
      LogLevel level,
      Exception e,
      string message,
      params object[] args)
    {
      if (Logger.traceListenerInternal != null)
      {
        Logger.traceListenerInternal.LogException(level, e, message, args);
      }
      else
      {
        if (level == LogLevel.Verbose)
          return;
        int length = args.Length;
      }
    }

    public static void PerfStart(string eventName)
    {
      if (Logger.traceListenerInternal == null)
        return;
      Logger.traceListenerInternal.PerfStart(eventName);
    }

    public static void PerfEnd(string eventName)
    {
      if (Logger.traceListenerInternal == null)
        return;
      Logger.traceListenerInternal.PerfEnd(eventName);
    }

    public static void TelemetryEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      if (Logger.traceListenerInternal == null)
        return;
      Logger.traceListenerInternal.TelemetryEvent(eventName, properties, metrics);
    }

    public static ICancellableTransaction TelemetryTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      return Logger.traceListenerInternal != null ? Logger.traceListenerInternal.TelemetryTimedEvent(eventName, properties, metrics) : (ICancellableTransaction) null;
    }
  }
}
