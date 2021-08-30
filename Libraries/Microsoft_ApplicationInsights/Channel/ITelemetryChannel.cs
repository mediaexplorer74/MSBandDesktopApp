// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Channel.ITelemetryChannel
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;

namespace Microsoft.ApplicationInsights.Channel
{
  public interface ITelemetryChannel : IDisposable
  {
    bool DeveloperMode { get; set; }

    string EndpointAddress { get; set; }

    void Send(ITelemetry item);

    void Flush();
  }
}
