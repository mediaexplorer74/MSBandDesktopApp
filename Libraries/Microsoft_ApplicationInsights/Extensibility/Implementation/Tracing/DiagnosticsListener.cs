// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsListener
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnosticsListener : EventListener
  {
    private const long AllKeyword = -1;
    private readonly IList<IDiagnosticsSender> diagnosticsSenders = (IList<IDiagnosticsSender>) new List<IDiagnosticsSender>();
    private HashSet<WeakReference> eventSources = new HashSet<WeakReference>();
    private EventLevel logLevel = EventLevel.Error;

    public DiagnosticsListener(IList<IDiagnosticsSender> senders) => this.diagnosticsSenders = senders != null && senders.Count >= 1 ? senders : throw new ArgumentNullException(nameof (senders));

    private DiagnosticsListener()
    {
    }

    public EventLevel LogLevel
    {
      get => this.logLevel;
      set
      {
        this.logLevel = value;
        HashSet<WeakReference> weakReferenceSet = new HashSet<WeakReference>();
        foreach (WeakReference eventSource in this.eventSources)
        {
          EventSource target = (EventSource) eventSource.Target;
          if (target != null)
          {
            this.EnableEvents(target, this.LogLevel, EventKeywords.All);
            weakReferenceSet.Add(eventSource);
          }
        }
        this.eventSources = weakReferenceSet;
      }
    }

    public void WriteEvent(TraceEvent eventData)
    {
      if (eventData.MetaData == null || eventData.MetaData.MessageFormat == null || eventData.MetaData.Level > this.LogLevel)
        return;
      foreach (IDiagnosticsSender diagnosticsSender in (IEnumerable<IDiagnosticsSender>) this.diagnosticsSenders)
        diagnosticsSender.Send(eventData);
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventSourceEvent)
    {
      EventMetaData eventMetaData = new EventMetaData()
      {
        Keywords = (long) eventSourceEvent.Keywords,
        MessageFormat = eventSourceEvent.Message,
        EventId = eventSourceEvent.EventId,
        Level = eventSourceEvent.Level
      };
      this.WriteEvent(new TraceEvent()
      {
        MetaData = eventMetaData,
        Payload = eventSourceEvent.Payload != null ? eventSourceEvent.Payload.ToArray<object>() : (object[]) null
      });
    }

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
      if (eventSource.Name.StartsWith("Microsoft-ApplicationInsights-", StringComparison.Ordinal))
      {
        this.eventSources.Add(new WeakReference((object) eventSource));
        this.EnableEvents(eventSource, this.LogLevel, EventKeywords.All);
      }
      base.OnEventSourceCreated(eventSource);
    }
  }
}
