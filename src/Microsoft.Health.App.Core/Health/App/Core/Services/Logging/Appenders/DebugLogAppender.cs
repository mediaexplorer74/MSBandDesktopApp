// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Appenders.DebugLogAppender
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.IO;
using System.Text;

namespace Microsoft.Health.App.Core.Services.Logging.Appenders
{
  public class DebugLogAppender : AppenderBase
  {
    private StringWriter writer;
    private StringBuilder writerBuilder;

    public DebugLogAppender()
    {
      this.writer = new StringWriter();
      this.writerBuilder = this.writer.GetStringBuilder();
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      lock (this.writer)
      {
        this.Layout.Format((TextWriter) this.writer, loggingEvent);
        this.writerBuilder.Length = 0;
        if (this.writerBuilder.Capacity <= 8192)
          return;
        this.writerBuilder.Capacity = 8192;
      }
    }
  }
}
