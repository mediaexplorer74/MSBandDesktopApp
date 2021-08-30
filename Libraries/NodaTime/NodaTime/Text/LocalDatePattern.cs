// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalDatePattern
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Globalization;
using NodaTime.Text.Patterns;
using NodaTime.Utility;
using System;
using System.Globalization;

namespace NodaTime.Text
{
  [Immutable]
  public sealed class LocalDatePattern : IPattern<LocalDate>
  {
    private const string DefaultFormatPattern = "D";
    internal static readonly LocalDate DefaultTemplateValue = new LocalDate(2000, 1, 1);
    internal static readonly PatternBclSupport<LocalDate> BclSupport = new PatternBclSupport<LocalDate>("D", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<LocalDate>>) (fi => fi.LocalDatePatternParser));
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<LocalDate> pattern;
    private readonly LocalDate templateValue;

    public static LocalDatePattern IsoPattern => LocalDatePattern.Patterns.IsoPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    public LocalDate TemplateValue => this.templateValue;

    private LocalDatePattern(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalDate templateValue,
      IPattern<LocalDate> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
      this.templateValue = templateValue;
    }

    public ParseResult<LocalDate> Parse(string text) => this.pattern.Parse(text);

    public string Format(LocalDate value) => this.pattern.Format(value);

    private static LocalDatePattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalDate templateValue)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<LocalDate> pattern = templateValue == LocalDatePattern.DefaultTemplateValue ? formatInfo.LocalDatePatternParser.ParsePattern(patternText) : new LocalDatePatternParser(templateValue).ParsePattern(patternText, formatInfo);
      return new LocalDatePattern(patternText, formatInfo, templateValue, pattern);
    }

    public static LocalDatePattern Create(
      string patternText,
      CultureInfo cultureInfo,
      LocalDate templateValue)
    {
      return LocalDatePattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo), templateValue);
    }

    public static LocalDatePattern Create(
      string patternText,
      CultureInfo cultureInfo)
    {
      return LocalDatePattern.Create(patternText, cultureInfo, LocalDatePattern.DefaultTemplateValue);
    }

    public static LocalDatePattern CreateWithCurrentCulture(string patternText) => LocalDatePattern.Create(patternText, NodaFormatInfo.CurrentInfo, LocalDatePattern.DefaultTemplateValue);

    public static LocalDatePattern CreateWithInvariantCulture(string patternText) => LocalDatePattern.Create(patternText, NodaFormatInfo.InvariantInfo, LocalDatePattern.DefaultTemplateValue);

    private LocalDatePattern WithFormatInfo(NodaFormatInfo newFormatInfo) => LocalDatePattern.Create(this.patternText, newFormatInfo, this.templateValue);

    public LocalDatePattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public LocalDatePattern WithTemplateValue(LocalDate newTemplateValue) => LocalDatePattern.Create(this.patternText, this.formatInfo, newTemplateValue);

    private static class Patterns
    {
      internal static readonly LocalDatePattern IsoPatternImpl = LocalDatePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd");
    }
  }
}
