// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.TimePatternHelper
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Globalization;
using System.Text;

namespace NodaTime.Text.Patterns
{
  internal static class TimePatternHelper
  {
    internal static CharacterHandler<TResult, TBucket> CreatePeriodHandler<TResult, TBucket>(
      int maxCount,
      Func<TResult, int> getter,
      Action<TBucket, int> setter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        if (pattern.PeekNext() == 'F')
        {
          pattern.MoveNext();
          int count = pattern.GetRepeatCount(maxCount);
          builder.AddField(PatternFields.FractionalSeconds, pattern.Current);
          builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((valueCursor, bucket) =>
          {
            if (!valueCursor.Match('.'))
              return (ParseResult<TResult>) null;
            int result;
            if (!valueCursor.ParseFraction(count, maxCount, out result, false))
              return ParseResult<TResult>.MismatchedNumber(valueCursor, new string('F', count));
            setter(bucket, result);
            return (ParseResult<TResult>) null;
          }));
          builder.AddFormatAction((Action<TResult, StringBuilder>) ((localTime, sb) => sb.Append('.')));
          builder.AddFormatFractionTruncate(count, maxCount, getter);
        }
        else
          builder.AddLiteral('.', new Func<ValueCursor, char, ParseResult<TResult>>(ParseResult<TResult>.MismatchedCharacter));
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateCommaDotHandler<TResult, TBucket>(
      int maxCount,
      Func<TResult, int> getter,
      Action<TBucket, int> setter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        if (pattern.PeekNext() == 'F')
        {
          pattern.MoveNext();
          int count = pattern.GetRepeatCount(maxCount);
          builder.AddField(PatternFields.FractionalSeconds, pattern.Current);
          builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((valueCursor, bucket) =>
          {
            if (!valueCursor.Match('.') && !valueCursor.Match(','))
              return (ParseResult<TResult>) null;
            int result;
            if (!valueCursor.ParseFraction(count, maxCount, out result, false))
              return ParseResult<TResult>.MismatchedNumber(valueCursor, new string('F', count));
            setter(bucket, result);
            return (ParseResult<TResult>) null;
          }));
          builder.AddFormatAction((Action<TResult, StringBuilder>) ((localTime, sb) => sb.Append('.')));
          builder.AddFormatFractionTruncate(count, maxCount, getter);
        }
        else
        {
          builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) => !str.Match('.') && !str.Match(',') ? ParseResult<TResult>.MismatchedCharacter(str, ';') : (ParseResult<TResult>) null));
          builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append('.')));
        }
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateFractionHandler<TResult, TBucket>(
      int maxCount,
      Func<TResult, int> getter,
      Action<TBucket, int> setter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        char patternCharacter = pattern.Current;
        int count = pattern.GetRepeatCount(maxCount);
        builder.AddField(PatternFields.FractionalSeconds, pattern.Current);
        builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
        {
          int result;
          if (!str.ParseFraction(count, maxCount, out result, patternCharacter == 'f'))
            return ParseResult<TResult>.MismatchedNumber(str, new string(patternCharacter, count));
          setter(bucket, result);
          return (ParseResult<TResult>) null;
        }));
        if (patternCharacter == 'f')
          builder.AddFormatFraction(count, maxCount, getter);
        else
          builder.AddFormatFractionTruncate(count, maxCount, getter);
      });
    }

    internal static CharacterHandler<TResult, TBucket> CreateAmPmHandler<TResult, TBucket>(
      Func<TResult, int> hourOfDayGetter,
      Action<TBucket, int> amPmSetter)
      where TBucket : ParseBucket<TResult>
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        int repeatCount = pattern.GetRepeatCount(2);
        builder.AddField(PatternFields.AmPm, pattern.Current);
        string amDesignator = builder.FormatInfo.AMDesignator;
        string pmDesignator = builder.FormatInfo.PMDesignator;
        if (amDesignator == "" && pmDesignator == "")
          builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
          {
            amPmSetter(bucket, 2);
            return (ParseResult<TResult>) null;
          }));
        else if (amDesignator == "" || pmDesignator == "")
        {
          int specifiedDesignatorValue = amDesignator == "" ? 1 : 0;
          string specifiedDesignator = specifiedDesignatorValue == 1 ? pmDesignator : amDesignator;
          TimePatternHelper.HandleHalfAmPmDesignator<TResult, TBucket>(repeatCount, specifiedDesignator, specifiedDesignatorValue, hourOfDayGetter, amPmSetter, builder);
        }
        else
        {
          CompareInfo compareInfo = builder.FormatInfo.CompareInfo;
          if (repeatCount == 1)
          {
            string amFirst = amDesignator.Substring(0, 1);
            string pmFirst = pmDesignator.Substring(0, 1);
            builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
            {
              if (str.MatchCaseInsensitive(amFirst, compareInfo, true))
              {
                amPmSetter(bucket, 0);
                return (ParseResult<TResult>) null;
              }
              if (!str.MatchCaseInsensitive(pmFirst, compareInfo, true))
                return ParseResult<TResult>.MissingAmPmDesignator(str);
              amPmSetter(bucket, 1);
              return (ParseResult<TResult>) null;
            }));
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(hourOfDayGetter(value) > 11 ? pmDesignator[0] : amDesignator[0])));
          }
          else
          {
            builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
            {
              bool flag = pmDesignator.Length > amDesignator.Length;
              string match13 = flag ? pmDesignator : amDesignator;
              string match14 = flag ? amDesignator : pmDesignator;
              int num = flag ? 1 : 0;
              if (str.MatchCaseInsensitive(match13, compareInfo, true))
              {
                amPmSetter(bucket, num);
                return (ParseResult<TResult>) null;
              }
              if (!str.MatchCaseInsensitive(match14, compareInfo, true))
                return ParseResult<TResult>.MissingAmPmDesignator(str);
              amPmSetter(bucket, checked (1 - num));
              return (ParseResult<TResult>) null;
            }));
            builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(hourOfDayGetter(value) > 11 ? pmDesignator : amDesignator)));
          }
        }
      });
    }

    private static void HandleHalfAmPmDesignator<TResult, TBucket>(
      int count,
      string specifiedDesignator,
      int specifiedDesignatorValue,
      Func<TResult, int> hourOfDayGetter,
      Action<TBucket, int> amPmSetter,
      SteppedPatternBuilder<TResult, TBucket> builder)
      where TBucket : ParseBucket<TResult>
    {
      CompareInfo compareInfo = builder.FormatInfo.CompareInfo;
      if (count == 1)
      {
        string abbreviation = specifiedDesignator.Substring(0, 1);
        builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
        {
          int num = str.MatchCaseInsensitive(abbreviation, compareInfo, true) ? specifiedDesignatorValue : checked (1 - specifiedDesignatorValue);
          amPmSetter(bucket, num);
          return (ParseResult<TResult>) null;
        }));
        builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) =>
        {
          if (hourOfDayGetter(value) / 12 != specifiedDesignatorValue)
            return;
          sb.Append(specifiedDesignator[0]);
        }));
      }
      else
      {
        builder.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
        {
          int num = str.MatchCaseInsensitive(specifiedDesignator, compareInfo, true) ? specifiedDesignatorValue : checked (1 - specifiedDesignatorValue);
          amPmSetter(bucket, num);
          return (ParseResult<TResult>) null;
        }));
        builder.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) =>
        {
          if (hourOfDayGetter(value) / 12 != specifiedDesignatorValue)
            return;
          sb.Append(specifiedDesignator);
        }));
      }
    }
  }
}
