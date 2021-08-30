// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.SessionStateTelemetry
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.DataContracts
{
  public sealed class SessionStateTelemetry : ITelemetry
  {
    internal const string TelemetryName = "SessionState";
    internal readonly SessionStateData Data;
    private readonly TelemetryContext context;

    public SessionStateTelemetry()
    {
      this.Data = new SessionStateData();
      this.context = new TelemetryContext((IDictionary<string, string>) new Dictionary<string, string>(), (IDictionary<string, string>) new Dictionary<string, string>());
    }

    public SessionStateTelemetry(SessionState state)
      : this()
    {
      this.State = state;
    }

    public DateTimeOffset Timestamp { get; set; }

    public TelemetryContext Context => this.context;

    public string Sequence { get; set; }

    public SessionState State { get; set; }

    void ITelemetry.Sanitize()
    {
    }
  }
}
