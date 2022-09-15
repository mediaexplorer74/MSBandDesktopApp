// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.LogEventInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;
using System.Text;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public class LogEventInfo
  {
    private LogEventInfo()
    {
    }

    public LogLevel Level { get; private set; }

    public string Message { get; private set; }

    public object[] Parameters { get; private set; }

    public Exception Exception { get; private set; }

    public string FormattedMessage => this.Message != null ? (this.Parameters != null && this.Parameters.Length != 0 ? string.Format(this.Message, this.Parameters) : this.Message) : (this.Exception != null ? this.Exception.Message : (string) null);

    public DateTime Timestamp { get; private set; }

    public string LoggerName { get; private set; }

    public int ThreadId { get; private set; }

    public static LogEventInfo Create(
      LogLevel level,
      string message,
      object[] parameters,
      Exception exception,
      string loggerName)
    {
      LogEventInfo logEventInfo = new LogEventInfo();
      logEventInfo.Init();
      logEventInfo.Level = level;
      logEventInfo.Message = message;
      logEventInfo.Parameters = parameters;
      logEventInfo.Exception = exception;
      logEventInfo.LoggerName = loggerName;
      return logEventInfo;
    }

    private void Init()
    {
      this.Timestamp = DateTime.Now;
      this.ThreadId = Environment.CurrentManagedThreadId;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = " : ";
      stringBuilder.AppendFormat("{0:yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff}", new object[1]
      {
        (object) this.Timestamp
      });
      stringBuilder.Append(str);
      stringBuilder.Append((object) this.Level);
      stringBuilder.Append(str);
      stringBuilder.Append(this.ThreadId);
      if (!string.IsNullOrWhiteSpace(this.LoggerName))
      {
        stringBuilder.Append(str);
        stringBuilder.Append(this.LoggerName);
      }
      if (!string.IsNullOrWhiteSpace(this.FormattedMessage))
      {
        stringBuilder.Append(str);
        stringBuilder.Append(this.FormattedMessage);
      }
      if (this.Exception != null)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append((object) this.Exception);
      }
      return stringBuilder.ToString();
    }
  }
}
