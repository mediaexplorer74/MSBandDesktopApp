// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.CloudTimedTelemetryEvent
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client.Logging;
using System;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public class CloudTimedTelemetryEvent : ICloudTimedTelemetryEvent, IDisposable
  {
    private readonly ITimedTelemetryEvent timedEvent;

    public CloudTimedTelemetryEvent(ITimedTelemetryEvent timedEvent) => this.timedEvent = timedEvent;

    public void Dispose() => this.timedEvent.Dispose();

    public void AddProperty(string key, string value) => this.timedEvent.AddProperty(key, value);

    public void AddMetric(string key, double value) => this.timedEvent.AddMetric(key, value);
  }
}
