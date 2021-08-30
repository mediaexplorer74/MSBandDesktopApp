// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.DurationPattern
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
  public sealed class DurationPattern : IPattern<Duration>
  {
    internal static readonly PatternBclSupport<Duration> BclSupport = new PatternBclSupport<Duration>("o", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<Duration>>) (fi => fi.DurationPatternParser));
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<Duration> pattern;

    public static DurationPattern RoundtripPattern => DurationPattern.Patterns.RoundtripPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    private DurationPattern(
      string patternText,
      NodaFormatInfo formatInfo,
      IPattern<Duration> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
    }

    public ParseResult<Duration> Parse(string text) => this.pattern.Parse(text);

    public string Format(Duration value) => this.pattern.Format(value);

    private static DurationPattern Create(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<Duration> pattern = formatInfo.DurationPatternParser.ParsePattern(patternText);
      return new DurationPattern(patternText, formatInfo, pattern);
    }

    public static DurationPattern Create(string patternText, CultureInfo cultureInfo) => DurationPattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo));

    public static DurationPattern CreateWithCurrentCulture(string patternText) => DurationPattern.Create(patternText, NodaFormatInfo.CurrentInfo);

    public static DurationPattern CreateWithInvariantCulture(string patternText) => DurationPattern.Create(patternText, NodaFormatInfo.InvariantInfo);

    public DurationPattern WithCulture(CultureInfo cultureInfo) => DurationPattern.Create(this.patternText, NodaFormatInfo.GetFormatInfo(cultureInfo));

    internal static class Patterns
    {
      internal static readonly DurationPattern RoundtripPatternImpl = DurationPattern.CreateWithInvariantCulture("-D:hh:mm:ss.FFFFFFF");
    }
  }
}
