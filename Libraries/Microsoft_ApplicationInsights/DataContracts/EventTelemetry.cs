// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.EventTelemetry
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
  public sealed class EventTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "Event";
    internal readonly string BaseType = typeof (EventData).Name;
    internal readonly EventData Data;
    private readonly TelemetryContext context;

    public EventTelemetry()
    {
      this.Data = new EventData();
      this.context = new TelemetryContext(this.Data.properties, (IDictionary<string, string>) new Dictionary<string, string>());
    }

    public EventTelemetry(string name)
      : this()
    {
      this.Name = name;
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => this.context;

    public string Name
    {
      get => this.Data.name;
      set => this.Data.name = value;
    }

    public IDictionary<string, double> Metrics => this.Data.measurements;

    public IDictionary<string, string> Properties => this.Data.properties;

    void ITelemetry.Sanitize()
    {
      this.Name = this.Name.SanitizeName();
      this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof (EventTelemetry).FullName);
      this.Properties.SanitizeProperties();
      this.Metrics.SanitizeMeasurements();
    }
  }
}
