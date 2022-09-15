// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ApplicationInsightsTelemetryListener
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public class ApplicationInsightsTelemetryListener : ITelemetryListener
  {
    private const string DurationMsMetricName = "DurationMs";
    private const string ODSUserIdName = "ODSUserId";
    private const string BandVersionName = "BandVersion";
    private const string FirmwareVersionName = "FirmwareVersion";
    private TelemetryClient client;
    private string BuildInfoSubKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";

    public ApplicationInsightsTelemetryListener(string instrumentationKey)
    {
      this.client = new TelemetryClient();
      this.client.Context.InstrumentationKey = instrumentationKey;
      this.client.Context.User.Id = Guid.Empty.ToString();
      this.client.Context.Session.Id = Guid.NewGuid().ToString();
      this.client.Context.Component.Version = Globals.ApplicationVersion.ToString();
      this.client.Context.Device.Language = CultureInfo.CurrentCulture.Name;
      this.client.Context.Device.OperatingSystem = this.GetWindowsVersion();
    }

    public Task InitializeAsync() => throw new NotImplementedException();

    private string GetWindowsVersion()
    {
      string str = "";
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(this.BuildInfoSubKey);
        str = string.Format("{0} {1}.{2}.{3}.0", registryKey.GetValue("ProductName"), registryKey.GetValue("CurrentMajorVersionNumber"), registryKey.GetValue("CurrentMinorVersionNumber"), registryKey.GetValue("CurrentBuildNumber"));
      }
      catch
      {
      }
      return str;
    }

    public void SetOdsUserId(Guid odsUserId)
    {
      this.client.Flush();
      this.client.Context.User.Id = odsUserId.ToString("D");
      this.client.Context.Session.Id = Guid.NewGuid().ToString();
      if (odsUserId == Guid.Empty)
      {
        if (!this.client.Context.Properties.ContainsKey("ODSUserId"))
          return;
        this.client.Context.Properties.Remove("ODSUserId");
      }
      else
      {
        if (this.client.Context.Properties.ContainsKey("ODSUserId") && !(this.client.Context.Properties["ODSUserId"] != odsUserId.ToString("D")))
          return;
        this.client.Context.Properties["ODSUserId"] = odsUserId.ToString("D");
      }
    }

    public void SetBandVersion(string bandVersion)
    {
      if (string.IsNullOrEmpty(bandVersion))
      {
        if (!this.client.Context.Properties.ContainsKey("BandVersion"))
          return;
        this.client.Context.Properties.Remove("BandVersion");
      }
      else
      {
        if (this.client.Context.Properties.ContainsKey("BandVersion") && this.client.Context.Properties["BandVersion"].Equals(bandVersion, StringComparison.CurrentCulture))
          return;
        this.client.Context.Properties["BandVersion"] = bandVersion;
      }
    }

    public void SetFirmwareVersion(string firmwareVersion)
    {
      if (string.IsNullOrEmpty(firmwareVersion))
      {
        if (!this.client.Context.Properties.ContainsKey("FirmwareVersion"))
          return;
        this.client.Context.Properties.Remove("FirmwareVersion");
      }
      else
      {
        if (this.client.Context.Properties.ContainsKey("FirmwareVersion") && this.client.Context.Properties["FirmwareVersion"].Equals(firmwareVersion, StringComparison.CurrentCulture))
          return;
        this.client.Context.Properties["FirmwareVersion"] = firmwareVersion;
      }
    }

    public void ResetSessionId()
    {
      this.client.Flush();
      this.client.Context.Session.Id = Guid.NewGuid().ToString();
    }

    public void LogEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      if (properties == null && metrics == null)
        this.client.TrackEvent(eventName, (IDictionary<string, string>) null, (IDictionary<string, double>) null);
      else
        this.client.TrackEvent(eventName, properties, metrics);
    }

    public void LogPageView(string pagePath) => this.client.TrackPageView(pagePath);

    public void LogException(
      Exception exception,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      this.client.TrackException(exception, properties, metrics);
    }

    public ITimedTelemetryEvent StartTimedEvent(
      string eventName,
      IDictionary<string, string> properties,
      IDictionary<string, double> metrics)
    {
      EventTelemetry eventTelemetry = new EventTelemetry(eventName);
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
      return (ITimedTelemetryEvent) new ApplicationInsightsTelemetryListener.TimedAnalyticsEvent(eventTelemetry, this.client);
    }

    public void Flush() => this.client.Flush();

    private class TimedAnalyticsEvent : ITimedTelemetryEvent, IDisposable
    {
      private EventTelemetry eventTelemetry;
      private TelemetryClient client;
      private Stopwatch stopwatch;
      private bool cancelled;
      private bool disposed;

      public TimedAnalyticsEvent(EventTelemetry eventTelemetry, TelemetryClient client)
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
          this.client.Track((ITelemetry) this.eventTelemetry);
        }
        this.disposed = true;
      }

      public void AddProperty(string key, string value) => this.eventTelemetry.Properties[key] = value;

      public void AddMetric(string key, double value) => this.eventTelemetry.Metrics[key] = value;
    }
  }
}
