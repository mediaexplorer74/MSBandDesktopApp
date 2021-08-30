// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.RequestData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_RequestData")]
  [GeneratedCode("gbc", "3.02")]
  internal class RequestData
  {
    public int ver { get; set; }

    public string id { get; set; }

    public string name { get; set; }

    public string startTime { get; set; }

    public string duration { get; set; }

    public string responseCode { get; set; }

    public bool success { get; set; }

    public string httpMethod { get; set; }

    public string url { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public IDictionary<string, double> measurements { get; set; }

    public RequestData()
      : this("AI.RequestData", nameof (RequestData))
    {
    }

    protected RequestData(string fullName, string name)
    {
      this.ver = 2;
      this.id = string.Empty;
      this.name = string.Empty;
      this.startTime = string.Empty;
      this.duration = string.Empty;
      this.responseCode = string.Empty;
      this.httpMethod = string.Empty;
      this.url = string.Empty;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
      this.measurements = (IDictionary<string, double>) new Dictionary<string, double>();
    }
  }
}
