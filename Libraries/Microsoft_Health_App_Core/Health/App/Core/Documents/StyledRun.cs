// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Documents.StyledRun
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.Health.App.Core.Documents
{
  public class StyledRun : IMarkupString
  {
    public StyledRun(string text, StyledRunType runType)
    {
      Assert.ParamIsNotNullOrEmpty(text, nameof (text));
      Assert.EnumIsDefined<StyledRunType>(runType, nameof (runType));
      this.Text = text;
      this.RunType = runType;
    }

    public string Text { get; }

    public StyledRunType RunType { get; }

    public string ToMarkupString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<span type=\"{0}\">{1}</span>", new object[2]
    {
      (object) this.RunType,
      (object) this.Text
    });

    IEnumerable<StyledRun> IMarkupString.Runs => Enumerable.Repeat<StyledRun>(this, 1);

    string IMarkupString.ToMarkupString() => this.ToMarkupString();
  }
}
