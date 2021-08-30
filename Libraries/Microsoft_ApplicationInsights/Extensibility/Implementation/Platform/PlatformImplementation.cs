// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Platform.PlatformImplementation
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
  internal class PlatformImplementation : IPlatform
  {
    public IDictionary<string, object> GetApplicationSettings() => throw new NotImplementedException();

    public string ReadConfigurationXml()
    {
      string path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "ApplicationInsights.config");
      if (!File.Exists(path))
        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");
      return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
    }

    public ExceptionDetails GetExceptionDetails(
      Exception exception,
      ExceptionDetails parentExceptionDetails)
    {
      return ExceptionConverter.ConvertToExceptionDetails(exception, parentExceptionDetails);
    }

    public IDebugOutput GetDebugOutput() => (IDebugOutput) new DebugOutput();
  }
}
