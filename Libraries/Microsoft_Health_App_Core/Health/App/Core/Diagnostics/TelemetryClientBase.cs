// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.TelemetryClientBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public abstract class TelemetryClientBase : ITelemetryClient
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Diagnostics\\TelemetryClientBase.cs");

    public abstract void SetBandVersion(string bandVersion);

    public abstract void SetFirmwareVersion(string firmwareVersion);

    public abstract void SetInstrumentationKey(string instrumentationKey);

    public abstract void SetOdsUserId(Guid odsUserId);

    public void TrackEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      this.TrackEventInternal(eventName, properties, metrics);
      this.LogEventToDiagnosticsLog(eventName, "Event", properties, metrics);
    }

    public void TrackException(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      this.TrackExceptionInternal(exception, properties, metrics);
      this.LogEventToDiagnosticsLog(exception.ToString(), "Exception", properties, metrics);
    }

    public void TrackPageView(string pagePath)
    {
      this.TrackPageViewInternal(pagePath);
      this.LogEventToDiagnosticsLog(pagePath, "Pageview");
    }

    protected abstract void TrackEventInternal(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);

    protected abstract void TrackExceptionInternal(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics);

    protected abstract void TrackPageViewInternal(string pagePath);

    private void LogEventToDiagnosticsLog(
      string eventName,
      string eventType,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      string str = JsonConvert.SerializeObject((object) new TelemetryObject()
      {
        Name = eventName,
        Type = eventType,
        Metrics = metrics,
        Properties = properties
      });
      TelemetryClientBase.Logger.Debug((object) str);
    }
  }
}
