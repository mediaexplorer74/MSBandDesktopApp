// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.DataPoint
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData]
  [GeneratedCode("gbc", "3.02")]
  internal class DataPoint
  {
    public string name { get; set; }

    public DataPointType kind { get; set; }

    public double value { get; set; }

    public int? count { get; set; }

    public double? min { get; set; }

    public double? max { get; set; }

    public double? stdDev { get; set; }

    public DataPoint()
      : this("AI.DataPoint", nameof (DataPoint))
    {
    }

    protected DataPoint(string fullName, string name)
    {
      this.name = string.Empty;
      this.kind = DataPointType.Measurement;
    }
  }
}
