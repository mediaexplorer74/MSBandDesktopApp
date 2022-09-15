// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.LogManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public static class LogManager
  {
    private static readonly Dictionary<string, ILog> Logs = new Dictionary<string, ILog>();
    private static readonly ILogger UnconfiguredLogger = (ILogger) new Logger((ILoggerConfiguration) new LoggerConfiguration()
    {
      Appenders = (ICollection<IAppender>) new List<IAppender>(),
      Levels = (ICollection<Level>) new List<Level>()
    });
    private static ILogger rootLogger;

    public static void Start(ILoggerConfiguration configuration) => LogManager.RootLogger = configuration != null ? (ILogger) new Logger(configuration) : throw new ArgumentNullException(nameof (configuration));

    public static ILog GetFileLogger([CallerFilePath] string callerFilePath = null)
    {
      string name = Path.GetFileNameWithoutExtension(callerFilePath);
      int num = name.LastIndexOf('\\');
      if (num >= 0)
        name = name.Substring(num + 1);
      return LogManager.GetLogger(name);
    }

    public static ILog GetLogger(string name)
    {
      ILog log1;
      LogManager.Logs.TryGetValue(name, out log1);
      if (log1 != null)
        return log1;
      ILog log2 = (ILog) new Log(name);
      LogManager.Logs.Add(name, log2);
      return log2;
    }

    public static ILogger RootLogger
    {
      get => LogManager.rootLogger != null ? LogManager.rootLogger : LogManager.UnconfiguredLogger;
      private set => LogManager.rootLogger = value;
    }

    public static void Shutdown()
    {
      LogManager.RootLogger.Dispose();
      LogManager.Logs.Clear();
      LogManager.RootLogger = (ILogger) null;
    }
  }
}
