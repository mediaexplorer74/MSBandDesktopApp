// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.AppenderBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class AppenderBase : IAppender
  {
    public string Name { get; set; }

    public ILogLineLayout Layout { get; set; }

    public ILogFilter FilterHead { get; set; }

    public IEnumerable<Level> Levels { get; set; }

    public virtual void Close()
    {
    }

    public void DoAppend(LoggingEvent loggingEvent)
    {
      if (this.Layout != null)
      {
        if (this.IsFiltered(loggingEvent))
          return;
        this.Append(loggingEvent);
      }
      else
        throw new Exception(string.Format("The current logging configuration specified an appender named {0} without specifying a layout.", new object[1]
        {
          (object) this.Name
        }));
    }

    protected virtual void Append(LoggingEvent loggingEvent)
    {
    }

    private bool IsFiltered(LoggingEvent loggingEvent)
    {
      for (ILogFilter logFilter = this.FilterHead; logFilter != null; logFilter = logFilter.Next())
      {
        if (logFilter.IsFilteredOut(loggingEvent))
          return true;
      }
      return false;
    }
  }
}
