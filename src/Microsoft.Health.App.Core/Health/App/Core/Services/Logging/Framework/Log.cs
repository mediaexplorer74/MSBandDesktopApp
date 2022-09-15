// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.Log
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class Log : ILog
  {
    private readonly string loggerName;

    public Log(string loggerName) => this.loggerName = loggerName != null ? loggerName : throw new ArgumentNullException(nameof (loggerName));

    public bool IsDebugEnabled => LogManager.RootLogger.IsEnabledFor(Level.Debug);

    public bool IsErrorEnabled => LogManager.RootLogger.IsEnabledFor(Level.Error);

    public bool IsFatalEnabled => LogManager.RootLogger.IsEnabledFor(Level.Fatal);

    public bool IsInfoEnabled => LogManager.RootLogger.IsEnabledFor(Level.Info);

    public bool IsWarnEnabled => LogManager.RootLogger.IsEnabledFor(Level.Warn);

    public void Debug(object message)
    {
      if (!this.IsDebugEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Debug, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName));
    }

    public void Debug(object message, Exception exception)
    {
      if (!this.IsDebugEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Debug, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName, exception));
    }

    public void DebugFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Debug, string.Format(provider, format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Debug((object) format);
    }

    public void DebugFormat(string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Debug, string.Format(format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Debug((object) format);
    }

    public void Error(object message)
    {
      if (!this.IsErrorEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Error, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName));
    }

    public void Error(object message, Exception exception)
    {
      if (!this.IsErrorEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Error, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName, exception));
    }

    public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Error, string.Format(provider, format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Error((object) format);
    }

    public void ErrorFormat(string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Error, string.Format(format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Error((object) format);
    }

    public void Fatal(object message)
    {
      if (!this.IsFatalEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Fatal, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName));
    }

    public void Fatal(object message, Exception exception)
    {
      if (!this.IsFatalEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Fatal, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName, exception));
    }

    public void FatalFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Fatal, string.Format(provider, format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Fatal((object) format);
    }

    public void FatalFormat(string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Fatal, string.Format(format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Fatal((object) format);
    }

    public void Info(object message)
    {
      if (!this.IsInfoEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Info, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName));
    }

    public void Info(object message, Exception exception)
    {
      if (!this.IsInfoEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Info, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName, exception));
    }

    public void InfoFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Info, string.Format(provider, format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Info((object) format);
    }

    public void InfoFormat(string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Info, string.Format(format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Info((object) format);
    }

    public void Warn(object message)
    {
      if (!this.IsWarnEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Warn, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName));
    }

    public void Warn(object message, Exception exception)
    {
      if (!this.IsWarnEnabled)
        return;
      LogManager.RootLogger.Log(new LoggingEvent(Level.Warn, message.ToString(), this.loggerName, Log.TimeStamp, Log.ThreadName, exception));
    }

    public void WarnFormat(IFormatProvider provider, string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Warn, string.Format(provider, format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Warn((object) format);
    }

    public void WarnFormat(string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      if (args.Length != 0)
        LogManager.RootLogger.Log(new LoggingEvent(Level.Warn, string.Format(format, args), this.loggerName, Log.TimeStamp, Log.ThreadName));
      else
        this.Warn((object) format);
    }

    protected static string ThreadName => Environment.CurrentManagedThreadId.ToString();

    protected static DateTimeOffset TimeStamp => DateTimeOffset.Now;
  }
}
