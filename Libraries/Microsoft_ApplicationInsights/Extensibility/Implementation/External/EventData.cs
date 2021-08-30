// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.EventData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_EventData")]
  [GeneratedCode("gbc", "3.02")]
  internal class EventData
  {
    public int ver { get; set; }

    public string name { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public IDictionary<string, double> measurements { get; set; }

    public EventData()
      : this("AI.EventData", nameof (EventData))
    {
    }

    protected EventData(string fullName, string name)
    {
      this.ver = 2;
      this.name = string.Empty;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
      this.measurements = (IDictionary<string, double>) new Dictionary<string, double>();
    }
  }
}
