// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.LocalDatePatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NodaTime.Text
{
  internal sealed class LocalDatePatternParser : IPatternParser<LocalDate>
  {
    private const int TwoDigitYearMax = 30;
    private readonly LocalDate templateValue;
    private static readonly Dictionary<char, CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>(SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>(SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>(SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>(SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>.HandleBackslash)
      },
      {
        '/',
        (CharacterHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.DateSeparator, new Func<ValueCursor, ParseResult<LocalDate>>(ParseResult<LocalDate>.DateSeparatorMismatch)))
      },
      {
        'y',
        DatePatternHelper.CreateYearHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>((Func<LocalDate, int>) (value => value.YearOfCentury), (Func<LocalDate, int>) (value => value.Year), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.Year = value))
      },
      {
        'Y',
        SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>.HandlePaddedField(5, PatternFields.YearOfEra, 0, 99999, (Func<LocalDate, int>) (value => value.YearOfEra), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.YearOfEra = value))
      },
      {
        'M',
        DatePatternHelper.CreateMonthOfYearHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>((Func<LocalDate, int>) (value => value.Month), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.MonthOfYearText = value), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.MonthOfYearNumeric = value))
      },
      {
        'd',
        DatePatternHelper.CreateDayHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>((Func<LocalDate, int>) (value => value.Day), (Func<LocalDate, int>) (value => value.DayOfWeek), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.DayOfMonth = value), (Action<LocalDatePatternParser.LocalDateParseBucket, int>) ((bucket, value) => bucket.DayOfWeek = value))
      },
      {
        'c',
        DatePatternHelper.CreateCalendarHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>((Func<LocalDate, CalendarSystem>) (value => value.Calendar), (Action<LocalDatePatternParser.LocalDateParseBucket, CalendarSystem>) ((bucket, value) => bucket.Calendar = value))
      },
      {
        'g',
        DatePatternHelper.CreateEraHandler<LocalDate, LocalDatePatternParser.LocalDateParseBucket>((Func<LocalDate, Era>) (date => date.Era), (Func<LocalDatePatternParser.LocalDateParseBucket, LocalDatePatternParser.LocalDateParseBucket>) (bucket => bucket))
      }
    };

    internal LocalDatePatternParser(LocalDate templateValue) => this.templateValue = templateValue;

    public IPattern<LocalDate> ParsePattern(
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
            (object) typeof (LocalDate)
          });
      }
      SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<LocalDate, LocalDatePatternParser.LocalDateParseBucket>(formatInfo, (Func<LocalDatePatternParser.LocalDateParseBucket>) (() => new LocalDatePatternParser.LocalDateParseBucket(this.templateValue)));
      steppedPatternBuilder.ParseCustomPattern(patternText, LocalDatePatternParser.PatternCharacterHandlers);
      steppedPatternBuilder.ValidateUsedFields();
      return (IPattern<LocalDate>) steppedPatternBuilder.Build();
    }

    private string ExpandStandardFormatPattern(char patternCharacter, NodaFormatInfo formatInfo)
    {
      switch (patternCharacter)
      {
        case 'D':
          return formatInfo.DateTimeFormat.LongDatePattern;
        case 'd':
          return formatInfo.DateTimeFormat.ShortDatePattern;
        default:
          return (string) null;
      }
    }

    internal sealed class LocalDateParseBucket : ParseBucket<LocalDate>
    {
      private readonly LocalDate templateValue;
      internal CalendarSystem Calendar;
      internal int Year;
      private int EraIndex;
      internal int YearOfEra;
      internal int MonthOfYearNumeric;
      internal int MonthOfYearText;
      internal int DayOfMonth;
      internal int DayOfWeek;

      internal LocalDateParseBucket(LocalDate templateValue)
      {
        this.templateValue = templateValue;
        this.Calendar = templateValue.Calendar;
      }

      internal ParseResult<TResult> ParseEra<TResult>(
        NodaFormatInfo formatInfo,
        ValueCursor cursor)
      {
        CompareInfo compareInfo = formatInfo.CompareInfo;
        IList<Era> eras = this.Calendar.Eras;
        int index = 0;
        while (index < eras.Count)
        {
          foreach (string eraName in (IEnumerable<string>) formatInfo.GetEraNames(eras[index]))
          {
            if (cursor.MatchCaseInsensitive(eraName, compareInfo, true))
            {
              this.EraIndex = index;
              return (ParseResult<TResult>) null;
            }
          }
          checked { ++index; }
        }
        return ParseResult<TResult>.MismatchedText(cursor, 'g');
      }

      internal override ParseResult<LocalDate> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        ParseResult<LocalDate> year = this.DetermineYear(usedFields, text);
        if (year != null)
          return year;
        ParseResult<LocalDate> month = this.DetermineMonth(usedFields, text);
        if (month != null)
          return month;
        int day = ParseBucket<LocalDate>.IsFieldUsed(usedFields, PatternFields.DayOfMonth) ? this.DayOfMonth : this.templateValue.Day;
        if (day > this.Calendar.GetDaysInMonth(this.Year, this.MonthOfYearNumeric))
          return ParseResult<LocalDate>.DayOfMonthOutOfRange(text, day, this.MonthOfYearNumeric, this.Year);
        LocalDate localDate = new LocalDate(this.Year, this.MonthOfYearNumeric, day, this.Calendar);
        return ParseBucket<LocalDate>.IsFieldUsed(usedFields, PatternFields.DayOfWeek) && this.DayOfWeek != localDate.DayOfWeek ? ParseResult<LocalDate>.InconsistentDayOfWeekTextValue(text) : ParseResult<LocalDate>.ForValue(localDate);
      }

      private ParseResult<LocalDate> DetermineYear(
        PatternFields usedFields,
        string text)
      {
        int num = 0;
        if (ParseBucket<LocalDate>.IsFieldUsed(usedFields, PatternFields.YearOfEra))
        {
          if (!ParseBucket<LocalDate>.IsFieldUsed(usedFields, PatternFields.Era))
            this.EraIndex = this.Calendar.Eras.IndexOf(this.templateValue.Era);
          if (this.YearOfEra < this.Calendar.GetMinYearOfEra(this.EraIndex) || this.YearOfEra > this.Calendar.GetMaxYearOfEra(this.EraIndex))
            return ParseResult<LocalDate>.YearOfEraOutOfRange(text, this.YearOfEra, this.EraIndex, this.Calendar);
          num = this.Calendar.GetAbsoluteYear(this.YearOfEra, this.EraIndex);
        }
        switch (usedFields & (PatternFields.Year | PatternFields.YearTwoDigits | PatternFields.YearOfEra))
        {
          case PatternFields.None:
            this.Year = this.templateValue.Year;
            break;
          case PatternFields.Year | PatternFields.YearTwoDigits:
            this.Year = LocalDatePatternParser.LocalDateParseBucket.GetAbsoluteYearFromTwoDigits(this.templateValue.Year, this.Year);
            break;
          case PatternFields.YearOfEra:
            this.Year = num;
            break;
          case PatternFields.Year | PatternFields.YearOfEra:
            if (this.Year != num)
              return ParseResult<LocalDate>.InconsistentValues(text, 'y', 'Y');
            this.Year = num;
            break;
          case PatternFields.Year | PatternFields.YearTwoDigits | PatternFields.YearOfEra:
            if (Math.Abs(num) % 100 != this.Year)
              return ParseResult<LocalDate>.InconsistentValues(text, 'y', 'Y');
            this.Year = num;
            break;
        }
        return this.Year > this.Calendar.MaxYear || this.Year < this.Calendar.MinYear ? ParseResult<LocalDate>.FieldValueOutOfRangePostParse(text, this.Year, 'y') : (ParseResult<LocalDate>) null;
      }

      private static int GetAbsoluteYearFromTwoDigits(int absoluteBase, int twoDigits)
      {
        if (absoluteBase < 0)
          return checked (-LocalDatePatternParser.LocalDateParseBucket.GetAbsoluteYearFromTwoDigits(Math.Abs(absoluteBase), twoDigits));
        int num = checked (absoluteBase - unchecked (absoluteBase % 100));
        if (twoDigits > 30)
          checked { num -= 100; }
        return checked (num + twoDigits);
      }

      private ParseResult<LocalDate> DetermineMonth(
        PatternFields usedFields,
        string text)
      {
        switch (usedFields & (PatternFields.MonthOfYearNumeric | PatternFields.MonthOfYearText))
        {
          case PatternFields.None:
            this.MonthOfYearNumeric = this.templateValue.Month;
            break;
          case PatternFields.MonthOfYearText:
            this.MonthOfYearNumeric = this.MonthOfYearText;
            break;
          case PatternFields.MonthOfYearNumeric | PatternFields.MonthOfYearText:
            if (this.MonthOfYearNumeric != this.MonthOfYearText)
              return ParseResult<LocalDate>.InconsistentMonthValues(text);
            break;
        }
        return this.MonthOfYearNumeric > this.Calendar.GetMaxMonth(this.Year) ? ParseResult<LocalDate>.MonthOutOfRange(text, this.MonthOfYearNumeric, this.Year) : (ParseResult<LocalDate>) null;
      }
    }
  }
}
