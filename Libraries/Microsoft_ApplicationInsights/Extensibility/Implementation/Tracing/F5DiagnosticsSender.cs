// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.F5DiagnosticsSender
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
using System;
using System.Globalization;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class F5DiagnosticsSender : IDiagnosticsSender
  {
    protected IDebugOutput debugOutput;

    public F5DiagnosticsSender() => this.debugOutput = PlatformSingleton.Current.GetDebugOutput();

    public void Send(TraceEvent eventData)
    {
      if (eventData.MetaData == null || string.IsNullOrEmpty(eventData.MetaData.MessageFormat))
        return;
      this.debugOutput.WriteLine(eventData.Payload == null || eventData.Payload.Length <= 0 ? eventData.MetaData.MessageFormat : string.Format((IFormatProvider) CultureInfo.InvariantCulture, eventData.MetaData.MessageFormat, eventData.Payload));
    }
  }
}
