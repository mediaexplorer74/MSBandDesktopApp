// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Documents.StyledSpanParser
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Health.App.Core.Documents
{
  internal class StyledSpanParser : IFormatProvider, ICustomFormatter
  {
    private List<StyledRun> placeholderRuns;
    private string formattedString;

    public StyledSpanParser(string format, params object[] args)
    {
      Assert.ParamIsNotNull((object) format, nameof (format));
      this.placeholderRuns = new List<StyledRun>();
      this.formattedString = string.Format((IFormatProvider) this, format, args);
    }

    public IEnumerable<StyledRun> Parse()
    {
      List<StyledRun> styledRunList = new List<StyledRun>();
      int startIndex = 0;
      foreach (StyledRun placeholderRun in this.placeholderRuns)
      {
        string markupString = placeholderRun.ToMarkupString();
        int endIndex = this.formattedString.IndexOf(markupString, startIndex);
        if (endIndex != startIndex)
          styledRunList.Add(this.ParseNoneStyledRun(startIndex, endIndex));
        styledRunList.Add(placeholderRun);
        startIndex = endIndex + markupString.Length;
      }
      if (startIndex != this.formattedString.Length)
        styledRunList.Add(this.ParseNoneStyledRun(startIndex, this.formattedString.Length));
      return (IEnumerable<StyledRun>) styledRunList;
    }

    object IFormatProvider.GetFormat(Type formatType) => this.GetFormat(formatType);

    string ICustomFormatter.Format(
      string format,
      object arg,
      IFormatProvider formatProvider)
    {
      return this.Format(format, arg, formatProvider);
    }

    protected virtual object GetFormat(Type formatType) => (object) formatType == (object) typeof (ICustomFormatter) ? (object) this : (object) null;

    protected virtual string Format(string format, object arg, IFormatProvider formatProvider)
    {
      Assert.ParamIsNotNullOrEmpty(format, nameof (format));
      if (arg is IMarkupString markupString)
      {
        this.placeholderRuns.AddRange(markupString.Runs);
        return markupString.ToMarkupString();
      }
      string text = arg?.ToString();
      if (string.IsNullOrEmpty(text))
        return string.Empty;
      StyledRunType result;
      if (!Enum.TryParse<StyledRunType>(format, out result))
        throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The run type '{0}' is not supported.", new object[1]
        {
          (object) format
        }));
      StyledRun styledRun = new StyledRun(text, result);
      this.placeholderRuns.Add(styledRun);
      return styledRun.ToMarkupString();
    }

    private StyledRun ParseNoneStyledRun(int startIndex, int endIndex) => new StyledRun(this.formattedString.Substring(startIndex, endIndex - startIndex), StyledRunType.None);
  }
}
