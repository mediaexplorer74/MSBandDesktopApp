// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Configurations.DebugApplicationLoggerConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Appenders;
using Microsoft.Health.App.Core.Services.Logging.Filters;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Logging.Layouts;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Services.Logging.Configurations
{
  public class DebugApplicationLoggerConfiguration : ILoggerConfiguration
  {
    private const string LoggingCategory = "Logging";
    private readonly IConfigurationService configurationService;
    private readonly List<IAppender> appenders;
    public static readonly ConfigurationValue<bool> IsBandSoftwareDevelopmentKitLoggingOnConsoleEnabledConfigurationValue = ConfigurationValue.CreateBoolean("Logging", "Band SDK", (Func<bool>) (() => false));
    public static readonly MultipleSelectionConfigurationValue LoggingLevelsShownOnConsoleConfigurationValue = ConfigurationValue.CreateMultipleSelectables("Logging", "Levels", typeof (Level), (Func<IList<Enum>>) (() => (IList<Enum>) new List<Enum>()
    {
      (Enum) Level.Error,
      (Enum) Level.Fatal,
      (Enum) Level.Info,
      (Enum) Level.Warn
    }), (Func<IList<Enum>, string>) (list => ServiceLocator.Current.GetInstance<ISerializationService>().Serialize<IEnumerable<Level>>(list.Cast<Level>())), (Func<string, IList<Enum>>) (serialized => (IList<Enum>) ServiceLocator.Current.GetInstance<ISerializationService>().Deserialize<IList<Level>>(serialized).Cast<Enum>().ToList<Enum>()));

    private bool IsBandSoftwareDevelopmentKitLoggingOnConsoleEnabled => this.configurationService.GetValue<bool>(DebugApplicationLoggerConfiguration.IsBandSoftwareDevelopmentKitLoggingOnConsoleEnabledConfigurationValue);

    private IEnumerable<Level> LoggingLevelsShownOnConsole => (IEnumerable<Level>) this.configurationService.GetValue<IList<Enum>>((ConfigurationValue<IList<Enum>>) DebugApplicationLoggerConfiguration.LoggingLevelsShownOnConsoleConfigurationValue).Cast<Level>().ToList<Level>();

    public DebugApplicationLoggerConfiguration(
      IApplicationLifecycleService applicationLifecycleService,
      IFileSystemService fileSystemService,
      IConfigurationService configurationService)
    {
      this.configurationService = configurationService;
      DebugLogAppender debugLogAppender1 = new DebugLogAppender();
      debugLogAppender1.Name = "Console Logging Appender";
      debugLogAppender1.Layout = (ILogLineLayout) new VisualStudioDebugLayout();
      debugLogAppender1.Levels = this.LoggingLevelsShownOnConsole;
      DebugLogAppender debugLogAppender2 = debugLogAppender1;
      StorageFileAppender storageFileAppender1 = new StorageFileAppender(applicationLifecycleService, fileSystemService, "App");
      storageFileAppender1.Name = "Phone Storage Logging Appender";
      storageFileAppender1.Layout = (ILogLineLayout) new DefaultLogLineLayout();
      storageFileAppender1.AutoFlush = false;
      storageFileAppender1.AutoFlushOverride = Level.Error;
      storageFileAppender1.AppendToExisting = true;
      storageFileAppender1.MaxLogFiles = 10;
      storageFileAppender1.MaxFileSize = new long?(1048576L);
      storageFileAppender1.Levels = (IEnumerable<Level>) new List<Level>()
      {
        Level.Debug,
        Level.Error,
        Level.Fatal,
        Level.Info,
        Level.Warn
      };
      StorageFileAppender storageFileAppender2 = storageFileAppender1;
      if (!this.IsBandSoftwareDevelopmentKitLoggingOnConsoleEnabled)
      {
        BandSoftwareDevelopmentKitLogFilter developmentKitLogFilter1 = new BandSoftwareDevelopmentKitLogFilter();
        developmentKitLogFilter1.Name = "Band Software Development Kit Logging Filter";
        BandSoftwareDevelopmentKitLogFilter developmentKitLogFilter2 = developmentKitLogFilter1;
        debugLogAppender2.FilterHead = (ILogFilter) developmentKitLogFilter2;
      }
      this.appenders = new List<IAppender>()
      {
        (IAppender) debugLogAppender2,
        (IAppender) storageFileAppender2
      };
    }

    public ICollection<IAppender> Appenders => (ICollection<IAppender>) this.appenders;
  }
}
