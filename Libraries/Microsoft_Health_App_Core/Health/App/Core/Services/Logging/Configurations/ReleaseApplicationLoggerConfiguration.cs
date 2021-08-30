// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Configurations.ReleaseApplicationLoggerConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Appenders;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Logging.Layouts;
using Microsoft.Health.App.Core.Services.Storage;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services.Logging.Configurations
{
  public class ReleaseApplicationLoggerConfiguration : ILoggerConfiguration
  {
    private readonly List<IAppender> appenders;

    public ReleaseApplicationLoggerConfiguration(
      IApplicationLifecycleService applicationLifecycleService,
      IFileSystemService fileSystemService)
    {
      StorageFileAppender storageFileAppender = new StorageFileAppender(applicationLifecycleService, fileSystemService, "App");
      storageFileAppender.Name = "Phone Storage Logging Appender";
      storageFileAppender.Layout = (ILogLineLayout) new DefaultLogLineLayout();
      storageFileAppender.AutoFlush = false;
      storageFileAppender.AutoFlushOverride = Level.Error;
      storageFileAppender.AppendToExisting = true;
      storageFileAppender.MaxLogFiles = 10;
      storageFileAppender.MaxFileSize = new long?(1048576L);
      storageFileAppender.Levels = (IEnumerable<Level>) new List<Level>()
      {
        Level.Debug,
        Level.Error,
        Level.Fatal,
        Level.Info,
        Level.Warn
      };
      this.appenders = new List<IAppender>()
      {
        (IAppender) storageFileAppender
      };
    }

    public ICollection<IAppender> Appenders => (ICollection<IAppender>) this.appenders;
  }
}
