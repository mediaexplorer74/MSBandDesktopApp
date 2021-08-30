// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.InstantPatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using NodaTime.Utility;
using System;
using System.Globalization;

namespace NodaTime.Text
{
  internal sealed class InstantPatternParser : IPatternParser<Instant>
  {
    private const string GeneralPatternText = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
    private readonly string minLabel;
    private readonly string maxLabel;

    internal InstantPatternParser(string minLabel, string maxLabel)
    {
      this.minLabel = minLabel;
      this.maxLabel = maxLabel;
    }

    public IPattern<Instant> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText == "g")
        patternText = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
      if (patternText.Length > 1)
        return (IPattern<Instant>) new InstantPatternParser.LocalDateTimePatternAdapter(formatInfo.LocalDateTimePatternParser.ParsePattern(patternText), this.minLabel, this.maxLabel);
      char lowerInvariant = char.ToLowerInvariant(patternText[0]);
      switch (lowerInvariant)
      {
        case 'd':
          return (IPattern<Instant>) new InstantPatternParser.NumberPattern(formatInfo, patternText, "D");
        case 'n':
          return (IPattern<Instant>) new InstantPatternParser.NumberPattern(formatInfo, patternText, "N0");
        default:
          throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
          {
            (object) lowerInvariant,
            (object) typeof (Instant)
          });
      }
    }

    private sealed class NumberPattern : AbstractPattern<Instant>
    {
      private const NumberStyles ParsingNumberStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands;
      private readonly string patternText;
      private readonly string systemFormatString;

      internal NumberPattern(
        NodaFormatInfo formatInfo,
        string patternText,
        string systemFormatString)
        : base(formatInfo)
      {
        this.patternText = patternText;
        this.systemFormatString = systemFormatString;
      }

      protected override ParseResult<Instant> ParseImpl(string value)
      {
        long result;
        return long.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, (IFormatProvider) this.FormatInfo.NumberFormat, out result) ? ParseResult<Instant>.ForValue(new Instant(result)) : ParseResult<Instant>.CannotParseValue(new ValueCursor(value), this.patternText);
      }

      public override string Format(Instant value) => value.Ticks.ToString(this.systemFormatString, (IFormatProvider) this.FormatInfo.NumberFormat);
    }

    private sealed class LocalDateTimePatternAdapter : IPattern<Instant>
    {
      private readonly IPattern<LocalDateTime> pattern;
      private readonly string minLabel;
      private readonly string maxLabel;

      internal LocalDateTimePatternAdapter(
        IPattern<LocalDateTime> pattern,
        string minLabel,
        string maxLabel)
      {
        this.pattern = pattern;
        this.minLabel = minLabel;
        this.maxLabel = maxLabel;
      }

      public string Format(Instant value)
      {
        if (value == Instant.MinValue)
          return this.minLabel;
        if (value == Instant.MaxValue)
          return this.maxLabel;
        if (value.Ticks < CalendarSystem.Iso.MinTicks)
          return string.Format("{0} {1} ticks is earlier than the earliest supported ISO calendar value.", new object[2]
          {
            (object) "Out of formatting range: ",
            (object) value.Ticks
          });
        if (value.Ticks <= CalendarSystem.Iso.MaxTicks)
          return this.pattern.Format(new LocalDateTime(new LocalInstant(value.Ticks)));
        return string.Format("{0} {1} ticks is later than the latest supported ISO calendar value.", new object[2]
        {
          (object) "Out of formatting range: ",
          (object) value.Ticks
        });
      }

      public ParseResult<Instant> Parse(string text)
      {
        if (text == this.minLabel)
          return ParseResult<Instant>.ForValue(Instant.MinValue);
        return text == this.maxLabel ? ParseResult<Instant>.ForValue(Instant.MaxValue) : this.pattern.Parse(text).Convert<Instant>((Func<LocalDateTime, Instant>) (local => new Instant(local.LocalInstant.Ticks)));
      }
    }
  }
}
