// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public sealed class TelemetryConfiguration : IDisposable
  {
    private static object syncRoot = new object();
    private static TelemetryConfiguration active;
    private readonly SnapshottingList<IContextInitializer> contextInitializers = new SnapshottingList<IContextInitializer>();
    private readonly SnapshottingList<ITelemetryInitializer> telemetryInitializers = new SnapshottingList<ITelemetryInitializer>();
    private readonly SnapshottingList<object> telemetryModules = new SnapshottingList<object>();
    private string instrumentationKey = string.Empty;
    private bool disableTelemetry;

    public static TelemetryConfiguration Active
    {
      get
      {
        if (TelemetryConfiguration.active == null)
        {
          lock (TelemetryConfiguration.syncRoot)
          {
            if (TelemetryConfiguration.active == null)
            {
              TelemetryConfiguration.active = new TelemetryConfiguration();
              TelemetryConfigurationFactory.Instance.Initialize(TelemetryConfiguration.active);
            }
          }
        }
        return TelemetryConfiguration.active;
      }
      internal set
      {
        lock (TelemetryConfiguration.syncRoot)
          TelemetryConfiguration.active = value;
      }
    }

    public string InstrumentationKey
    {
      get => this.instrumentationKey;
      set => this.instrumentationKey = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public bool DisableTelemetry
    {
      get => this.disableTelemetry;
      set
      {
        if (value)
          CoreEventSource.Log.TrackingWasDisabled();
        else
          CoreEventSource.Log.TrackingWasEnabled();
        this.disableTelemetry = value;
      }
    }

    public IList<IContextInitializer> ContextInitializers => (IList<IContextInitializer>) this.contextInitializers;

    public IList<ITelemetryInitializer> TelemetryInitializers => (IList<ITelemetryInitializer>) this.telemetryInitializers;

    public IList<object> TelemetryModules => (IList<object>) this.telemetryModules;

    public ITelemetryChannel TelemetryChannel { get; set; }

    public static TelemetryConfiguration CreateDefault()
    {
      TelemetryConfiguration configuration = new TelemetryConfiguration();
      TelemetryConfigurationFactory.Instance.Initialize(configuration);
      return configuration;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      Interlocked.CompareExchange<TelemetryConfiguration>(ref TelemetryConfiguration.active, (TelemetryConfiguration) null, this);
      this.TelemetryChannel?.Dispose();
    }
  }
}
