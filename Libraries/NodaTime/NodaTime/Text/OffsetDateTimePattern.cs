// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.OffsetDateTimePattern
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
  public sealed class OffsetDateTimePattern : IPattern<OffsetDateTime>
  {
    internal static readonly OffsetDateTime DefaultTemplateValue = new LocalDateTime(2000, 1, 1, 0, 0).WithOffset(Offset.Zero);
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<OffsetDateTime> pattern;
    private readonly OffsetDateTime templateValue;

    public static OffsetDateTimePattern GeneralIsoPattern => OffsetDateTimePattern.Patterns.GeneralIsoPatternImpl;

    public static OffsetDateTimePattern ExtendedIsoPattern => OffsetDateTimePattern.Patterns.ExtendedIsoPatternImpl;

    public static OffsetDateTimePattern Rfc3339Pattern => OffsetDateTimePattern.Patterns.Rfc3339PatternImpl;

    public static OffsetDateTimePattern FullRoundtripPattern => OffsetDateTimePattern.Patterns.FullRoundtripPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    public OffsetDateTime TemplateValue => this.templateValue;

    private OffsetDateTimePattern(
      string patternText,
      NodaFormatInfo formatInfo,
      OffsetDateTime templateValue,
      IPattern<OffsetDateTime> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.templateValue = templateValue;
      this.pattern = pattern;
    }

    public ParseResult<OffsetDateTime> Parse(string text) => this.pattern.Parse(text);

    public string Format(OffsetDateTime value) => this.pattern.Format(value);

    private static OffsetDateTimePattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      OffsetDateTime templateValue)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<OffsetDateTime> pattern = new OffsetDateTimePatternParser(templateValue).ParsePattern(patternText, formatInfo);
      return new OffsetDateTimePattern(patternText, formatInfo, templateValue, pattern);
    }

    public static OffsetDateTimePattern Create(
      string patternText,
      CultureInfo cultureInfo,
      OffsetDateTime templateValue)
    {
      return OffsetDateTimePattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo), templateValue);
    }

    public static OffsetDateTimePattern CreateWithInvariantCulture(
      string patternText)
    {
      return OffsetDateTimePattern.Create(patternText, NodaFormatInfo.InvariantInfo, OffsetDateTimePattern.DefaultTemplateValue);
    }

    public static OffsetDateTimePattern CreateWithCurrentCulture(
      string patternText)
    {
      return OffsetDateTimePattern.Create(patternText, NodaFormatInfo.CurrentInfo, OffsetDateTimePattern.DefaultTemplateValue);
    }

    public OffsetDateTimePattern WithPatternText(string newPatternText) => OffsetDateTimePattern.Create(newPatternText, this.formatInfo, this.templateValue);

    private OffsetDateTimePattern WithFormatInfo(NodaFormatInfo newFormatInfo) => OffsetDateTimePattern.Create(this.patternText, newFormatInfo, this.templateValue);

    public OffsetDateTimePattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public OffsetDateTimePattern WithTemplateValue(
      OffsetDateTime newTemplateValue)
    {
      return OffsetDateTimePattern.Create(this.patternText, this.formatInfo, newTemplateValue);
    }

    internal static class Patterns
    {
      internal static readonly OffsetDateTimePattern GeneralIsoPatternImpl = OffsetDateTimePattern.Create("yyyy'-'MM'-'dd'T'HH':'mm':'sso<G>", NodaFormatInfo.InvariantInfo, OffsetDateTimePattern.DefaultTemplateValue);
      internal static readonly OffsetDateTimePattern ExtendedIsoPatternImpl = OffsetDateTimePattern.Create("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFFo<G>", NodaFormatInfo.InvariantInfo, OffsetDateTimePattern.DefaultTemplateValue);
      internal static readonly OffsetDateTimePattern Rfc3339PatternImpl = OffsetDateTimePattern.Create("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFFo<Z+HH:mm>", NodaFormatInfo.InvariantInfo, OffsetDateTimePattern.DefaultTemplateValue);
      internal static readonly OffsetDateTimePattern FullRoundtripPatternImpl = OffsetDateTimePattern.Create("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFFo<G> '('c')'", NodaFormatInfo.InvariantInfo, OffsetDateTimePattern.DefaultTemplateValue);
      internal static readonly PatternBclSupport<OffsetDateTime> BclSupport = new PatternBclSupport<OffsetDateTime>("G", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<OffsetDateTime>>) (fi => fi.OffsetDateTimePatternParser));
    }
  }
}
