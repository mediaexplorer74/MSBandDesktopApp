// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Layouts.VisualStudioDebugLayout
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.IO;

namespace Microsoft.Health.App.Core.Services.Logging.Layouts
{
  public class VisualStudioDebugLayout : ILogLineLayout
  {
    private const string ColumnSeparator = " : ";
    private const int LevelColumnWidth = 5;
    private const int LoggerNameColumnWidth = 30;
    private const int ThreadNameColumnWidth = 2;

    public void Format(TextWriter textWriter, LoggingEvent loggingEvent)
    {
      textWriter.Write("{0:yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff}", new object[1]
      {
        (object) loggingEvent.TimeStamp
      });
      textWriter.Write(" : ");
      textWriter.Write(loggingEvent.Level.ToString().ForceLengthPadLeft(5));
      textWriter.Write(" : ");
      textWriter.Write(loggingEvent.LoggerName.ForceLengthPadLeft(30));
      textWriter.Write(" : ");
      textWriter.Write(loggingEvent.ThreadName.ForceLengthPadLeft(2));
      textWriter.Write(" : ");
      textWriter.Write(loggingEvent.Message);
      if (loggingEvent.ExceptionObject == null)
        return;
      textWriter.Write(textWriter.NewLine);
      textWriter.Write(textWriter.NewLine);
      textWriter.Write((object) loggingEvent.ExceptionObject);
      textWriter.Write(textWriter.NewLine);
    }
  }
}
