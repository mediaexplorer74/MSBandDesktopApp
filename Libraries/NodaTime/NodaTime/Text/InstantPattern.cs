// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.InstantPattern
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
  public sealed class InstantPattern : IPattern<Instant>
  {
    internal const string DefaultMinLabel = "MinInstant";
    internal const string DefaultMaxLabel = "MaxInstant";
    internal const string OutOfRangeLabel = "Out of formatting range: ";
    private const string DefaultFormatPattern = "g";
    internal static readonly PatternBclSupport<Instant> BclSupport = new PatternBclSupport<Instant>("g", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<Instant>>) (fi => fi.InstantPatternParser));
    private readonly string minLabel;
    private readonly string maxLabel;
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPattern<Instant> pattern;

    public static InstantPattern GeneralPattern => InstantPattern.Patterns.GeneralPatternImpl;

    public static InstantPattern ExtendedIsoPattern => InstantPattern.Patterns.ExtendedIsoPatternImpl;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    private InstantPattern(
      string patternText,
      NodaFormatInfo formatInfo,
      string minLabel,
      string maxLabel,
      IPattern<Instant> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
      this.minLabel = minLabel;
      this.maxLabel = maxLabel;
    }

    public ParseResult<Instant> Parse(string text) => this.pattern.Parse(text);

    public string Format(Instant value) => this.pattern.Format(value);

    private static InstantPattern Create(
      string patternText,
      NodaFormatInfo formatInfo,
      string minLabel,
      string maxLabel)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<Instant> pattern;
      if (minLabel == "MinInstant" && maxLabel == "MaxInstant")
      {
        pattern = formatInfo.InstantPatternParser.ParsePattern(patternText);
      }
      else
      {
        Preconditions.CheckNotNull<string>(minLabel, nameof (minLabel));
        Preconditions.CheckNotNull<string>(maxLabel, nameof (maxLabel));
        Preconditions.CheckArgument(minLabel != "", nameof (minLabel), "minLabel must be non-empty");
        Preconditions.CheckArgument(maxLabel != "", nameof (maxLabel), "maxLabel must be non-empty");
        Preconditions.CheckArgument(minLabel != maxLabel, nameof (minLabel), "minLabel and maxLabel must differ");
        pattern = new InstantPatternParser(minLabel, maxLabel).ParsePattern(patternText, formatInfo);
      }
      return new InstantPattern(patternText, formatInfo, "MinInstant", "MaxInstant", pattern);
    }

    private static InstantPattern Create(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPattern<Instant> pattern = formatInfo.InstantPatternParser.ParsePattern(patternText);
      return new InstantPattern(patternText, formatInfo, "MinInstant", "MaxInstant", pattern);
    }

    public static InstantPattern Create(string patternText, CultureInfo cultureInfo) => InstantPattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo));

    public static InstantPattern CreateWithCurrentCulture(string patternText) => InstantPattern.Create(patternText, NodaFormatInfo.CurrentInfo);

    public static InstantPattern CreateWithInvariantCulture(string patternText) => InstantPattern.Create(patternText, NodaFormatInfo.InvariantInfo);

    public static InstantPattern CreateNumericPattern(
      CultureInfo cultureInfo,
      bool includeThousandsSeparators)
    {
      return InstantPattern.Create(includeThousandsSeparators ? "n" : "d", cultureInfo);
    }

    private InstantPattern WithFormatInfo(NodaFormatInfo formatInfo) => InstantPattern.Create(this.patternText, formatInfo, this.minLabel, this.maxLabel);

    public InstantPattern WithCulture(CultureInfo cultureInfo) => this.WithFormatInfo(NodaFormatInfo.GetFormatInfo(cultureInfo));

    public InstantPattern WithMinMaxLabels(string minLabel, string maxLabel) => InstantPattern.Create(this.patternText, this.formatInfo, minLabel, maxLabel);

    private static class Patterns
    {
      internal static readonly InstantPattern ExtendedIsoPatternImpl = InstantPattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFF'Z'");
      internal static readonly InstantPattern GeneralPatternImpl = InstantPattern.CreateWithInvariantCulture("yyyy-MM-ddTHH:mm:ss'Z'");
    }
  }
}
