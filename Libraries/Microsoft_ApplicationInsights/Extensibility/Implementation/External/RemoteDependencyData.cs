// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.RemoteDependencyData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_RemoteDependencyData")]
  [GeneratedCode("gbc", "3.02")]
  internal class RemoteDependencyData
  {
    public int ver { get; set; }

    public string name { get; set; }

    public DataPointType kind { get; set; }

    public double value { get; set; }

    public int? count { get; set; }

    public double? min { get; set; }

    public double? max { get; set; }

    public double? stdDev { get; set; }

    public DependencyKind dependencyKind { get; set; }

    public bool? success { get; set; }

    public bool? async { get; set; }

    public DependencySourceType dependencySource { get; set; }

    public string commandName { get; set; }

    public string dependencyTypeName { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public RemoteDependencyData()
      : this("AI.RemoteDependencyData", nameof (RemoteDependencyData))
    {
    }

    protected RemoteDependencyData(string fullName, string name)
    {
      this.ver = 2;
      this.name = string.Empty;
      this.kind = DataPointType.Measurement;
      this.dependencyKind = DependencyKind.Other;
      this.success = new bool?(true);
      this.dependencySource = DependencySourceType.Undefined;
      this.commandName = string.Empty;
      this.dependencyTypeName = string.Empty;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }
  }
}
