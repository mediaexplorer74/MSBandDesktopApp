// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Logging.HealthApplicationLogUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Configurations;
using Microsoft.Health.App.Core.Services.Logging.Framework;

namespace Microsoft.Health.App.Core.Utilities.Logging
{
  public static class HealthApplicationLogUtilities
  {
    public static void StartLoggingAndInjectListeners(ILoggerConfiguration configuration)
    {
      LogManager.Start(configuration);
      ILog fileLogger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Utilities\\Logging\\HealthApplicationLogUtilities.cs");
      fileLogger.InfoFormat("Logging System Started. {0}", (object) configuration.ToDebugString());
      fileLogger.Debug((object) "Configuring Microsoft Band software development kit to use the logging system...");
      Microsoft.Band.Admin.Logger.SetInstance((TraceListenerBase) new KdkTraceListener());
    }
  }
}
