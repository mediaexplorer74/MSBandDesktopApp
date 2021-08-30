// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.BaseTelemetryListener
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public abstract class BaseTelemetryListener : ITelemetryListener
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Diagnostics\\BaseTelemetryListener.cs");

    protected ITelemetryClient Client { get; set; }

    public abstract Task InitializeAsync();

    public void SetOdsUserId(Guid odsUserId) => this.Client.SetOdsUserId(odsUserId);

    public void SetBandVersion(string bandVersion) => this.Client.SetBandVersion(bandVersion);

    public void SetFirmwareVersion(string firmwareVersion) => this.Client.SetFirmwareVersion(firmwareVersion);

    public void LogEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      this.Client.TrackEvent(eventName, properties, metrics);
    }

    public void LogPageView(string pagePath) => this.Client.TrackPageView(pagePath);

    public void LogException(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      this.Client.TrackException(exception, properties, metrics);
    }

    public ITimedTelemetryEvent StartTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      TelemetryEvent eventTelemetry = new TelemetryEvent(eventName);
      if (properties != null)
      {
        foreach (string key in (IEnumerable<string>) properties.Keys)
          eventTelemetry.Properties.Add(key, properties[key]);
      }
      if (metrics != null)
      {
        foreach (string key in (IEnumerable<string>) metrics.Keys)
          eventTelemetry.Metrics.Add(key, metrics[key]);
      }
      eventTelemetry.Timestamp = DateTimeOffset.Now;
      return (ITimedTelemetryEvent) new BaseTelemetryListener.TimedAnalyticsEvent(eventTelemetry, this.Client);
    }

    private class TimedAnalyticsEvent : ITimedTelemetryEvent, IDisposable
    {
      private TelemetryEvent eventTelemetry;
      private ITelemetryClient client;
      private Stopwatch stopwatch;
      private bool cancelled;
      private bool disposed;

      public TimedAnalyticsEvent(TelemetryEvent eventTelemetry, ITelemetryClient client)
      {
        this.eventTelemetry = eventTelemetry;
        this.client = client;
        this.stopwatch = Stopwatch.StartNew();
      }

      public void Cancel() => this.cancelled = true;

      public void End() => this.Dispose();

      public void Dispose()
      {
        if (this.disposed)
          return;
        this.stopwatch.Stop();
        if (!this.cancelled)
        {
          this.eventTelemetry.Metrics["DurationMs"] = (double) this.stopwatch.ElapsedMilliseconds;
          TelemetryObject telemetryObject = new TelemetryObject()
          {
            Name = this.eventTelemetry.Name,
            Type = "Event",
            Metrics = this.eventTelemetry.Metrics,
            Properties = this.eventTelemetry.Properties,
            Sequence = this.eventTelemetry.Sequence,
            Timestamp = this.eventTelemetry.Timestamp
          };
          this.client.TrackEvent(telemetryObject.Name, telemetryObject.Properties, telemetryObject.Metrics);
          BaseTelemetryListener.Logger.Debug((object) JsonConvert.SerializeObject((object) telemetryObject));
        }
        this.disposed = true;
      }

      public void AddProperty(string key, string value) => this.eventTelemetry.Properties[key] = value;

      public void AddMetric(string key, double value) => this.eventTelemetry.Metrics[key] = value;
    }
  }
}
