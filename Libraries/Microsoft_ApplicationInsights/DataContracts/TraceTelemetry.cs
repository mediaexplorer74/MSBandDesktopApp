// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.TraceTelemetry
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
  public sealed class TraceTelemetry : ITelemetry, ISupportProperties
  {
    internal const string TelemetryName = "Message";
    internal readonly string BaseType = typeof (MessageData).Name;
    internal readonly MessageData Data;
    private readonly TelemetryContext context;

    public TraceTelemetry()
    {
      this.Data = new MessageData();
      this.context = new TelemetryContext(this.Data.properties, (IDictionary<string, string>) new Dictionary<string, string>());
    }

    public TraceTelemetry(string message)
      : this()
    {
      this.Message = message;
    }

    public TraceTelemetry(string message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel severityLevel)
      : this(message)
    {
      this.SeverityLevel = new Microsoft.ApplicationInsights.DataContracts.SeverityLevel?(severityLevel);
    }

    public DateTimeOffset Timestamp { get; set; }

    public string Sequence { get; set; }

    public TelemetryContext Context => this.context;

    public string Message
    {
      get => this.Data.message;
      set => this.Data.message = value;
    }

    public Microsoft.ApplicationInsights.DataContracts.SeverityLevel? SeverityLevel
    {
      get => this.Data.severityLevel.TranslateSeverityLevel();
      set => this.Data.severityLevel = value.TranslateSeverityLevel();
    }

    public IDictionary<string, string> Properties => this.Data.properties;

    void ITelemetry.Sanitize()
    {
      this.Data.message = this.Data.message.SanitizeMessage();
      this.Data.message = Utils.PopulateRequiredStringValue(this.Data.message, "message", typeof (TraceTelemetry).FullName);
      this.Data.properties.SanitizeProperties();
    }
  }
}
