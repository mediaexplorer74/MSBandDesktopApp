// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.ITelemetryListener
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public interface ITelemetryListener
  {
    Task InitializeAsync();

    void SetOdsUserId(Guid odsUserId);

    void SetBandVersion(string bandVersion);

    void SetFirmwareVersion(string firmwareVersion);

    void LogEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);

    void LogPageView(string pagePath);

    void LogException(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);

    ITimedTelemetryEvent StartTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);
  }
}
