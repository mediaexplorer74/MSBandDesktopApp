// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.DatePatternHelper
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Globalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.Text.Patterns
{
  internal static class DatePatternHelper
  {
    internal static CharacterHandler<TResult, TBucket> CreateYearHandler<TResult, TBucket>(
      Func<TResult, int> centuryGetter,
      Func<TResult, int> yearGetter,
      Action<TBucket, int> setter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        int count = pattern.GetRepeatCount(5);
        builder.AddField(PatternFields.Year, pattern.Current);
        switch (count)
        {
          case 1:
          case 2:
            builder.AddParseValueAction(count, 2, 'y', -99, 99, setter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(centuryGetter(value), count, sb)));
            builder.AddField(PatternFields.YearTwoDigits, pattern.Current);
            break;
          case 3:
            builder.AddParseValueAction(3, 5, 'y', -99999, 99999, setter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(yearGetter(value), 3, sb)));
            break;
          case 4:
            bool flag = DatePatternHelper.CheckIfNextCharacterMightBeDigit(pattern);
            builder.AddParseValueAction(4, flag ? 4 : 5, 'y', -99999, 99999, setter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(yearGetter(value), 4, sb)));
            break;
          case 5:
            builder.AddParseValueAction(count, count, 'y', -99999, 99999, setter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(yearGetter(value), 5, sb)));
            break;
          default:
            throw new InvalidOperationException("Bug in Noda Time; invalid count for year went undetected.");
        }
      });
    }

    private static bool CheckIfNextCharacterMightBeDigit(PatternCursor pattern)
    {
      int index = pattern.Index;
      try
      {
        if (!pattern.MoveNext())
          return false;
        char current1 = pattern.Current;
        if (current1 >= '0' && current1 <= '9' || current1 >= 'a' && current1 <= 'z' || current1 >= 'A' && current1 <= 'Z')
          return true;
        switch (current1)
        {
          case '"':
          case '\'':
            try
            {
              string quotedString = pattern.GetQuotedString(current1);
              if (quotedString.Length == 0)
                return true;
              char ch = quotedString[0];
              return ch >= '0' && ch <= '9';
            }
            catch (InvalidPatternException ex)
            {
              return true;
            }
          case '%':
            return true;
          case '\\':
            if (!pattern.MoveNext())
              return true;
            char current2 = pattern.Current;
            return current2 >= '0' && current2 <= '9';
          default:
            return false;
        }
      }
      finally
      {
        pattern.Move(index);
      }
    }

    internal static CharacterHandler<TResult, TBucket> CreateMonthOfYearHandler<TResult, TBucket>(
      Func<TResult, int> numberGetter,
      Action<TBucket, int> textSetter,
      Action<TBucket, int> numberSetter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        int count = pattern.GetRepeatCount(4);
        PatternFields field;
        switch (count)
        {
          case 1:
          case 2:
            field = PatternFields.MonthOfYearNumeric;
            builder.AddParseValueAction(count, 2, pattern.Current, 0, 99, numberSetter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(numberGetter(value), count, sb)));
            break;
          case 3:
          case 4:
            field = PatternFields.MonthOfYearText;
            NodaFormatInfo formatInfo = builder.FormatInfo;
            IList<string> stringList = count == 3 ? formatInfo.ShortMonthNames : formatInfo.LongMonthNames;
            IList<string> textValues1 = count == 3 ? formatInfo.ShortMonthGenitiveNames : formatInfo.LongMonthGenitiveNames;
            if (stringList == textValues1)
              builder.AddParseLongestTextAction(pattern.Current, textSetter, formatInfo.CompareInfo, stringList);
            else
              builder.AddParseLongestTextAction(pattern.Current, textSetter, formatInfo.CompareInfo, textValues1, stringList);
            builder.AddFormatAction(new Action<TResult, StringBuilder>(new DatePatternHelper.MonthFormatActionHolder<TResult, TBucket>(formatInfo, count, numberGetter).DummyMethod));
            break;
          default:
            throw new InvalidOperationException("Invalid count!");
        }
        builder.AddField(field, pattern.Current);
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateDayHandler<TResult, TBucket>(
      Func<TResult, int> dayOfMonthGetter,
      Func<TResult, int> dayOfWeekGetter,
      Action<TBucket, int> dayOfMonthSetter,
      Action<TBucket, int> dayOfWeekSetter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        int count = pattern.GetRepeatCount(4);
        PatternFields field;
        switch (count)
        {
          case 1:
          case 2:
            field = PatternFields.DayOfMonth;
            builder.AddParseValueAction(count, 2, pattern.Current, 1, 99, dayOfMonthSetter);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(dayOfMonthGetter(value), count, sb)));
            break;
          case 3:
          case 4:
            field = PatternFields.DayOfWeek;
            NodaFormatInfo formatInfo = builder.FormatInfo;
            IList<string> textValues = count == 3 ? formatInfo.ShortDayNames : formatInfo.LongDayNames;
            builder.AddParseLongestTextAction(pattern.Current, dayOfWeekSetter, formatInfo.CompareInfo, textValues);
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(textValues[dayOfWeekGetter(value)])));
            break;
          default:
            throw new InvalidOperationException("Invalid count!");
        }
        builder.AddField(field, pattern.Current);
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateEraHandler<TResult, TBucket>(
      Func<TResult, Era> eraFromValue,
      Func<TBucket, LocalDatePatternParser.LocalDateParseBucket> dateBucketFromBucket)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        pattern.GetRepeatCount(2);
        builder.AddField(PatternFields.Era, pattern.Current);
        NodaFormatInfo formatInfo = builder.FormatInfo;
        builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((cursor, bucket) => dateBucketFromBucket(bucket).ParseEra<TResult>(formatInfo, cursor)));
        builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(formatInfo.GetEraPrimaryName(eraFromValue(value)))));
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateCalendarHandler<TResult, TBucket>(
      Func<TResult, CalendarSystem> getter,
      Action<TBucket, CalendarSystem> setter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        builder.AddField(PatternFields.Calendar, pattern.Current);
        builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((cursor, bucket) =>
        {
          foreach (string id in CalendarSystem.Ids)
          {
            if (cursor.MatchCaseInsensitive(id, NodaFormatInfo.InvariantInfo.CompareInfo, true))
            {
              setter(bucket, CalendarSystem.ForId(id));
              return (ParseResult<TResult>) null;
            }
          }
          return ParseResult<TResult>.NoMatchingCalendarSystem(cursor);
        }));
        builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(getter(value).Id)));
      });
    }

    private sealed class MonthFormatActionHolder<TResult, TBucket> : 
      SteppedPatternBuilder<TResult, TBucket>.IPostPatternParseFormatAction
      where TBucket : ParseBucket<TResult>
    {
      private readonly int count;
      private readonly NodaFormatInfo formatInfo;
      private readonly Func<TResult, int> getter;

      internal MonthFormatActionHolder(
        NodaFormatInfo formatInfo,
        int count,
        Func<TResult, int> getter)
      {
        this.count = count;
        this.formatInfo = formatInfo;
        this.getter = getter;
      }

      internal void DummyMethod(TResult value, StringBuilder builder) => throw new InvalidOperationException("This method should never be called");

      public Action<TResult, StringBuilder> BuildFormatAction(
        PatternFields finalFields)
      {
        bool flag = (finalFields & PatternFields.DayOfMonth) != PatternFields.None;
        IList<string> textValues = this.count == 3 ? (flag ? this.formatInfo.ShortMonthGenitiveNames : this.formatInfo.ShortMonthNames) : (flag ? this.formatInfo.LongMonthGenitiveNames : this.formatInfo.LongMonthNames);
        return (Action<TResult, StringBuilder>) ((value, sb) => sb.Append(textValues[this.getter(value)]));
      }
    }
  }
}
