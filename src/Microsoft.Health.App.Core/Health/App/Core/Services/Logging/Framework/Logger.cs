// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.Logger
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class Logger : ILogger, IDisposable
  {
    public Logger(ILoggerConfiguration configuration)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      if (configuration.Appenders.Any<IAppender>((Func<IAppender, bool>) (appender => appender == null)))
        throw new Exception("The current logging configuration specified a null appender.");
      this.Configuration = configuration;
      this.ActiveLevels = this.Configuration.Appenders.SelectMany<IAppender, Level>((Func<IAppender, IEnumerable<Level>>) (appender => appender.Levels)).Distinct<Level>();
    }

    public ILoggerConfiguration Configuration { get; private set; }

    private IEnumerable<Level> ActiveLevels { get; set; }

    public bool IsEnabledFor(Level level) => this.ActiveLevels.Contains<Level>(level);

    public void Log(LoggingEvent loggingEvent)
    {
      foreach (IAppender appender in this.Configuration.Appenders.Where<IAppender>((Func<IAppender, bool>) (appender => appender.Levels.Contains<Level>(loggingEvent.Level))))
        appender.DoAppend(loggingEvent);
    }

    public void Dispose()
    {
      foreach (IAppender appender in (IEnumerable<IAppender>) this.Configuration.Appenders)
        appender.Close();
    }
  }
}
