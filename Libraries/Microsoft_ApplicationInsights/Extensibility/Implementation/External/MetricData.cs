// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.MetricData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_MetricData")]
  [GeneratedCode("gbc", "3.02")]
  internal class MetricData
  {
    public int ver { get; set; }

    public IList<DataPoint> metrics { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public MetricData()
      : this("AI.MetricData", nameof (MetricData))
    {
    }

    protected MetricData(string fullName, string name)
    {
      this.ver = 2;
      this.metrics = (IList<DataPoint>) new List<DataPoint>();
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }
  }
}
