// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.DurationPatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NodaTime.Text
{
  internal sealed class DurationPatternParser : IPatternParser<Duration>
  {
    private static readonly Dictionary<char, CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket>.HandleBackslash)
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<Duration, DurationPatternParser.DurationParseBucket>(7, new Func<Duration, int>(DurationPatternParser.GetPositiveTickOfSecond), (Action<DurationPatternParser.DurationParseBucket, int>) ((bucket, value) => {checked {bucket.NegativeTicks -= (long) value;}}))
      },
      {
        ':',
        (CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<Duration>>(ParseResult<Duration>.TimeSeparatorMismatch)))
      },
      {
        'D',
        DurationPatternParser.CreateTotalHandler(PatternFields.DayOfMonth, 864000000000L)
      },
      {
        'H',
        DurationPatternParser.CreateTotalHandler(PatternFields.Hours24, 36000000000L)
      },
      {
        'h',
        DurationPatternParser.CreatePartialHandler(PatternFields.Hours24, 36000000000L, 24)
      },
      {
        'M',
        DurationPatternParser.CreateTotalHandler(PatternFields.Minutes, 600000000L)
      },
      {
        'm',
        DurationPatternParser.CreatePartialHandler(PatternFields.Minutes, 600000000L, 60)
      },
      {
        'S',
        DurationPatternParser.CreateTotalHandler(PatternFields.Seconds, 10000000L)
      },
      {
        's',
        DurationPatternParser.CreatePartialHandler(PatternFields.Seconds, 10000000L, 60)
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<Duration, DurationPatternParser.DurationParseBucket>(7, new Func<Duration, int>(DurationPatternParser.GetPositiveTickOfSecond), (Action<DurationPatternParser.DurationParseBucket, int>) ((bucket, value) => {checked {bucket.NegativeTicks -= (long) value;}}))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<Duration, DurationPatternParser.DurationParseBucket>(7, new Func<Duration, int>(DurationPatternParser.GetPositiveTickOfSecond), (Action<DurationPatternParser.DurationParseBucket, int>) ((bucket, value) => {checked {bucket.NegativeTicks -= (long) value;}}))
      },
      {
        '+',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(DurationPatternParser.HandlePlus)
      },
      {
        '-',
        new CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>(DurationPatternParser.HandleMinus)
      }
    };

    public IPattern<Duration> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      Preconditions.CheckNotNull<string>(patternText, nameof (patternText));
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText.Length == 1)
      {
        if (patternText[0] == 'o')
          return (IPattern<Duration>) DurationPattern.Patterns.RoundtripPatternImpl;
        throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
        {
          (object) patternText[0],
          (object) typeof (Duration)
        });
      }
      SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket>(formatInfo, (Func<DurationPatternParser.DurationParseBucket>) (() => new DurationPatternParser.DurationParseBucket()));
      steppedPatternBuilder.ParseCustomPattern(patternText, DurationPatternParser.PatternCharacterHandlers);
      return (IPattern<Duration>) steppedPatternBuilder.Build();
    }

    private static int GetPositiveTickOfSecond(Duration duration) => checked ((int) unchecked (DurationPatternParser.GetPositiveTicks(duration) % 10000000UL));

    private static CharacterHandler<Duration, DurationPatternParser.DurationParseBucket> CreateTotalHandler(
      PatternFields field,
      long ticksPerUnit)
    {
      return (CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>) ((pattern, builder) =>
      {
        int repeatCount = pattern.GetRepeatCount(10);
        if ((builder.UsedFields & PatternFields.TotalDuration) != PatternFields.None)
          throw new InvalidPatternException(Messages.Parse_MultipleCapitalDurationFields);
        builder.AddField(field, pattern.Current);
        builder.AddField(PatternFields.TotalDuration, pattern.Current);
        builder.AddParseValueAction(repeatCount, 10, pattern.Current, 0, int.MaxValue, (Action<DurationPatternParser.DurationParseBucket, int>) ((bucket, value) => {checked {bucket.NegativeTicks -= (long) value * ticksPerUnit;}}));
        builder.AddFormatLeftPad(repeatCount, (Func<Duration, int>) (duration => {checked {(int) unchecked (DurationPatternParser.GetPositiveTicks(duration) / checked ((ulong) ticksPerUnit));}}));
      });
    }

    private static CharacterHandler<Duration, DurationPatternParser.DurationParseBucket> CreatePartialHandler(
      PatternFields field,
      long ticksPerUnit,
      int unitsPerContainer)
    {
      return (CharacterHandler<Duration, DurationPatternParser.DurationParseBucket>) ((pattern, builder) =>
      {
        int repeatCount = pattern.GetRepeatCount(2);
        builder.AddField(field, pattern.Current);
        builder.AddParseValueAction(repeatCount, 2, pattern.Current, 0, checked (unitsPerContainer - 1), (Action<DurationPatternParser.DurationParseBucket, int>) ((bucket, value) => {checked {bucket.NegativeTicks -= (long) value * ticksPerUnit;}}));
        builder.AddFormatLeftPad(repeatCount, (Func<Duration, int>) (duration => {checked {(int) unchecked (DurationPatternParser.GetPositiveTicks(duration) / checked ((ulong) ticksPerUnit) % checked ((ulong) unitsPerContainer));}}));
      });
    }

    private static void HandlePlus(
      PatternCursor pattern,
      SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket> builder)
    {
      builder.AddField(PatternFields.Sign, pattern.Current);
      builder.AddRequiredSign((Action<DurationPatternParser.DurationParseBucket, bool>) ((bucket, positive) => bucket.IsNegative = !positive), (Func<Duration, bool>) (duration => duration.Ticks >= 0L));
    }

    private static void HandleMinus(
      PatternCursor pattern,
      SteppedPatternBuilder<Duration, DurationPatternParser.DurationParseBucket> builder)
    {
      builder.AddField(PatternFields.Sign, pattern.Current);
      builder.AddNegativeOnlySign((Action<DurationPatternParser.DurationParseBucket, bool>) ((bucket, positive) => bucket.IsNegative = !positive), (Func<Duration, bool>) (duration => duration.Ticks >= 0L));
    }

    private static ulong GetPositiveTicks(Duration duration)
    {
      long ticks = duration.Ticks;
      return ticks != long.MinValue ? checked ((ulong) Math.Abs(ticks)) : 9223372036854775808UL;
    }

    [DebuggerStepThrough]
    private sealed class DurationParseBucket : ParseBucket<Duration>
    {
      internal long NegativeTicks;
      public bool IsNegative;

      internal override ParseResult<Duration> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        return ParseResult<Duration>.ForValue(Duration.FromTicks(this.IsNegative ? this.NegativeTicks : checked (-this.NegativeTicks)));
      }
    }
  }
}
