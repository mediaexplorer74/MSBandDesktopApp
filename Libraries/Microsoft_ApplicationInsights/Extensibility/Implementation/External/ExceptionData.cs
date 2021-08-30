// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.ExceptionData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [GeneratedCode("gbc", "3.02")]
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_ExceptionData")]
  internal class ExceptionData
  {
    public int ver { get; set; }

    public string handledAt { get; set; }

    public IList<ExceptionDetails> exceptions { get; set; }

    public SeverityLevel? severityLevel { get; set; }

    public string problemId { get; set; }

    public int crashThreadId { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public IDictionary<string, double> measurements { get; set; }

    public ExceptionData()
      : this("AI.ExceptionData", nameof (ExceptionData))
    {
    }

    protected ExceptionData(string fullName, string name)
    {
      this.ver = 2;
      this.handledAt = string.Empty;
      this.exceptions = (IList<ExceptionDetails>) new List<ExceptionDetails>();
      this.problemId = string.Empty;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
      this.measurements = (IDictionary<string, double>) new Dictionary<string, double>();
    }
  }
}
