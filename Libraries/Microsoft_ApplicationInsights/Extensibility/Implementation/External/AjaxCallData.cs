// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.AjaxCallData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.CodeDom.Compiler;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [GeneratedCode("gbc", "3.02")]
  [Microsoft.Diagnostics.Tracing.EventData(Name = "PartB_AjaxCallData")]
  internal class AjaxCallData : PageViewData
  {
    public string ajaxUrl { get; set; }

    public double requestSize { get; set; }

    public double responseSize { get; set; }

    public string timeToFirstByte { get; set; }

    public string timeToLastByte { get; set; }

    public string callbackDuration { get; set; }

    public string responseCode { get; set; }

    public bool success { get; set; }

    public AjaxCallData()
      : this("AI.AjaxCallData", nameof (AjaxCallData))
    {
    }

    protected AjaxCallData(string fullName, string name)
    {
      this.ajaxUrl = string.Empty;
      this.timeToFirstByte = string.Empty;
      this.timeToLastByte = string.Empty;
      this.callbackDuration = string.Empty;
      this.responseCode = string.Empty;
    }
  }
}
