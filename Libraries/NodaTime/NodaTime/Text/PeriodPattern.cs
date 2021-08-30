// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.PeriodPattern
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Properties;
using NodaTime.Utility;
using System.Text;

namespace NodaTime.Text
{
  [Immutable]
  public sealed class PeriodPattern : IPattern<Period>
  {
    public static readonly PeriodPattern RoundtripPattern = new PeriodPattern((IPattern<Period>) new PeriodPattern.RoundtripPatternImpl());
    public static readonly PeriodPattern NormalizingIsoPattern = new PeriodPattern((IPattern<Period>) new PeriodPattern.NormalizingIsoPatternImpl());
    private readonly IPattern<Period> pattern;

    private PeriodPattern(IPattern<Period> pattern) => this.pattern = Preconditions.CheckNotNull<IPattern<Period>>(pattern, nameof (pattern));

    public ParseResult<Period> Parse(string text) => this.pattern.Parse(text);

    public string Format(Period value) => this.pattern.Format(value);

    private static void AppendValue(StringBuilder builder, long value, string suffix)
    {
      if (value == 0L)
        return;
      FormatHelper.FormatInvariant(value, builder);
      builder.Append(suffix);
    }

    private static ParseResult<Period> InvalidUnit(
      ValueCursor cursor,
      char unitCharacter)
    {
      return ParseResult<Period>.ForInvalidValue(cursor, Messages.Parse_InvalidUnitSpecifier, (object) unitCharacter);
    }

    private static ParseResult<Period> RepeatedUnit(
      ValueCursor cursor,
      char unitCharacter)
    {
      return ParseResult<Period>.ForInvalidValue(cursor, Messages.Parse_RepeatedUnitSpecifier, (object) unitCharacter);
    }

    private static ParseResult<Period> MisplacedUnit(
      ValueCursor cursor,
      char unitCharacter)
    {
      return ParseResult<Period>.ForInvalidValue(cursor, Messages.Parse_MisplacedUnitSpecifier, (object) unitCharacter);
    }

    private sealed class RoundtripPatternImpl : IPattern<Period>
    {
      public ParseResult<Period> Parse(string text)
      {
        switch (text)
        {
          case "":
            return ParseResult<Period>.ValueStringEmpty;
          case null:
            return ParseResult<Period>.ArgumentNull(nameof (text));
          default:
            ValueCursor cursor = new ValueCursor(text);
            cursor.MoveNext();
            if (cursor.Current != 'P')
              return ParseResult<Period>.MismatchedCharacter(cursor, 'P');
            bool flag = true;
            PeriodBuilder periodBuilder = new PeriodBuilder();
            PeriodUnits periodUnits = PeriodUnits.None;
            while (cursor.MoveNext())
            {
              if (flag && cursor.Current == 'T')
              {
                flag = false;
              }
              else
              {
                long result;
                ParseResult<Period> int64 = cursor.ParseInt64<Period>(out result);
                if (int64 != null)
                  return int64;
                if (cursor.Length == cursor.Index)
                  return ParseResult<Period>.EndOfString(cursor);
                PeriodUnits unit;
                switch (cursor.Current)
                {
                  case 'D':
                    unit = PeriodUnits.Days;
                    break;
                  case 'H':
                    unit = PeriodUnits.Hours;
                    break;
                  case 'M':
                    unit = flag ? PeriodUnits.Months : PeriodUnits.Minutes;
                    break;
                  case 'S':
                    unit = PeriodUnits.Seconds;
                    break;
                  case 'W':
                    unit = PeriodUnits.Weeks;
                    break;
                  case 'Y':
                    unit = PeriodUnits.Years;
                    break;
                  case 's':
                    unit = PeriodUnits.Milliseconds;
                    break;
                  case 't':
                    unit = PeriodUnits.Ticks;
                    break;
                  default:
                    return PeriodPattern.InvalidUnit(cursor, cursor.Current);
                }
                if ((unit & periodUnits) != PeriodUnits.None)
                  return PeriodPattern.RepeatedUnit(cursor, cursor.Current);
                if (unit < periodUnits)
                  return PeriodPattern.MisplacedUnit(cursor, cursor.Current);
                if ((unit & PeriodUnits.AllTimeUnits) == PeriodUnits.None != flag)
                  return PeriodPattern.MisplacedUnit(cursor, cursor.Current);
                periodBuilder[unit] = result;
                periodUnits |= unit;
              }
            }
            return ParseResult<Period>.ForValue(periodBuilder.Build());
        }
      }

      public string Format(Period value)
      {
        Preconditions.CheckNotNull<Period>(value, nameof (value));
        StringBuilder builder = new StringBuilder("P");
        PeriodPattern.AppendValue(builder, value.Years, "Y");
        PeriodPattern.AppendValue(builder, value.Months, "M");
        PeriodPattern.AppendValue(builder, value.Weeks, "W");
        PeriodPattern.AppendValue(builder, value.Days, "D");
        if (value.HasTimeComponent)
        {
          builder.Append("T");
          PeriodPattern.AppendValue(builder, value.Hours, "H");
          PeriodPattern.AppendValue(builder, value.Minutes, "M");
          PeriodPattern.AppendValue(builder, value.Seconds, "S");
          PeriodPattern.AppendValue(builder, value.Milliseconds, "s");
          PeriodPattern.AppendValue(builder, value.Ticks, "t");
        }
        return builder.ToString();
      }
    }

    private sealed class NormalizingIsoPatternImpl : IPattern<Period>
    {
      public ParseResult<Period> Parse(string text)
      {
        switch (text)
        {
          case "":
            return ParseResult<Period>.ValueStringEmpty;
          case null:
            return ParseResult<Period>.ArgumentNull(nameof (text));
          default:
            ValueCursor cursor = new ValueCursor(text);
            cursor.MoveNext();
            if (cursor.Current != 'P')
              return ParseResult<Period>.MismatchedCharacter(cursor, 'P');
            bool flag1 = true;
            PeriodBuilder periodBuilder = new PeriodBuilder();
            PeriodUnits periodUnits = PeriodUnits.None;
            while (cursor.MoveNext())
            {
              if (flag1 && cursor.Current == 'T')
              {
                flag1 = false;
              }
              else
              {
                bool flag2 = cursor.Current == '-';
                long result1;
                ParseResult<Period> int64 = cursor.ParseInt64<Period>(out result1);
                if (int64 != null)
                  return int64;
                if (cursor.Length == cursor.Index)
                  return ParseResult<Period>.EndOfString(cursor);
                PeriodUnits unit;
                switch (cursor.Current)
                {
                  case ',':
                  case '.':
                    unit = PeriodUnits.Ticks;
                    break;
                  case 'D':
                    unit = PeriodUnits.Days;
                    break;
                  case 'H':
                    unit = PeriodUnits.Hours;
                    break;
                  case 'M':
                    unit = flag1 ? PeriodUnits.Months : PeriodUnits.Minutes;
                    break;
                  case 'S':
                    unit = PeriodUnits.Seconds;
                    break;
                  case 'W':
                    unit = PeriodUnits.Weeks;
                    break;
                  case 'Y':
                    unit = PeriodUnits.Years;
                    break;
                  default:
                    return PeriodPattern.InvalidUnit(cursor, cursor.Current);
                }
                if ((unit & periodUnits) != PeriodUnits.None)
                  return PeriodPattern.RepeatedUnit(cursor, cursor.Current);
                if (unit < periodUnits)
                  return PeriodPattern.MisplacedUnit(cursor, cursor.Current);
                if ((unit & PeriodUnits.AllTimeUnits) == PeriodUnits.None != flag1)
                  return PeriodPattern.MisplacedUnit(cursor, cursor.Current);
                if (unit == PeriodUnits.Ticks)
                {
                  if ((periodUnits & PeriodUnits.Seconds) != PeriodUnits.None)
                    return PeriodPattern.MisplacedUnit(cursor, cursor.Current);
                  periodBuilder.Seconds = result1;
                  int result2;
                  if (!cursor.MoveNext() || !cursor.ParseFraction(7, 7, out result2, false))
                    return ParseResult<Period>.MissingNumber(cursor);
                  if (flag2)
                    result2 = checked (-result2);
                  periodBuilder.Milliseconds = (long) result2 / 10000L % 1000L;
                  periodBuilder.Ticks = (long) result2 % 10000L;
                  if (cursor.Current != 'S')
                    return ParseResult<Period>.MismatchedCharacter(cursor, 'S');
                  return cursor.MoveNext() ? ParseResult<Period>.ExpectedEndOfString(cursor) : ParseResult<Period>.ForValue(periodBuilder.Build());
                }
                periodBuilder[unit] = result1;
                periodUnits |= unit;
              }
            }
            return periodUnits == PeriodUnits.None ? ParseResult<Period>.ForInvalidValue(cursor, Messages.Parse_EmptyPeriod) : ParseResult<Period>.ForValue(periodBuilder.Build());
        }
      }

      public string Format(Period value)
      {
        Preconditions.CheckNotNull<Period>(value, nameof (value));
        value = value.Normalize();
        if (value.Equals(Period.Zero))
          return "P0D";
        StringBuilder stringBuilder = new StringBuilder("P");
        PeriodPattern.AppendValue(stringBuilder, value.Years, "Y");
        PeriodPattern.AppendValue(stringBuilder, value.Months, "M");
        PeriodPattern.AppendValue(stringBuilder, value.Weeks, "W");
        PeriodPattern.AppendValue(stringBuilder, value.Days, "D");
        if (value.HasTimeComponent)
        {
          stringBuilder.Append("T");
          PeriodPattern.AppendValue(stringBuilder, value.Hours, "H");
          PeriodPattern.AppendValue(stringBuilder, value.Minutes, "M");
          long num1 = checked (value.Milliseconds * 10000L + value.Ticks);
          long num2 = value.Seconds;
          if (num1 != 0L || num2 != 0L)
          {
            if (num1 < 0L || num2 < 0L)
            {
              stringBuilder.Append("-");
              num1 = checked (-num1);
              num2 = checked (-num2);
            }
            FormatHelper.FormatInvariant(num2, stringBuilder);
            if (num1 != 0L)
            {
              stringBuilder.Append(".");
              FormatHelper.AppendFractionTruncate(checked ((int) num1), 7, 7, stringBuilder);
            }
            stringBuilder.Append("S");
          }
        }
        return stringBuilder.ToString();
      }
    }
  }
}
