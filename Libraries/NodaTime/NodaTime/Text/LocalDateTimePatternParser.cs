// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalDateTimePatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using System;
using System.Collections.Generic;

namespace NodaTime.Text
{
  internal sealed class LocalDateTimePatternParser : IPatternParser<LocalDateTime>
  {
    private readonly LocalDate templateValueDate;
    private readonly LocalTime templateValueTime;
    private static readonly Dictionary<char, CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandleBackslash)
      },
      {
        '/',
        (CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.DateSeparator, new Func<ValueCursor, ParseResult<LocalDateTime>>(ParseResult<LocalDateTime>.DateSeparatorMismatch)))
      },
      {
        'y',
        DatePatternHelper.CreateYearHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, int>) (value => value.YearOfCentury), (Func<LocalDateTime, int>) (value => value.Year), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.Year = value))
      },
      {
        'Y',
        SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePaddedField(5, PatternFields.YearOfEra, 0, 99999, (Func<LocalDateTime, int>) (value => value.YearOfEra), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.YearOfEra = value))
      },
      {
        'M',
        DatePatternHelper.CreateMonthOfYearHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, int>) (value => value.Month), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearText = value), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearNumeric = value))
      },
      {
        'd',
        DatePatternHelper.CreateDayHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, int>) (value => value.Day), (Func<LocalDateTime, int>) (value => value.DayOfWeek), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfMonth = value), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfWeek = value))
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(7, (Func<LocalDateTime, int>) (value => value.TickOfSecond), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ';',
        TimePatternHelper.CreateCommaDotHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(7, (Func<LocalDateTime, int>) (value => value.TickOfSecond), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ':',
        (CharacterHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<LocalDateTime>>(ParseResult<LocalDateTime>.TimeSeparatorMismatch)))
      },
      {
        'h',
        SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours12, 1, 12, (Func<LocalDateTime, int>) (value => value.ClockHourOfHalfDay), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours12 = value))
      },
      {
        'H',
        SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours24, 0, 24, (Func<LocalDateTime, int>) (value => value.Hour), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours24 = value))
      },
      {
        'm',
        SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Minutes, 0, 59, (Func<LocalDateTime, int>) (value => value.Minute), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Minutes = value))
      },
      {
        's',
        SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Seconds, 0, 59, (Func<LocalDateTime, int>) (value => value.Second), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Seconds = value))
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(7, (Func<LocalDateTime, int>) (value => value.TickOfSecond), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(7, (Func<LocalDateTime, int>) (value => value.TickOfSecond), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        't',
        TimePatternHelper.CreateAmPmHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, int>) (time => time.Hour), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.AmPm = value))
      },
      {
        'c',
        DatePatternHelper.CreateCalendarHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, CalendarSystem>) (value => value.Calendar), (Action<LocalDateTimePatternParser.LocalDateTimeParseBucket, CalendarSystem>) ((bucket, value) => bucket.Date.Calendar = value))
      },
      {
        'g',
        DatePatternHelper.CreateEraHandler<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>((Func<LocalDateTime, Era>) (value => value.Era), (Func<LocalDateTimePatternParser.LocalDateTimeParseBucket, LocalDatePatternParser.LocalDateParseBucket>) (bucket => bucket.Date))
      }
    };

    internal LocalDateTimePatternParser(LocalDateTime templateValue)
    {
      this.templateValueDate = templateValue.Date;
      this.templateValueTime = templateValue.TimeOfDay;
    }

    public IPattern<LocalDateTime> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText.Length == 1)
      {
        char patternCharacter = patternText[0];
        switch (patternCharacter)
        {
          case 'O':
          case 'o':
            return (IPattern<LocalDateTime>) LocalDateTimePattern.Patterns.BclRoundtripPatternImpl;
          case 'r':
            return (IPattern<LocalDateTime>) LocalDateTimePattern.Patterns.FullRoundtripPatternImpl;
          case 's':
            return (IPattern<LocalDateTime>) LocalDateTimePattern.Patterns.GeneralIsoPatternImpl;
          default:
            patternText = this.ExpandStandardFormatPattern(patternCharacter, formatInfo);
            if (patternText == null)
              throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
              {
                (object) patternCharacter,
                (object) typeof (LocalDateTime)
              });
            break;
        }
      }
      SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<LocalDateTime, LocalDateTimePatternParser.LocalDateTimeParseBucket>(formatInfo, (Func<LocalDateTimePatternParser.LocalDateTimeParseBucket>) (() => new LocalDateTimePatternParser.LocalDateTimeParseBucket(this.templateValueDate, this.templateValueTime)));
      steppedPatternBuilder.ParseCustomPattern(patternText, LocalDateTimePatternParser.PatternCharacterHandlers);
      steppedPatternBuilder.ValidateUsedFields();
      return (IPattern<LocalDateTime>) steppedPatternBuilder.Build();
    }

    private string ExpandStandardFormatPattern(char patternCharacter, NodaFormatInfo formatInfo)
    {
      switch (patternCharacter)
      {
        case 'F':
          return formatInfo.DateTimeFormat.FullDateTimePattern;
        case 'G':
          return formatInfo.DateTimeFormat.ShortDatePattern + " " + formatInfo.DateTimeFormat.LongTimePattern;
        case 'f':
          return formatInfo.DateTimeFormat.LongDatePattern + " " + formatInfo.DateTimeFormat.ShortTimePattern;
        case 'g':
          return formatInfo.DateTimeFormat.ShortDatePattern + " " + formatInfo.DateTimeFormat.ShortTimePattern;
        default:
          return (string) null;
      }
    }

    internal sealed class LocalDateTimeParseBucket : ParseBucket<LocalDateTime>
    {
      internal readonly LocalDatePatternParser.LocalDateParseBucket Date;
      internal readonly LocalTimePatternParser.LocalTimeParseBucket Time;

      internal LocalDateTimeParseBucket(LocalDate templateValueDate, LocalTime templateValueTime)
      {
        this.Date = new LocalDatePatternParser.LocalDateParseBucket(templateValueDate);
        this.Time = new LocalTimePatternParser.LocalTimeParseBucket(templateValueTime);
      }

      internal static ParseResult<LocalDateTime> CombineBuckets(
        PatternFields usedFields,
        LocalDatePatternParser.LocalDateParseBucket dateBucket,
        LocalTimePatternParser.LocalTimeParseBucket timeBucket,
        string text)
      {
        bool flag = false;
        if (timeBucket.Hours24 == 24)
        {
          timeBucket.Hours24 = 0;
          flag = true;
        }
        ParseResult<LocalDate> parseResult1 = dateBucket.CalculateValue(usedFields & PatternFields.AllDateFields, text);
        if (!parseResult1.Success)
          return parseResult1.ConvertError<LocalDateTime>();
        ParseResult<LocalTime> parseResult2 = timeBucket.CalculateValue(usedFields & PatternFields.AllTimeFields, text);
        if (!parseResult2.Success)
          return parseResult2.ConvertError<LocalDateTime>();
        LocalDate localDate = parseResult1.Value;
        LocalTime localTime = parseResult2.Value;
        if (flag)
        {
          if (localTime != LocalTime.Midnight)
            return ParseResult<LocalDateTime>.InvalidHour24(text);
          localDate = localDate.PlusDays(1);
        }
        return ParseResult<LocalDateTime>.ForValue(localDate + localTime);
      }

      internal override ParseResult<LocalDateTime> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        return LocalDateTimePatternParser.LocalDateTimeParseBucket.CombineBuckets(usedFields, this.Date, this.Time, text);
      }
    }
  }
}
