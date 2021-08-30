// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.PageViewPerfData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_PageViewPerfData")]
  [GeneratedCode("gbc", "3.02")]
  internal class PageViewPerfData : PageViewData
  {
    public string perfTotal { get; set; }

    public string networkConnect { get; set; }

    public string sentRequest { get; set; }

    public string receivedResponse { get; set; }

    public string domProcessing { get; set; }

    public PageViewPerfData()
      : this("AI.PageViewPerfData", nameof (PageViewPerfData))
    {
    }

    protected PageViewPerfData(string fullName, string name)
    {
      this.perfTotal = string.Empty;
      this.networkConnect = string.Empty;
      this.sentRequest = string.Empty;
      this.receivedResponse = string.Empty;
      this.domProcessing = string.Empty;
    }
  }
}
