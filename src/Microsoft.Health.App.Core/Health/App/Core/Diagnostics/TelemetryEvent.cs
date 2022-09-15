// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.TelemetryEvent
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public class TelemetryEvent
  {
    public TelemetryEvent(string eventName)
    {
      this.Name = eventName;
      this.Metrics = (IDictionary<string, double>) new Dictionary<string, double>();
      this.Properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }

    public string Name { get; set; }

    public IDictionary<string, double> Metrics { get; set; }

    public IDictionary<string, string> Properties { get; set; }

    public string Sequence { get; set; }

    public DateTimeOffset Timestamp { get; set; }
  }
}
