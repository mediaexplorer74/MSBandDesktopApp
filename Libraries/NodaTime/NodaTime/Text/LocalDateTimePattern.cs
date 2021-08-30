// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalDateTimePattern
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
  public sealed class LocalDateTimePattern : IPattern<LocalDateTime>
  {
    private const string DefaultFormatPattern = "G";
    internal static readonly LocalDateTime DefaultTemplateValue = new LocalDateTime(2000, 1, 1, 0, 0);
    internal static readonly PatternBclSupport<LocalDateTime> BclSupport = new PatternBclSupport<LocalDateTime>("G", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<LocalDateTime>>) (fi => fi.LocalDateTimePatternParser));
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<LocalDateTime> pattern;
    private readonly LocalDateTime templateValue;

    public static LocalDateTimePattern GeneralIsoPattern => LocalDateTimePattern.Patterns.GeneralIsoPatternImpl;

    public static LocalDateTimePattern ExtendedIsoPattern => LocalDateTimePattern.Patterns.ExtendedIsoPatternImpl;

    public static LocalDateTimePattern BclRoundtripPattern => LocalDateTimePattern.Patterns.BclRoundtripPatternImpl;

    public static LocalDateTimePattern FullRoundtripPattern => LocalDateTimePattern.Patterns.FullRoundtripPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    public LocalDateTime TemplateValue => this.templateValue;

    private LocalDateTimePattern(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalDateTime templateValue,
      IPattern<LocalDateTime> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
      this.templateValue = templateValue;
    }

    public ParseResult<LocalDateTime> Parse(string text) => this.pattern.Parse(text);

    public string Format(LocalDateTime value) => this.pattern.Format(value);

    private static LocalDateTimePattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      LocalDateTime templateValue)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<LocalDateTime> pattern = templateValue == LocalDateTimePattern.DefaultTemplateValue ? formatInfo.LocalDateTimePatternParser.ParsePattern(patternText) : new LocalDateTimePatternParser(templateValue).ParsePattern(patternText, formatInfo);
      return new LocalDateTimePattern(patternText, formatInfo, templateValue, pattern);
    }

    public static LocalDateTimePattern Create(
      string patternText,
      CultureInfo cultureInfo,
      LocalDateTime templateValue)
    {
      return LocalDateTimePattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo), templateValue);
    }

    public static LocalDateTimePattern Create(
      string patternText,
      CultureInfo cultureInfo)
    {
      return LocalDateTimePattern.Create(patternText, cultureInfo, LocalDateTimePattern.DefaultTemplateValue);
    }

    public static LocalDateTimePattern CreateWithCurrentCulture(
      string patternText)
    {
      return LocalDateTimePattern.Create(patternText, NodaFormatInfo.CurrentInfo, LocalDateTimePattern.DefaultTemplateValue);
    }

    public static LocalDateTimePattern CreateWithInvariantCulture(
      string patternText)
    {
      return LocalDateTimePattern.Create(patternText, NodaFormatInfo.InvariantInfo, LocalDateTimePattern.DefaultTemplateValue);
    }

    private LocalDateTimePattern WithFormatInfo(NodaFormatInfo newFormatInfo) => LocalDateTimePattern.Create(this.patternText, newFormatInfo, this.templateValue);

    public LocalDateTimePattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public LocalDateTimePattern WithTemplateValue(LocalDateTime newTemplateValue) => LocalDateTimePattern.Create(this.patternText, this.formatInfo, newTemplateValue);

    internal static class Patterns
    {
      internal static readonly LocalDateTimePattern GeneralIsoPatternImpl = LocalDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
      internal static readonly LocalDateTimePattern ExtendedIsoPatternImpl = LocalDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFF");
      internal static readonly LocalDateTimePattern BclRoundtripPatternImpl = LocalDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff");
      internal static readonly LocalDateTimePattern FullRoundtripPatternImpl = LocalDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff '('c')'");
    }
  }
}
