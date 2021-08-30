// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.LoggingEvent
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class LoggingEvent
  {
    public LoggingEvent(
      Level level,
      string message,
      string loggerName,
      DateTimeOffset timeStamp,
      string threadName,
      Exception exception = null)
    {
      this.ThreadName = threadName;
      this.ExceptionObject = exception;
      this.TimeStamp = timeStamp;
      this.LoggerName = loggerName;
      this.Message = message;
      this.Level = level;
    }

    public Level Level { get; private set; }

    public string Message { get; private set; }

    public string LoggerName { get; private set; }

    public DateTimeOffset TimeStamp { get; private set; }

    public string ThreadName { get; private set; }

    public Exception ExceptionObject { get; private set; }
  }
}
