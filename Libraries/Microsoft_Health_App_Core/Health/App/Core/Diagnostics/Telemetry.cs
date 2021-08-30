// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.Telemetry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class Telemetry
  {
    private static ITelemetryListener[] listeners = new ITelemetryListener[0];
    private static Guid currentUserId = Guid.Empty;
    private static string bandVersion = string.Empty;
    private static string firmwareVersion = string.Empty;
    private static IConfigProvider configProvider;

    public static void AddListener(ITelemetryListener listener)
    {
      Assert.ParamIsNotNull((object) listener, nameof (listener));
      List<ITelemetryListener> list = ((IEnumerable<ITelemetryListener>) Telemetry.listeners).ToList<ITelemetryListener>();
      listener.SetOdsUserId(Telemetry.currentUserId);
      listener.SetFirmwareVersion(Telemetry.firmwareVersion);
      listener.SetBandVersion(Telemetry.bandVersion);
      list.Add(listener);
      Telemetry.listeners = list.ToArray();
    }

    public static void RemoveListener(ITelemetryListener listener)
    {
      Assert.ParamIsNotNull((object) listener, nameof (listener));
      List<ITelemetryListener> list = ((IEnumerable<ITelemetryListener>) Telemetry.listeners).ToList<ITelemetryListener>();
      list.Remove(listener);
      Telemetry.listeners = list.ToArray();
    }

    public static void SetOdsUserId(Guid odsUserId)
    {
      Telemetry.currentUserId = odsUserId;
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.SetOdsUserId(odsUserId)));
    }

    public static void SetBandVersion(string bandVersion)
    {
      if (bandVersion == null)
        return;
      Telemetry.bandVersion = bandVersion;
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.SetBandVersion(Telemetry.bandVersion)));
    }

    public static void SetFirmwareVersion(string firmwareVersion)
    {
      if (firmwareVersion == null)
        return;
      Telemetry.firmwareVersion = firmwareVersion;
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.SetFirmwareVersion(Telemetry.firmwareVersion)));
    }

    public static bool IsFirmwareVersionSet() => !string.IsNullOrEmpty(Telemetry.firmwareVersion);

    public static void LogEvent(
      string eventName,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      Assert.ParamIsNotNull((object) eventName, nameof (eventName));
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.LogEvent(eventName, properties, metrics)));
    }

    public static void LogPageView(string pagePath)
    {
      Assert.ParamIsNotNull((object) pagePath, nameof (pagePath));
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.LogPageView(pagePath)));
    }

    public static void LogException(
      Exception exception,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      Assert.ParamIsNotNull((object) exception, nameof (exception));
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => l.LogException(exception, properties, metrics)));
    }

    public static ITimedTelemetryEvent StartTimedEvent(
      string eventName,
      IDictionary<string, string> properties = null,
      IDictionary<string, double> metrics = null)
    {
      Assert.ParamIsNotNull((object) eventName, nameof (eventName));
      List<ITimedTelemetryEvent> targets = new List<ITimedTelemetryEvent>(Telemetry.listeners.Length);
      Telemetry.ForEachListener((Action<ITelemetryListener>) (l => targets.Add(l.StartTimedEvent(eventName, properties, metrics))));
      return (ITimedTelemetryEvent) new Telemetry.CompositeTimedTelemetryEvent((ICollection<ITimedTelemetryEvent>) targets);
    }

    private static void ForEachListener(Action<ITelemetryListener> action)
    {
      ITelemetryListener[] listeners = Telemetry.listeners;
      if (listeners.Length == 0)
        return;
      foreach (ITelemetryListener telemetryListener in listeners)
        action(telemetryListener);
    }

    private class CompositeTimedTelemetryEvent : ITimedTelemetryEvent, IDisposable
    {
      private readonly ICollection<ITimedTelemetryEvent> targets;

      public CompositeTimedTelemetryEvent(ICollection<ITimedTelemetryEvent> targets) => this.targets = targets;

      public void Cancel()
      {
        foreach (ITimedTelemetryEvent target in (IEnumerable<ITimedTelemetryEvent>) this.targets)
          target?.Cancel();
      }

      public void End()
      {
        foreach (ITimedTelemetryEvent target in (IEnumerable<ITimedTelemetryEvent>) this.targets)
          target?.End();
      }

      public void Dispose()
      {
        foreach (ITimedTelemetryEvent target in (IEnumerable<ITimedTelemetryEvent>) this.targets)
          target?.Dispose();
      }

      public void AddProperty(string key, string value)
      {
        foreach (ITimedTelemetryEvent target in (IEnumerable<ITimedTelemetryEvent>) this.targets)
          target?.AddProperty(key, value);
      }

      public void AddMetric(string key, double value)
      {
        foreach (ITimedTelemetryEvent target in (IEnumerable<ITimedTelemetryEvent>) this.targets)
          target?.AddMetric(key, value);
      }
    }
  }
}
