// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Layouts.DefaultLogLineLayout
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Microsoft.Health.App.Core.Services.Logging.Layouts
{
  public class DefaultLogLineLayout : ILogLineLayout
  {
    private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
    {
      DateFormatHandling = DateFormatHandling.IsoDateFormat,
      Formatting = Formatting.None,
      NullValueHandling = NullValueHandling.Ignore
    });

    public void Format(TextWriter textWriter, LoggingEvent loggingEvent)
    {
      LogEntry logEntry = new LogEntry()
      {
        Time = loggingEvent.TimeStamp,
        Category = loggingEvent.LoggerName,
        Level = DefaultLogLineLayout.ConvertLevel(loggingEvent.Level),
        Message = loggingEvent.Message
      };
      Exception exceptionObject = loggingEvent.ExceptionObject;
      if (exceptionObject != null)
        logEntry.Exception = new LogEntryException()
        {
          Type = ((object) exceptionObject).GetType().FullName,
          Message = exceptionObject.Message,
          Info = exceptionObject.ToString()
        };
      DefaultLogLineLayout.Serializer.Serialize(textWriter, (object) logEntry);
      textWriter.Write(textWriter.NewLine);
    }

    private static string ConvertLevel(Level level)
    {
      switch (level)
      {
        case Level.Debug:
          return "Debug";
        case Level.Info:
          return "Info";
        case Level.Warn:
          return "Warning";
        case Level.Error:
        case Level.Fatal:
          return "Error";
        default:
          throw new ArgumentOutOfRangeException(nameof (level));
      }
    }
  }
}
