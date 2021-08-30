// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.PerformanceCounterData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [GeneratedCode("gbc", "3.02")]
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_PerformanceCounterData")]
  internal class PerformanceCounterData
  {
    public int ver { get; set; }

    public string categoryName { get; set; }

    public string counterName { get; set; }

    public string instanceName { get; set; }

    public DataPointType kind { get; set; }

    public int? count { get; set; }

    public double? min { get; set; }

    public double? max { get; set; }

    public double? stdDev { get; set; }

    public double value { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public PerformanceCounterData()
      : this("AI.PerformanceCounterData", nameof (PerformanceCounterData))
    {
    }

    protected PerformanceCounterData(string fullName, string name)
    {
      this.ver = 2;
      this.categoryName = string.Empty;
      this.counterName = string.Empty;
      this.instanceName = string.Empty;
      this.kind = DataPointType.Aggregation;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }
  }
}
