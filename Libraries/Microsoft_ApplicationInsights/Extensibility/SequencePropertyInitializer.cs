// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.SequencePropertyInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using System;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public sealed class SequencePropertyInitializer : ITelemetryInitializer
  {
    private readonly string stablePrefix = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=') + ":";
    private long currentNumber;

    public void Initialize(ITelemetry telemetry)
    {
      if (!string.IsNullOrEmpty(telemetry.Sequence))
        return;
      telemetry.Sequence = this.stablePrefix + (object) Interlocked.Increment(ref this.currentNumber);
    }
  }
}
