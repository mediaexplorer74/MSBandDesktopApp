// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.MessageData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_MessageData")]
  [GeneratedCode("gbc", "3.02")]
  internal class MessageData
  {
    public int ver { get; set; }

    public string message { get; set; }

    public SeverityLevel? severityLevel { get; set; }

    public IDictionary<string, string> properties { get; set; }

    public MessageData()
      : this("AI.MessageData", nameof (MessageData))
    {
    }

    protected MessageData(string fullName, string name)
    {
      this.ver = 2;
      this.message = string.Empty;
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>();
    }
  }
}
