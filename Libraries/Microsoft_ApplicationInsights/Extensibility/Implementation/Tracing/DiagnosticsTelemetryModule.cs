// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsTelemetryModule
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnosticsTelemetryModule : ISupportConfiguration, IDisposable
  {
    protected readonly IList<IDiagnosticsSender> Senders = (IList<IDiagnosticsSender>) new List<IDiagnosticsSender>();
    protected DiagnosticsListener eventListener;
    private readonly IDiagnoisticsEventThrottlingScheduler throttlingScheduler = (IDiagnoisticsEventThrottlingScheduler) new DiagnoisticsEventThrottlingScheduler();
    private volatile bool disposed;
    private string instrumentationKey;

    public DiagnosticsTelemetryModule()
    {
      this.Senders.Add((IDiagnosticsSender) new PortalDiagnosticsQueueSender());
      this.Senders.Add((IDiagnosticsSender) new F5DiagnosticsSender());
      this.eventListener = new DiagnosticsListener(this.Senders);
    }

    ~DiagnosticsTelemetryModule() => this.Dispose(false);

    public string Severity
    {
      get => this.eventListener.LogLevel.ToString();
      set
      {
        if (string.IsNullOrEmpty(value) || !Enum.IsDefined(typeof (EventLevel), (object) value))
          return;
        this.eventListener.LogLevel = (EventLevel) Enum.Parse(typeof (EventLevel), value, true);
      }
    }

    public string DiagnosticsInstrumentationKey
    {
      get => this.instrumentationKey;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.instrumentationKey = value;
        foreach (PortalDiagnosticsSender diagnosticsSender in Enumerable.OfType<PortalDiagnosticsSender>(this.Senders))
          diagnosticsSender.DiagnosticsInstrumentationKey = this.instrumentationKey;
      }
    }

    public void Initialize(TelemetryConfiguration configuration)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      PortalDiagnosticsQueueSender diagnosticsQueueSender = Enumerable.OfType<PortalDiagnosticsQueueSender>(this.Senders).FirstOrDefault<PortalDiagnosticsQueueSender>();
      if (diagnosticsQueueSender != null)
        this.Senders.Remove((IDiagnosticsSender) diagnosticsQueueSender);
      PortalDiagnosticsSender diagnosticsSender = new PortalDiagnosticsSender(configuration, (IDiagnoisticsEventThrottlingManager) new DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottling>(new DiagnoisticsEventThrottling(5), this.throttlingScheduler, 5U));
      diagnosticsSender.DiagnosticsInstrumentationKey = this.DiagnosticsInstrumentationKey;
      this.Senders.Add((IDiagnosticsSender) diagnosticsSender);
      foreach (TraceEvent eventData in (IEnumerable<TraceEvent>) diagnosticsQueueSender.EventData)
        diagnosticsSender.Send(eventData);
    }

    public void Dispose() => this.Dispose(true);

    private void Dispose(bool managed)
    {
      if (managed && !this.disposed)
      {
        this.eventListener.Dispose();
        (this.throttlingScheduler as IDisposable).Dispose();
        GC.SuppressFinalize((object) this);
      }
      this.disposed = true;
    }
  }
}
