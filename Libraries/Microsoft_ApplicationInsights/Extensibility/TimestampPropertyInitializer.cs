// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.TimestampPropertyInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public sealed class TimestampPropertyInitializer : ITelemetryInitializer
  {
    public void Initialize(ITelemetry telemetry)
    {
      if (!(telemetry.Timestamp == new DateTimeOffset()))
        return;
      telemetry.Timestamp = Clock.Instance.Time;
    }
  }
}
