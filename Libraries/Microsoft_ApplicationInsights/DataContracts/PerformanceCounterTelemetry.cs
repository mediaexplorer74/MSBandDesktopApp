// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.PerformanceCounterTelemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.ApplicationInsights.DataContracts
{
  [DebuggerDisplay("CategoryName={CategoryName}; CounterName={CounterName}; InstanceName={InstanceName}; Value={Value}; Timestamp={Timestamp}")]
  internal class PerformanceCounterTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "PerformanceCounter";
    internal readonly string BaseType = typeof (PerformanceCounterData).Name;
    internal readonly PerformanceCounterData Data;
    private TelemetryContext context;

    public PerformanceCounterTelemetry() => this.Data = new PerformanceCounterData();

    public PerformanceCounterTelemetry(
      string categoryName,
      string counterName,
      string instanceName,
      double value)
      : this()
    {
      this.CategoryName = categoryName;
      this.CounterName = counterName;
      this.InstanceName = instanceName;
      this.Value = value;
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => LazyInitializer.EnsureInitialized<TelemetryContext>(ref this.context);

    public double Value
    {
      get => this.Data.value;
      set => this.Data.value = value;
    }

    public string CategoryName
    {
      get => this.Data.categoryName;
      set => this.Data.categoryName = value;
    }

    public string CounterName
    {
      get => this.Data.counterName;
      set => this.Data.counterName = value;
    }

    public string InstanceName
    {
      get => this.Data.instanceName;
      set => this.Data.instanceName = value;
    }

    public IDictionary<string, string> Properties => this.Data.properties;

    void ITelemetry.Sanitize()
    {
    }
  }
}
