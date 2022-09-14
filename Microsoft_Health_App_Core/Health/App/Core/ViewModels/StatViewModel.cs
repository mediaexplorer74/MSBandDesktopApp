// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.StatViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class StatViewModel
  {
    public string Label { get; set; }

    public string Glyph { get; set; }

    public string GlyphPrefix { get; set; }

    public object Value { get; set; }

    public StatValueType ValueType { get; set; }

    public bool ShowNotAvailableOnZero { get; set; } = true;

    public SubStatViewModel SubStat1 { get; set; }

    public SubStatViewModel SubStat2 { get; set; }
  }
}
