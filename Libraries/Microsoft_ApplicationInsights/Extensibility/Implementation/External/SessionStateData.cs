// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.SessionStateData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_SessionStateData")]
  [GeneratedCode("gbc", "3.02")]
  internal class SessionStateData
  {
    public int ver { get; set; }

    public SessionState state { get; set; }

    public SessionStateData()
      : this("AI.SessionStateData", nameof (SessionStateData))
    {
    }

    protected SessionStateData(string fullName, string name)
    {
      this.ver = 2;
      this.state = SessionState.Start;
    }
  }
}
