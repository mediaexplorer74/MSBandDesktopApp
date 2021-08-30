// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.PageViewTelemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.DataContracts
{
  public sealed class PageViewTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "PageView";
    internal readonly string BaseType = typeof (PageViewData).Name;
    internal readonly PageViewData Data;
    private readonly TelemetryContext context;

    public PageViewTelemetry()
    {
      this.Data = new PageViewData();
      this.context = new TelemetryContext(this.Data.properties, (IDictionary<string, string>) new Dictionary<string, string>());
    }

    public PageViewTelemetry(string pageName)
      : this()
    {
      this.Name = pageName;
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => this.context;

    public string Name
    {
      get => this.Data.name;
      set => this.Data.name = value;
    }

    public Uri Url
    {
      get => this.Data.url.IsNullOrWhiteSpace() ? (Uri) null : new Uri(this.Data.url);
      set
      {
        if (value == (Uri) null)
          this.Data.url = (string) null;
        else
          this.Data.url = value.ToString();
      }
    }

    public TimeSpan Duration
    {
      get => Utils.ValidateDuration(this.Data.duration);
      set => this.Data.duration = value.ToString();
    }

    public IDictionary<string, double> Metrics => this.Data.measurements;

    public IDictionary<string, string> Properties => this.Data.properties;

    void ITelemetry.Sanitize()
    {
      this.Name = this.Name.SanitizeName();
      this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof (PageViewTelemetry).FullName);
      this.Properties.SanitizeProperties();
      this.Metrics.SanitizeMeasurements();
      this.Url = this.Url.SanitizeUri();
    }
  }
}
