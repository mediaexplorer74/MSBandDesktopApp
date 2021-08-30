// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.OffsetPattern
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
  public sealed class OffsetPattern : IPattern<Offset>
  {
    private const string DefaultFormatPattern = "g";
    public static readonly OffsetPattern GeneralInvariantPattern = OffsetPattern.CreateWithInvariantCulture("g");
    public static readonly OffsetPattern GeneralInvariantPatternWithZ = OffsetPattern.CreateWithInvariantCulture("G");
    internal static readonly PatternBclSupport<Offset> BclSupport = new PatternBclSupport<Offset>("g", (Func<NodaFormatInfo, FixedFormatInfoPatternParser<Offset>>) (fi => fi.OffsetPatternParser));
    private readonly string patternText;
    private readonly NodaFormatInfo formatInfo;
    private readonly IPartialPattern<Offset> pattern;

    public string PatternText => this.patternText;

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    private OffsetPattern(
      string patternText,
      NodaFormatInfo formatInfo,
      IPartialPattern<Offset> pattern)
    {
      this.patternText = patternText;
      this.formatInfo = formatInfo;
      this.pattern = pattern;
    }

    internal IPartialPattern<Offset> UnderlyingPattern => this.pattern;

    public ParseResult<Offset> Parse(string text) => this.pattern.Parse(text);

    public string Format(Offset value) => this.pattern.Format(value);

    internal static OffsetPattern Create(string patternText, NodaFormatInfo formatInfo)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      Preconditions.CheckNotNull<NodaFormatInfo>(formatInfo, nameof (formatInfo));
      IPartialPattern<Offset> pattern = (IPartialPattern<Offset>) formatInfo.OffsetPatternParser.ParsePattern(patternText);
      return new OffsetPattern(patternText, formatInfo, pattern);
    }

    public static OffsetPattern Create(string patternText, CultureInfo cultureInfo) => OffsetPattern.Create(patternText, NodaFormatInfo.GetFormatInfo(cultureInfo));

    public static OffsetPattern CreateWithCurrentCulture(string patternText) => OffsetPattern.Create(patternText, NodaFormatInfo.CurrentInfo);

    public static OffsetPattern CreateWithInvariantCulture(string patternText) => OffsetPattern.Create(patternText, NodaFormatInfo.InvariantInfo);

    public OffsetPattern WithCulture(CultureInfo cultureInfo) => OffsetPattern.Create(this.patternText, NodaFormatInfo.GetFormatInfo(cultureInfo));
  }
}
