// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.LogExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities.Logging;
using System;
using System.Diagnostics;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class LogExtensions
  {
    public static void Debug(this ILog log, string format, params object[] args)
    {
      if (!log.IsDebugEnabled)
        return;
      log.DebugFormat(format, args);
    }

    public static void Error(this ILog log, string format, params object[] args)
    {
      if (!log.IsErrorEnabled)
        return;
      log.ErrorFormat(format, args);
    }

    public static void Error(
      this ILog log,
      Exception exception,
      string format,
      params object[] args)
    {
      if (!log.IsErrorEnabled)
        return;
      log.Error((object) string.Format(format, args), exception);
    }

    public static void ErrorAndDebug(this ILog log, Exception exception)
    {
      if (!log.IsErrorEnabled)
        return;
      log.Error((object) "An Exception Occurred.", exception);
      Debugger.Break();
    }

    public static void ErrorAndDebug(
      this ILog log,
      Exception exception,
      string format,
      params object[] args)
    {
      if (!log.IsErrorEnabled)
        return;
      log.Error((object) string.Format(format, args), exception);
      Debugger.Break();
    }

    public static void Fatal(this ILog log, string format, params object[] args)
    {
      if (!log.IsFatalEnabled)
        return;
      log.FatalFormat(format, args);
    }

    public static void Fatal(
      this ILog log,
      Exception exception,
      string format,
      params object[] args)
    {
      if (!log.IsFatalEnabled)
        return;
      log.Fatal((object) string.Format(format, args), exception);
    }

    public static void FatalAndDebug(this ILog log, Exception exception)
    {
      if (!log.IsFatalEnabled)
        return;
      log.Fatal((object) "An Exception Occurred.", exception);
      Debugger.Break();
    }

    public static void FatalAndDebug(this ILog log, Exception exception, string s)
    {
      if (!log.IsFatalEnabled)
        return;
      log.Fatal((object) s, exception);
      Debugger.Break();
    }

    public static void Info(this ILog log, string format, params object[] args)
    {
      if (!log.IsInfoEnabled)
        return;
      log.InfoFormat(format, args);
    }

    public static void Warn(this ILog log, string format, params object[] args)
    {
      if (!log.IsWarnEnabled)
        return;
      log.WarnFormat(format, args);
    }

    public static void Warn(
      this ILog log,
      Exception exception,
      string format,
      params object[] args)
    {
      if (!log.IsWarnEnabled)
        return;
      log.Warn((object) string.Format(format, args), exception);
    }

    public static void WarnAndDebug(
      this ILog log,
      Exception exception,
      string format,
      params object[] args)
    {
      if (!log.IsWarnEnabled)
        return;
      log.Warn((object) string.Format(format, args), exception);
      Debugger.Break();
    }

    public static void WarnAndDebug(this ILog log, string message)
    {
      if (!log.IsWarnEnabled)
        return;
      log.Warn((object) message);
    }

    public static IActivityManager CreateActivityManager(this ILog log) => (IActivityManager) new ActivityManager(log);

    internal static bool IsLevelEnabled(this ILog log, Level level)
    {
      switch (level)
      {
        case Level.Debug:
          return log.IsDebugEnabled;
        case Level.Info:
          return log.IsInfoEnabled;
        case Level.Warn:
          return log.IsWarnEnabled;
        case Level.Error:
          return log.IsErrorEnabled;
        case Level.Fatal:
          return log.IsFatalEnabled;
        default:
          return false;
      }
    }

    internal static void LogFormat(
      this ILog log,
      Level level,
      string format,
      params object[] args)
    {
      if (!log.IsLevelEnabled(level))
        return;
      switch (level)
      {
        case Level.Debug:
          log.DebugFormat(format, args);
          break;
        case Level.Info:
          log.InfoFormat(format, args);
          break;
        case Level.Warn:
          log.WarnFormat(format, args);
          break;
        case Level.Error:
          log.ErrorFormat(format, args);
          break;
        case Level.Fatal:
          log.FatalFormat(format, args);
          break;
      }
    }
  }
}
