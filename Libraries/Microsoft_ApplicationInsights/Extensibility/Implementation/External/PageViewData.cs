// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.PageViewData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_PageViewData")]
  [GeneratedCode("gbc", "3.02")]
  internal class PageViewData : EventData
  {
    public string url { get; set; }

    public string duration { get; set; }

    public PageViewData()
      : this("AI.PageViewData", nameof (PageViewData))
    {
    }

    protected PageViewData(string fullName, string name)
    {
      this.url = string.Empty;
      this.duration = string.Empty;
    }
  }
}
