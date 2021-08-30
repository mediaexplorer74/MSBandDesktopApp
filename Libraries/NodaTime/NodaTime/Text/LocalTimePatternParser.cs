// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalTimePatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using System;
using System.Collections.Generic;

namespace NodaTime.Text
{
  internal sealed class LocalTimePatternParser : IPatternParser<LocalTime>
  {
    private readonly LocalTime templateValue;
    private static readonly Dictionary<char, CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandleBackslash)
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(7, (Func<LocalTime, int>) (value => value.TickOfSecond), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.FractionalSeconds = value))
      },
      {
        ';',
        TimePatternHelper.CreateCommaDotHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(7, (Func<LocalTime, int>) (value => value.TickOfSecond), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.FractionalSeconds = value))
      },
      {
        ':',
        (CharacterHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<LocalTime>>(ParseResult<LocalTime>.TimeSeparatorMismatch)))
      },
      {
        'h',
        SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours12, 1, 12, (Func<LocalTime, int>) (value => value.ClockHourOfHalfDay), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.Hours12 = value))
      },
      {
        'H',
        SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours24, 0, 23, (Func<LocalTime, int>) (value => value.Hour), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.Hours24 = value))
      },
      {
        'm',
        SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandlePaddedField(2, PatternFields.Minutes, 0, 59, (Func<LocalTime, int>) (value => value.Minute), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.Minutes = value))
      },
      {
        's',
        SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>.HandlePaddedField(2, PatternFields.Seconds, 0, 59, (Func<LocalTime, int>) (value => value.Second), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.Seconds = value))
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(7, (Func<LocalTime, int>) (value => value.TickOfSecond), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.FractionalSeconds = value))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(7, (Func<LocalTime, int>) (value => value.TickOfSecond), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.FractionalSeconds = value))
      },
      {
        't',
        TimePatternHelper.CreateAmPmHandler<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>((Func<LocalTime, int>) (time => time.Hour), (Action<LocalTimePatternParser.LocalTimeParseBucket, int>) ((bucket, value) => bucket.AmPm = value))
      }
    };

    public LocalTimePatternParser(LocalTime templateValue) => this.templateValue = templateValue;

    public IPattern<LocalTime> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText.Length == 1)
      {
        char patternCharacter = patternText[0];
        patternText = this.ExpandStandardFormatPattern(patternCharacter, formatInfo);
        if (patternText == null)
          throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
          {
            (object) patternCharacter,
            (object) typeof (LocalTime)
          });
      }
      SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<LocalTime, LocalTimePatternParser.LocalTimeParseBucket>(formatInfo, (Func<LocalTimePatternParser.LocalTimeParseBucket>) (() => new LocalTimePatternParser.LocalTimeParseBucket(this.templateValue)));
      steppedPatternBuilder.ParseCustomPattern(patternText, LocalTimePatternParser.PatternCharacterHandlers);
      steppedPatternBuilder.ValidateUsedFields();
      return (IPattern<LocalTime>) steppedPatternBuilder.Build();
    }

    private string ExpandStandardFormatPattern(char patternCharacter, NodaFormatInfo formatInfo)
    {
      switch (patternCharacter)
      {
        case 'T':
          return formatInfo.DateTimeFormat.LongTimePattern;
        case 'r':
          return "HH:mm:ss.FFFFFFF";
        case 't':
          return formatInfo.DateTimeFormat.ShortTimePattern;
        default:
          return (string) null;
      }
    }

    internal sealed class LocalTimeParseBucket : ParseBucket<LocalTime>
    {
      private readonly LocalTime templateValue;
      internal int FractionalSeconds;
      internal int Hours24;
      internal int Hours12;
      internal int Minutes;
      internal int Seconds;
      internal int AmPm;

      internal LocalTimeParseBucket(LocalTime templateValue) => this.templateValue = templateValue;

      internal override ParseResult<LocalTime> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        if (this.AmPm == 2)
          this.AmPm = this.templateValue.Hour / 12;
        int hour1;
        ParseResult<LocalTime> hour2 = this.DetermineHour(usedFields, text, out hour1);
        if (hour2 != null)
          return hour2;
        int minute = ParseBucket<LocalTime>.IsFieldUsed(usedFields, PatternFields.Minutes) ? this.Minutes : this.templateValue.Minute;
        int second = ParseBucket<LocalTime>.IsFieldUsed(usedFields, PatternFields.Seconds) ? this.Seconds : this.templateValue.Second;
        int tickWithinSecond = ParseBucket<LocalTime>.IsFieldUsed(usedFields, PatternFields.FractionalSeconds) ? this.FractionalSeconds : this.templateValue.TickOfSecond;
        return ParseResult<LocalTime>.ForValue(LocalTime.FromHourMinuteSecondTick(hour1, minute, second, tickWithinSecond));
      }

      private ParseResult<LocalTime> DetermineHour(
        PatternFields usedFields,
        string text,
        out int hour)
      {
        hour = 0;
        if (ParseBucket<LocalTime>.IsFieldUsed(usedFields, PatternFields.Hours24))
        {
          if (ParseBucket<LocalTime>.AreAllFieldsUsed(usedFields, PatternFields.Hours12 | PatternFields.Hours24) && this.Hours12 % 12 != this.Hours24 % 12)
            return ParseResult<LocalTime>.InconsistentValues(text, 'H', 'h');
          if (ParseBucket<LocalTime>.IsFieldUsed(usedFields, PatternFields.AmPm) && this.Hours24 / 12 != this.AmPm)
            return ParseResult<LocalTime>.InconsistentValues(text, 'H', 't');
          hour = this.Hours24;
          return (ParseResult<LocalTime>) null;
        }
        switch (usedFields & (PatternFields.Hours12 | PatternFields.AmPm))
        {
          case PatternFields.None:
            hour = this.templateValue.Hour;
            break;
          case PatternFields.Hours12:
            hour = checked (unchecked (this.Hours12 % 12) + unchecked (this.templateValue.Hour / 12) * 12);
            break;
          case PatternFields.AmPm:
            hour = checked (unchecked (this.templateValue.Hour % 12) + this.AmPm * 12);
            break;
          case PatternFields.Hours12 | PatternFields.AmPm:
            hour = checked (unchecked (this.Hours12 % 12) + this.AmPm * 12);
            break;
        }
        return (ParseResult<LocalTime>) null;
      }
    }
  }
}
