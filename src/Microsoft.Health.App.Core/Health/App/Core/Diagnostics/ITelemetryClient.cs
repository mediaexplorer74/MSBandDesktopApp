// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.ITelemetryClient
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public interface ITelemetryClient
  {
    void SetInstrumentationKey(string instrumentationKey);

    void SetOdsUserId(Guid odsUserId);

    void SetBandVersion(string bandVersion);

    void SetFirmwareVersion(string firmwareVersion);

    void TrackException(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);

    void TrackPageView(string pagePath);

    void TrackEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);
  }
}
