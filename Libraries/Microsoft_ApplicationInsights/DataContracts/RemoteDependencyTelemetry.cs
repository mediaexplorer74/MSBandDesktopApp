// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.RemoteDependencyTelemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.ApplicationInsights.DataContracts
{
  [DebuggerDisplay("Value={Value}; Name={Name}; Count={Count}; Success={Success}; Async={Async}; Timestamp={Timestamp}")]
  internal sealed class RemoteDependencyTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "RemoteDependency";
    internal readonly string BaseType = typeof (RemoteDependencyData).Name;
    internal readonly RemoteDependencyData Data;
    private readonly TelemetryContext context;

    public RemoteDependencyTelemetry()
    {
      this.Data = new RemoteDependencyData()
      {
        kind = DataPointType.Aggregation
      };
      this.context = new TelemetryContext(this.Data.properties, (IDictionary<string, string>) new Dictionary<string, string>());
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => this.context;

    public string Name
    {
      get => this.Data.name;
      set => this.Data.name = value;
    }

    public string CommandName
    {
      get => this.Data.commandName;
      set => this.Data.commandName = value;
    }

    public DependencyKind DependencyKind
    {
      get => this.Data.dependencyKind;
      set => this.Data.dependencyKind = value;
    }

    public double Value
    {
      get => this.Data.value;
      set => this.Data.value = value;
    }

    public int? Count
    {
      get => this.Data.count;
      set => this.Data.count = value;
    }

    public bool? Success
    {
      get => this.Data.success;
      set => this.Data.success = value;
    }

    public bool? Async
    {
      get => this.Data.async;
      set => this.Data.async = value;
    }

    public DependencySourceType DependencySource
    {
      get => this.Data.dependencySource;
      set => this.Data.dependencySource = value;
    }

    public IDictionary<string, string> Properties => this.Data.properties;

    void ITelemetry.Sanitize()
    {
      this.Name = this.Name.SanitizeName();
      this.Properties.SanitizeProperties();
    }
  }
}
