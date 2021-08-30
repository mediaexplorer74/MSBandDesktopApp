// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.SimpleLogFilter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class SimpleLogFilter : LogFilterBase
  {
    public SimpleLogFilter(Func<LoggingEvent, bool> filterFunction) => this.FilterFunction = filterFunction;

    private Func<LoggingEvent, bool> FilterFunction { get; set; }

    public override bool IsFilteredOut(LoggingEvent loggingEvent)
    {
      try
      {
        return this.FilterFunction(loggingEvent);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
