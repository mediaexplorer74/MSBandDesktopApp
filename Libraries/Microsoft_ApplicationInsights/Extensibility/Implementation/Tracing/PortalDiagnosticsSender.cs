// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.PortalDiagnosticsSender
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class PortalDiagnosticsSender : IDiagnosticsSender
  {
    private const string AiPrefix = "AI: ";
    private const string AiNonUserActionable = "AI (Internal): ";
    private readonly TelemetryClient telemetryClient;
    private readonly IDiagnoisticsEventThrottlingManager throttlingManager;

    public PortalDiagnosticsSender(
      TelemetryConfiguration configuration,
      IDiagnoisticsEventThrottlingManager throttlingManager)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      if (throttlingManager == null)
        throw new ArgumentNullException(nameof (throttlingManager));
      this.telemetryClient = new TelemetryClient(configuration);
      this.throttlingManager = throttlingManager;
    }

    public string DiagnosticsInstrumentationKey { get; set; }

    public void Send(TraceEvent eventData)
    {
      try
      {
        if (eventData.MetaData == null || string.IsNullOrEmpty(eventData.MetaData.MessageFormat) || ThreadResourceLock.IsResourceLocked)
          return;
        using (new ThreadResourceLock())
        {
          try
          {
            if (this.throttlingManager.ThrottleEvent(eventData.MetaData.EventId, eventData.MetaData.Keywords))
              return;
            this.InternalSendTraceTelemetry(eventData);
          }
          catch (Exception ex)
          {
            CoreEventSource.Log.LogError("Failed to send traces to the portal: " + (object) ex);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void InternalSendTraceTelemetry(TraceEvent eventData)
    {
      if (this.telemetryClient.Channel == null)
        return;
      TraceTelemetry telemetry = new TraceTelemetry();
      string str1;
      if (eventData.Payload != null)
      {
        for (int index = 1; index <= ((IEnumerable<object>) eventData.Payload).Count<object>(); ++index)
          telemetry.Properties.Add(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "arg{0}", (object) index), eventData.Payload[index - 1].ToString());
        str1 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, eventData.MetaData.MessageFormat, ((IEnumerable<object>) eventData.Payload).ToArray<object>());
      }
      else
        str1 = eventData.MetaData.MessageFormat;
      string str2 = (eventData.MetaData.Keywords & 1L) != 1L ? "AI (Internal): " + str1 : "AI: " + str1;
      telemetry.Message = str2;
      if (!string.IsNullOrEmpty(this.DiagnosticsInstrumentationKey))
        telemetry.Context.InstrumentationKey = this.DiagnosticsInstrumentationKey;
      this.telemetryClient.TrackTrace(telemetry);
    }
  }
}
