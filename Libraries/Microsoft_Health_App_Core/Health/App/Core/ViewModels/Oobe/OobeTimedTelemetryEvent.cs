// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobeTimedTelemetryEvent
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  public sealed class OobeTimedTelemetryEvent
  {
    private readonly Func<ITimedTelemetryEvent> timedEventFactory;
    private ITimedTelemetryEvent timedEvent;

    public OobeTimedTelemetryEvent(Func<ITimedTelemetryEvent> timedEventFactory) => this.timedEventFactory = timedEventFactory != null ? timedEventFactory : throw new ArgumentNullException(nameof (timedEventFactory));

    public void StartTimer()
    {
      if (this.timedEvent != null)
        this.timedEvent.Dispose();
      this.timedEvent = this.timedEventFactory();
    }

    public void EndTimer(bool cancel = false)
    {
      if (this.timedEvent == null)
        return;
      if (!cancel)
        this.timedEvent.End();
      this.timedEvent.Dispose();
      this.timedEvent = (ITimedTelemetryEvent) null;
    }

    public void AddMetric(string key, double value)
    {
      if (this.timedEvent == null)
        return;
      this.timedEvent.AddMetric(key, value);
    }

    public void AddProperty(string key, string value)
    {
      if (this.timedEvent == null)
        return;
      this.timedEvent.AddProperty(key, value);
    }
  }
}
