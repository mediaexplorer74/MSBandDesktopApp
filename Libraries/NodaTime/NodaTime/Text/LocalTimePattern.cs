// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalTimePattern
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
  public sealed class LocalTimePattern : IPattern<LocalTime>
  {
    private const string DefaultFormatPattern = "T";
    internal static readonly PatternBclSupport<LocalTime> BclSupport = new PatternBclSupport<LocalTime>("T", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<LocalTime>>) (fi => fi.LocalTimePatternParser));
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<LocalTime> pattern;
    private readonly LocalTime templateValue;

    public static LocalTimePattern ExtendedIsoPattern => LocalTimePattern.Patterns.ExtendedIsoPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    public LocalTime TemplateValue => this.templateValue;

    private LocalTimePattern(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalTime templateValue,
      IPattern<LocalTime> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
      this.templateValue = templateValue;
    }

    public ParseResult<LocalTime> Parse(string text) => this.pattern.Parse(text);

    public string Format(LocalTime value) => this.pattern.Format(value);

    private static LocalTimePattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalTime templateValue)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<LocalTime> pattern = templateValue == LocalTime.Midnight ? formatInfo.LocalTimePatternParser.ParsePattern(patternText) : new LocalTimePatternParser(templateValue).ParsePattern(patternText, formatInfo);
      return new LocalTimePattern(patternText, formatInfo, templateValue, pattern);
    }

    public static LocalTimePattern Create(
      string patternText,
      CultureInfo cultureInfo,
      LocalTime templateValue)
    {
      return LocalTimePattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo), templateValue);
    }

    public static LocalTimePattern Create(
      string patternText,
      CultureInfo cultureInfo)
    {
      return LocalTimePattern.Create(patternText, cultureInfo, LocalTime.Midnight);
    }

    public static LocalTimePattern CreateWithCurrentCulture(string patternText) => LocalTimePattern.Create(patternText, NodaFormatInfo.CurrentInfo, LocalTime.Midnight);

    public static LocalTimePattern CreateWithInvariantCulture(string patternText) => LocalTimePattern.Create(patternText, NodaFormatInfo.InvariantInfo, LocalTime.Midnight);

    private LocalTimePattern WithFormatInfo(NodaFormatInfo newFormatInfo) => LocalTimePattern.Create(this.patternText, newFormatInfo, this.templateValue);

    public LocalTimePattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public LocalTimePattern WithTemplateValue(LocalTime newTemplateValue) => LocalTimePattern.Create(this.patternText, this.formatInfo, newTemplateValue);

    private static class Patterns
    {
      internal static readonly LocalTimePattern ExtendedIsoPatternImpl = LocalTimePattern.CreateWithInvariantCulture("HH':'mm':'ss;FFFFFFF");
    }
  }
}
