// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.ZonedDateTimePatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NodaTime.Text
{
  internal sealed class ZonedDateTimePatternParser : IPatternParser<ZonedDateTime>
  {
    private readonly LocalDate templateValueDate;
    private readonly LocalTime templateValueTime;
    private readonly DateTimeZone templateValueZone;
    private readonly IDateTimeZoneProvider zoneProvider;
    private readonly ZoneLocalMappingResolver resolver;
    private static readonly Dictionary<char, CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandleBackslash)
      },
      {
        '/',
        (CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.DateSeparator, new Func<ValueCursor, ParseResult<ZonedDateTime>>(ParseResult<ZonedDateTime>.DateSeparatorMismatch)))
      },
      {
        'y',
        DatePatternHelper.CreateYearHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, int>) (value => value.YearOfCentury), (Func<ZonedDateTime, int>) (value => value.Year), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.Year = value))
      },
      {
        'Y',
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePaddedField(5, PatternFields.YearOfEra, 0, 99999, (Func<ZonedDateTime, int>) (value => value.YearOfEra), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.YearOfEra = value))
      },
      {
        'M',
        DatePatternHelper.CreateMonthOfYearHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, int>) (value => value.Month), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearText = value), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.MonthOfYearNumeric = value))
      },
      {
        'd',
        DatePatternHelper.CreateDayHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, int>) (value => value.Day), (Func<ZonedDateTime, int>) (value => value.DayOfWeek), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfMonth = value), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Date.DayOfWeek = value))
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(7, (Func<ZonedDateTime, int>) (value => value.TickOfSecond), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ';',
        TimePatternHelper.CreateCommaDotHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(7, (Func<ZonedDateTime, int>) (value => value.TickOfSecond), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        ':',
        (CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<ZonedDateTime>>(ParseResult<ZonedDateTime>.TimeSeparatorMismatch)))
      },
      {
        'h',
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours12, 1, 12, (Func<ZonedDateTime, int>) (value => value.ClockHourOfHalfDay), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours12 = value))
      },
      {
        'H',
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Hours24, 0, 24, (Func<ZonedDateTime, int>) (value => value.Hour), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Hours24 = value))
      },
      {
        'm',
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Minutes, 0, 59, (Func<ZonedDateTime, int>) (value => value.Minute), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Minutes = value))
      },
      {
        's',
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.HandlePaddedField(2, PatternFields.Seconds, 0, 59, (Func<ZonedDateTime, int>) (value => value.Second), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.Seconds = value))
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(7, (Func<ZonedDateTime, int>) (value => value.TickOfSecond), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(7, (Func<ZonedDateTime, int>) (value => value.TickOfSecond), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.FractionalSeconds = value))
      },
      {
        't',
        TimePatternHelper.CreateAmPmHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, int>) (time => time.Hour), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, int>) ((bucket, value) => bucket.Time.AmPm = value))
      },
      {
        'c',
        DatePatternHelper.CreateCalendarHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, CalendarSystem>) (value => value.LocalDateTime.Calendar), (Action<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, CalendarSystem>) ((bucket, value) => bucket.Date.Calendar = value))
      },
      {
        'g',
        DatePatternHelper.CreateEraHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>((Func<ZonedDateTime, Era>) (value => value.Era), (Func<ZonedDateTimePatternParser.ZonedDateTimeParseBucket, LocalDatePatternParser.LocalDateParseBucket>) (bucket => bucket.Date))
      },
      {
        'z',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(ZonedDateTimePatternParser.HandleZone)
      },
      {
        'x',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(ZonedDateTimePatternParser.HandleZoneAbbreviation)
      },
      {
        'o',
        new CharacterHandler<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(ZonedDateTimePatternParser.HandleOffset)
      }
    };

    internal ZonedDateTimePatternParser(
      ZonedDateTime templateValue,
      ZoneLocalMappingResolver resolver,
      IDateTimeZoneProvider zoneProvider)
    {
      this.templateValueDate = templateValue.Date;
      this.templateValueTime = templateValue.TimeOfDay;
      this.templateValueZone = templateValue.Zone;
      this.resolver = resolver;
      this.zoneProvider = zoneProvider;
    }

    public IPattern<ZonedDateTime> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      if (patternText.Length == 0)
        throw new InvalidPatternException(Messages.Parse_FormatStringEmpty);
      if (patternText.Length == 1)
      {
        switch (patternText[0])
        {
          case 'F':
            return (IPattern<ZonedDateTime>) ZonedDateTimePattern.Patterns.ExtendedFormatOnlyPatternImpl.WithZoneProvider(this.zoneProvider).WithResolver(this.resolver);
          case 'G':
            return (IPattern<ZonedDateTime>) ZonedDateTimePattern.Patterns.GeneralFormatOnlyPatternImpl.WithZoneProvider(this.zoneProvider).WithResolver(this.resolver);
          default:
            throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
            {
              (object) patternText[0],
              (object) typeof (ZonedDateTime)
            });
        }
      }
      else
      {
        SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>(formatInfo, (Func<ZonedDateTimePatternParser.ZonedDateTimeParseBucket>) (() => new ZonedDateTimePatternParser.ZonedDateTimeParseBucket(this.templateValueDate, this.templateValueTime, this.templateValueZone, this.resolver, this.zoneProvider)));
        if (this.zoneProvider == null)
          steppedPatternBuilder.SetFormatOnly();
        steppedPatternBuilder.ParseCustomPattern(patternText, ZonedDateTimePatternParser.PatternCharacterHandlers);
        steppedPatternBuilder.ValidateUsedFields();
        return (IPattern<ZonedDateTime>) steppedPatternBuilder.Build();
      }
    }

    private static void HandleZone(
      PatternCursor pattern,
      SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket> builder)
    {
      builder.AddField(PatternFields.Zone, pattern.Current);
      builder.AddParseAction(new SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.ParseAction(ZonedDateTimePatternParser.ParseZone));
      builder.AddFormatAction((Action<ZonedDateTime, StringBuilder>) ((value, sb) => sb.Append(value.Zone.Id)));
    }

    private static void HandleZoneAbbreviation(
      PatternCursor pattern,
      SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket> builder)
    {
      builder.AddField(PatternFields.ZoneAbbreviation, pattern.Current);
      builder.SetFormatOnly();
      builder.AddFormatAction((Action<ZonedDateTime, StringBuilder>) ((value, sb) => sb.Append(value.GetZoneInterval().Name)));
    }

    private static void HandleOffset(
      PatternCursor pattern,
      SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket> builder)
    {
      builder.AddField(PatternFields.EmbeddedOffset, pattern.Current);
      IPartialPattern<Offset> offsetPattern = OffsetPattern.Create(pattern.GetEmbeddedPattern('<', '>'), builder.FormatInfo).UnderlyingPattern;
      builder.AddParseAction((SteppedPatternBuilder<ZonedDateTime, ZonedDateTimePatternParser.ZonedDateTimeParseBucket>.ParseAction) ((value, bucket) =>
      {
        ParseResult<Offset> partial = offsetPattern.ParsePartial(value);
        if (!partial.Success)
          return partial.ConvertError<ZonedDateTime>();
        bucket.Offset = partial.Value;
        return (ParseResult<ZonedDateTime>) null;
      }));
      builder.AddFormatAction((Action<ZonedDateTime, StringBuilder>) ((value, sb) => offsetPattern.FormatPartial(value.Offset, sb)));
    }

    private static ParseResult<ZonedDateTime> ParseZone(
      ValueCursor value,
      ZonedDateTimePatternParser.ZonedDateTimeParseBucket bucket)
    {
      return bucket.ParseZone(value);
    }

    private sealed class ZonedDateTimeParseBucket : ParseBucket<ZonedDateTime>
    {
      internal readonly LocalDatePatternParser.LocalDateParseBucket Date;
      internal readonly LocalTimePatternParser.LocalTimeParseBucket Time;
      private DateTimeZone Zone;
      internal Offset Offset;
      private readonly ZoneLocalMappingResolver resolver;
      private readonly IDateTimeZoneProvider zoneProvider;

      internal ZonedDateTimeParseBucket(
        LocalDate templateDate,
        LocalTime templateTime,
        DateTimeZone templateZone,
        ZoneLocalMappingResolver resolver,
        IDateTimeZoneProvider zoneProvider)
      {
        this.Date = new LocalDatePatternParser.LocalDateParseBucket(templateDate);
        this.Time = new LocalTimePatternParser.LocalTimeParseBucket(templateTime);
        this.Zone = templateZone;
        this.resolver = resolver;
        this.zoneProvider = zoneProvider;
      }

      internal ParseResult<ZonedDateTime> ParseZone(ValueCursor value)
      {
        DateTimeZone dateTimeZone = this.TryParseFixedZone(value) ?? this.TryParseProviderZone(value);
        if (dateTimeZone == null)
          return ParseResult<ZonedDateTime>.NoMatchingZoneId(value);
        this.Zone = dateTimeZone;
        return (ParseResult<ZonedDateTime>) null;
      }

      private DateTimeZone TryParseFixedZone(ValueCursor value)
      {
        if (value.CompareOrdinal("UTC") != 0)
          return (DateTimeZone) null;
        value.Move(checked (value.Index + 3));
        ParseResult<Offset> partial = OffsetPattern.GeneralInvariantPattern.UnderlyingPattern.ParsePartial(value);
        return !partial.Success ? DateTimeZone.Utc : DateTimeZone.ForOffset(partial.Value);
      }

      private DateTimeZone TryParseProviderZone(ValueCursor value)
      {
        ReadOnlyCollection<string> ids = this.zoneProvider.Ids;
        int num1 = 0;
        int num2 = ids.Count;
        while (num1 < num2)
        {
          int index = checked (num1 + num2) / 2;
          int num3 = value.CompareOrdinal(ids[index]);
          if (num3 < 0)
            num2 = index;
          else if (num3 > 0)
          {
            num1 = checked (index + 1);
          }
          else
          {
            while (checked (index + 1) < num2 && value.CompareOrdinal(ids[checked (index + 1)]) == 0)
              checked { ++index; }
            string id = ids[index];
            value.Move(checked (value.Index + id.Length));
            return this.zoneProvider[id];
          }
        }
        return (DateTimeZone) null;
      }

      internal override ParseResult<ZonedDateTime> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        ParseResult<LocalDateTime> parseResult = LocalDateTimePatternParser.LocalDateTimeParseBucket.CombineBuckets(usedFields, this.Date, this.Time, text);
        if (!parseResult.Success)
          return parseResult.ConvertError<ZonedDateTime>();
        LocalDateTime localDateTime = parseResult.Value;
        if ((usedFields & PatternFields.EmbeddedOffset) == PatternFields.None)
        {
          try
          {
            return ParseResult<ZonedDateTime>.ForValue(this.Zone.ResolveLocal(localDateTime, this.resolver));
          }
          catch (SkippedTimeException ex)
          {
            return ParseResult<ZonedDateTime>.SkippedLocalTime(text);
          }
          catch (AmbiguousTimeException ex)
          {
            return ParseResult<ZonedDateTime>.AmbiguousLocalTime(text);
          }
        }
        else
        {
          ZoneLocalMapping zoneLocalMapping = this.Zone.MapLocal(localDateTime);
          ZonedDateTime zonedDateTime;
          switch (zoneLocalMapping.Count)
          {
            case 0:
              return ParseResult<ZonedDateTime>.InvalidOffset(text);
            case 1:
              zonedDateTime = zoneLocalMapping.First();
              break;
            case 2:
              zonedDateTime = zoneLocalMapping.First().Offset == this.Offset ? zoneLocalMapping.First() : zoneLocalMapping.Last();
              break;
            default:
              throw new InvalidOperationException("Mapping has count outside range 0-2; should not happen.");
          }
          return zonedDateTime.Offset != this.Offset ? ParseResult<ZonedDateTime>.InvalidOffset(text) : ParseResult<ZonedDateTime>.ForValue(zonedDateTime);
        }
      }
    }
  }
}
