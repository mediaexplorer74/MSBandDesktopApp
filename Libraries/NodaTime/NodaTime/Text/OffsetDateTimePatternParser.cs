// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.OffsetDateTimePatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.Text
{
  internal sealed class OffsetDateTimePatternParser : IPatternParser<OffsetDateTime>
  {
    private readonly LocalDate templateValueDate;
    private readonly LocalTime templateValueTime;
    private readonly Offset templateValueOffset;
    private static readonly Dictionary<char, CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandleBackslash)
      },
      {
        '/',
        (CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.DateSeparator, new Func<ValueCursor, ParseResult<OffsetDateTime>>(ParseResult<OffsetDateTime>.DateSeparatorMismatch)))
      },
      {
        'y',
        DatePatternHelper.CreateYearHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, int>) (value => value.YearOfCentury), (Func<OffsetDateTime, int>) (value => value.Year), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.Year = value))
      },
      {
        'Y',
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePaddedField(5, PatternFields.YearOfEra, 0, 99999, (Func<OffsetDateTime, int>) (value => value.YearOfEra), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.YearOfEra = value))
      },
      {
        'M',
        DatePatternHelper.CreateMonthOfYearHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, int>) (value => value.Month), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearText = value), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearNumeric = value))
      },
      {
        'd',
        DatePatternHelper.CreateDayHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, int>) (value => value.Day), (Func<OffsetDateTime, int>) (value => value.DayOfWeek), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfMonth = value), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfWeek = value))
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(7, (Func<OffsetDateTime, int>) (value => value.TickOfSecond), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ';',
        TimePatternHelper.CreateCommaDotHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(7, (Func<OffsetDateTime, int>) (value => value.TickOfSecond), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ':',
        (CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<OffsetDateTime>>(ParseResult<OffsetDateTime>.TimeSeparatorMismatch)))
      },
      {
        'h',
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours12, 1, 12, (Func<OffsetDateTime, int>) (value => value.ClockHourOfHalfDay), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours12 = value))
      },
      {
        'H',
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours24, 0, 24, (Func<OffsetDateTime, int>) (value => value.Hour), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours24 = value))
      },
      {
        'm',
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Minutes, 0, 59, (Func<OffsetDateTime, int>) (value => value.Minute), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Minutes = value))
      },
      {
        's',
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Seconds, 0, 59, (Func<OffsetDateTime, int>) (value => value.Second), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Seconds = value))
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(7, (Func<OffsetDateTime, int>) (value => value.TickOfSecond), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(7, (Func<OffsetDateTime, int>) (value => value.TickOfSecond), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        't',
        TimePatternHelper.CreateAmPmHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, int>) (time => time.Hour), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.AmPm = value))
      },
      {
        'c',
        DatePatternHelper.CreateCalendarHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, CalendarSystem>) (value => value.LocalDateTime.Calendar), (Action<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, CalendarSystem>) ((bucket, value) => bucket.Date.Calendar = value))
      },
      {
        'g',
        DatePatternHelper.CreateEraHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>((Func<OffsetDateTime, Era>) (value => value.Era), (Func<OffsetDateTimePatternParser.OffsetDateTimeParseBucket, LocalDatePatternParser.LocalDateParseBucket>) (bucket => bucket.Date))
      },
      {
        'o',
        new CharacterHandler<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(OffsetDateTimePatternParser.HandleOffset)
      }
    };

    internal OffsetDateTimePatternParser(OffsetDateTime templateValue)
    {
      this.templateValueDate = templateValue.Date;
      this.templateValueTime = templateValue.TimeOfDay;
      this.templateValueOffset = templateValue.Offset;
    }

    public IPattern<OffsetDateTime> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText.Length == 1)
      {
        switch (patternText[0])
        {
          case 'G':
            return (IPattern<OffsetDateTime>) OffsetDateTimePattern.Patterns.GeneralIsoPatternImpl;
          case 'o':
            return (IPattern<OffsetDateTime>) OffsetDateTimePattern.Patterns.ExtendedIsoPatternImpl;
          case 'r':
            return (IPattern<OffsetDateTime>) OffsetDateTimePattern.Patterns.FullRoundtripPatternImpl;
          default:
            throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
            {
              (object) patternText[0],
              (object) typeof (OffsetDateTime)
            });
        }
      }
      else
      {
        SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>(formatInfo, (Func<OffsetDateTimePatternParser.OffsetDateTimeParseBucket>) (() => new OffsetDateTimePatternParser.OffsetDateTimeParseBucket(this.templateValueDate, this.templateValueTime, this.templateValueOffset)));
        steppedPatternBuilder.ParseCustomPattern(patternText, OffsetDateTimePatternParser.PatternCharacterHandlers);
        steppedPatternBuilder.ValidateUsedFields();
        return (IPattern<OffsetDateTime>) steppedPatternBuilder.Build();
      }
    }

    private static void HandleOffset(
      PatternCursor pattern,
      SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket> builder)
    {
      builder.AddField(PatternFields.EmbeddedOffset, pattern.Current);
      IPartialPattern<Offset> offsetPattern = OffsetPattern.Create(pattern.GetEmbeddedPattern('<', '>'), builder.FormatInfo).UnderlyingPattern;
      builder.AddParseAction((SteppedPatternBuilder<OffsetDateTime, OffsetDateTimePatternParser.OffsetDateTimeParseBucket>.ParseAction) ((value, bucket) =>
      {
        ParseResult<Offset> partial = offsetPattern.ParsePartial(value);
        if (!partial.Success)
          return partial.ConvertError<OffsetDateTime>();
        bucket.Offset = partial.Value;
        return (ParseResult<OffsetDateTime>) null;
      }));
      builder.AddFormatAction((Action<OffsetDateTime, StringBuilder>) ((value, sb) => offsetPattern.FormatPartial(value.Offset, sb)));
    }

    private sealed class OffsetDateTimeParseBucket : ParseBucket<OffsetDateTime>
    {
      internal readonly LocalDatePatternParser.LocalDateParseBucket Date;
      internal readonly LocalTimePatternParser.LocalTimeParseBucket Time;
      internal Offset Offset;

      internal OffsetDateTimeParseBucket(
        LocalDate templateDate,
        LocalTime templateTime,
        Offset templateOffset)
      {
        this.Date = new LocalDatePatternParser.LocalDateParseBucket(templateDate);
        this.Time = new LocalTimePatternParser.LocalTimeParseBucket(templateTime);
        this.Offset = templateOffset;
      }

      internal override ParseResult<OffsetDateTime> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        ParseResult<LocalDateTime> parseResult = LocalDateTimePatternParser.LocalDateTimeParseBucket.CombineBuckets(usedFields, this.Date, this.Time, text);
        return !parseResult.Success ? parseResult.ConvertError<OffsetDateTime>() : ParseResult<OffsetDateTime>.ForValue(parseResult.Value.WithOffset(this.Offset));
      }
    }
  }
}
