// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Configurations.LoggerConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services.Logging.Configurations
{
  public class LoggerConfiguration : ILoggerConfiguration
  {
    public ICollection<Level> Levels { get; set; }

    public ICollection<IAppender> Appenders { get; set; }
  }
}
