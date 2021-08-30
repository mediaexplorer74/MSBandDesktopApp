// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.OffsetPatternParser
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Properties;
using NodaTime.Text.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace NodaTime.Text
{
  internal sealed class OffsetPatternParser : IPatternParser<Offset>
  {
    private static readonly Dictionary<char, CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>> PatternCharacterHandlers = new Dictionary<char, CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>>()
    {
      {
        '%',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandlePercent)
      },
      {
        '\'',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandleQuote)
      },
      {
        '"',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandleQuote)
      },
      {
        '\\',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandleBackslash)
      },
      {
        '.',
        TimePatternHelper.CreatePeriodHandler<Offset, OffsetPatternParser.OffsetParseBucket>(3, new Func<Offset, int>(OffsetPatternParser.GetPositiveMilliseconds), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Milliseconds = value))
      },
      {
        ':',
        (CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>) ((pattern, builder) => builder.AddLiteral(builder.FormatInfo.TimeSeparator, new Func<ValueCursor, ParseResult<Offset>>(ParseResult<Offset>.TimeSeparatorMismatch)))
      },
      {
        'h',
        (CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>) ((pattern, builder) =>
        {
          throw new InvalidPatternException(Messages.Parse_Hour12PatternNotSupported, new object[1]
          {
            (object) typeof (Offset)
          });
        })
      },
      {
        'H',
        SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandlePaddedField(2, PatternFields.Hours24, 0, 23, new Func<Offset, int>(OffsetPatternParser.GetPositiveHours), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Hours = value))
      },
      {
        'm',
        SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandlePaddedField(2, PatternFields.Minutes, 0, 59, new Func<Offset, int>(OffsetPatternParser.GetPositiveMinutes), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Minutes = value))
      },
      {
        's',
        SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>.HandlePaddedField(2, PatternFields.Seconds, 0, 59, new Func<Offset, int>(OffsetPatternParser.GetPositiveSeconds), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Seconds = value))
      },
      {
        '+',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(OffsetPatternParser.HandlePlus)
      },
      {
        '-',
        new CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>(OffsetPatternParser.HandleMinus)
      },
      {
        'f',
        TimePatternHelper.CreateFractionHandler<Offset, OffsetPatternParser.OffsetParseBucket>(3, new Func<Offset, int>(OffsetPatternParser.GetPositiveMilliseconds), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Milliseconds = value))
      },
      {
        'F',
        TimePatternHelper.CreateFractionHandler<Offset, OffsetPatternParser.OffsetParseBucket>(3, new Func<Offset, int>(OffsetPatternParser.GetPositiveMilliseconds), (Action<OffsetPatternParser.OffsetParseBucket, int>) ((bucket, value) => bucket.Milliseconds = value))
      },
      {
        'Z',
        (CharacterHandler<Offset, OffsetPatternParser.OffsetParseBucket>) ((ignored1, ignored2) =>
        {
          throw new InvalidPatternException(Messages.Parse_ZPrefixNotAtStartOfPattern);
        })
      }
    };

    private static int GetPositiveHours(Offset offset) => Math.Abs(offset.Milliseconds) / 3600000;

    private static int GetPositiveMinutes(Offset offset) => Math.Abs(offset.Milliseconds) % 3600000 / 60000;

    private static int GetPositiveSeconds(Offset offset) => Math.Abs(offset.Milliseconds) % 60000 / 1000;

    private static int GetPositiveMilliseconds(Offset offset) => Math.Abs(offset.Milliseconds) % 1000;

    public IPattern<Offset> ParsePattern(
      string patternText,
      NodaFormatInfo formatInfo)
    {
      return (IPattern<Offset>) this.ParsePartialPattern(patternText, formatInfo);
    }

    private IPartialPattern<Offset> ParsePartialPattern(
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
          case 'G':
            return (IPartialPattern<Offset>) new OffsetPatternParser.ZPrefixPattern(this.CreateGeneralPattern(formatInfo));
          case 'g':
            return this.CreateGeneralPattern(formatInfo);
          case 'n':
            return (IPartialPattern<Offset>) new OffsetPatternParser.NumberPattern(formatInfo);
          default:
            patternText = this.ExpandStandardFormatPattern(patternCharacter, formatInfo);
            if (patternText == null)
              throw new InvalidPatternException(Messages.Parse_UnknownStandardFormat, new object[2]
              {
                (object) patternCharacter,
                (object) typeof (Offset)
              });
            break;
        }
      }
      bool flag = !(patternText == "%Z") ? patternText.StartsWith("Z") : throw new InvalidPatternException(Messages.Parse_EmptyZPrefixedOffsetPattern);
      SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket> steppedPatternBuilder = new SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket>(formatInfo, (Func<OffsetPatternParser.OffsetParseBucket>) (() => new OffsetPatternParser.OffsetParseBucket()));
      steppedPatternBuilder.ParseCustomPattern(flag ? patternText.Substring(1) : patternText, OffsetPatternParser.PatternCharacterHandlers);
      IPartialPattern<Offset> fullPattern = steppedPatternBuilder.Build();
      return !flag ? fullPattern : (IPartialPattern<Offset>) new OffsetPatternParser.ZPrefixPattern(fullPattern);
    }

    private string ExpandStandardFormatPattern(char patternCharacter, NodaFormatInfo formatInfo)
    {
      switch (patternCharacter)
      {
        case 'f':
          return formatInfo.OffsetPatternFull;
        case 'l':
          return formatInfo.OffsetPatternLong;
        case 'm':
          return formatInfo.OffsetPatternMedium;
        case 's':
          return formatInfo.OffsetPatternShort;
        default:
          return (string) null;
      }
    }

    private IPartialPattern<Offset> CreateGeneralPattern(
      NodaFormatInfo formatInfo)
    {
      List<IPartialPattern<Offset>> patterns = new List<IPartialPattern<Offset>>();
      foreach (char ch in "flms")
        patterns.Add(this.ParsePartialPattern(ch.ToString(), formatInfo));
      Func<Offset, IPartialPattern<Offset>> formatPatternPicker = (Func<Offset, IPartialPattern<Offset>>) (value => OffsetPatternParser.PickGeneralFormatter(value, patterns));
      return (IPartialPattern<Offset>) new CompositePattern<Offset>((IEnumerable<IPartialPattern<Offset>>) patterns, formatPatternPicker);
    }

    private static IPartialPattern<Offset> PickGeneralFormatter(
      Offset value,
      List<IPartialPattern<Offset>> patterns)
    {
      int num = Math.Abs(value.Milliseconds);
      int index = num % 1000 == 0 ? (num % 60000 / 1000 == 0 ? (num % 3600000 / 60000 == 0 ? 3 : 2) : 1) : 0;
      return patterns[index];
    }

    private static void HandlePlus(
      PatternCursor pattern,
      SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket> builder)
    {
      builder.AddField(PatternFields.Sign, pattern.Current);
      builder.AddRequiredSign((Action<OffsetPatternParser.OffsetParseBucket, bool>) ((bucket, positive) => bucket.IsNegative = !positive), (Func<Offset, bool>) (offset => offset.Milliseconds >= 0));
    }

    private static void HandleMinus(
      PatternCursor pattern,
      SteppedPatternBuilder<Offset, OffsetPatternParser.OffsetParseBucket> builder)
    {
      builder.AddField(PatternFields.Sign, pattern.Current);
      builder.AddNegativeOnlySign((Action<OffsetPatternParser.OffsetParseBucket, bool>) ((bucket, positive) => bucket.IsNegative = !positive), (Func<Offset, bool>) (offset => offset.Milliseconds >= 0));
    }

    private sealed class ZPrefixPattern : IPartialPattern<Offset>, IPattern<Offset>
    {
      private readonly IPartialPattern<Offset> fullPattern;

      internal ZPrefixPattern(IPartialPattern<Offset> fullPattern) => this.fullPattern = fullPattern;

      public ParseResult<Offset> Parse(string text) => !(text == "Z") ? this.fullPattern.Parse(text) : ParseResult<Offset>.ForValue(Offset.Zero);

      public string Format(Offset value) => !(value == Offset.Zero) ? this.fullPattern.Format(value) : "Z";

      public ParseResult<Offset> ParsePartial(ValueCursor cursor)
      {
        if (cursor.Current != 'Z')
          return this.fullPattern.ParsePartial(cursor);
        cursor.MoveNext();
        return ParseResult<Offset>.ForValue(Offset.Zero);
      }

      public void FormatPartial(Offset value, StringBuilder builder)
      {
        if (value == Offset.Zero)
          builder.Append("Z");
        else
          this.fullPattern.FormatPartial(value, builder);
      }
    }

    private sealed class NumberPattern : IPartialPattern<Offset>, IPattern<Offset>
    {
      private readonly NodaFormatInfo formatInfo;
      private readonly int maxLength;

      internal NumberPattern(NodaFormatInfo formatInfo)
      {
        this.formatInfo = formatInfo;
        this.maxLength = Offset.MinValue.Milliseconds.ToString("N0", (IFormatProvider) formatInfo.NumberFormat).Length;
      }

      public ParseResult<Offset> ParsePartial(ValueCursor cursor)
      {
        int index = cursor.Index;
        int length = Math.Min(this.maxLength, checked (cursor.Length - cursor.Index));
        while (length >= 0)
        {
          int result;
          if (int.TryParse(cursor.Value.Substring(cursor.Index, length), NumberStyles.Integer | NumberStyles.AllowThousands, (IFormatProvider) this.formatInfo.NumberFormat, out result))
          {
            if (result < -86400000 || 86400000 < result)
            {
              cursor.Move(index);
              return ParseResult<Offset>.ValueOutOfRange(cursor, (object) result);
            }
            cursor.Move(checked (cursor.Index + length));
            return ParseResult<Offset>.ForValue(Offset.FromMilliseconds(result));
          }
          checked { --length; }
        }
        cursor.Move(index);
        return ParseResult<Offset>.CannotParseValue(cursor, "n");
      }

      public void FormatPartial(Offset value, StringBuilder builder) => builder.Append(this.Format(value));

      public ParseResult<Offset> Parse(string text)
      {
        int result;
        if (!int.TryParse(text, NumberStyles.Integer | NumberStyles.AllowThousands, (IFormatProvider) this.formatInfo.NumberFormat, out result))
          return ParseResult<Offset>.CannotParseValue(new ValueCursor(text), "n");
        return result < -86400000 || 86400000 < result ? ParseResult<Offset>.ValueOutOfRange(new ValueCursor(text), (object) result) : ParseResult<Offset>.ForValue(Offset.FromMilliseconds(result));
      }

      public string Format(Offset value) => value.Milliseconds.ToString("N0", (IFormatProvider) this.formatInfo.NumberFormat);
    }

    [DebuggerStepThrough]
    private sealed class OffsetParseBucket : ParseBucket<Offset>
    {
      internal int Hours;
      internal int Minutes;
      internal int Seconds;
      internal int Milliseconds;
      public bool IsNegative;

      internal override ParseResult<Offset> CalculateValue(
        PatternFields usedFields,
        string text)
      {
        int milliseconds = checked (this.Hours * 3600000 + this.Minutes * 60000 + this.Seconds * 1000 + this.Milliseconds);
        if (this.IsNegative)
          milliseconds = checked (-milliseconds);
        return ParseResult<Offset>.ForValue(Offset.FromMilliseconds(milliseconds));
      }
    }
  }
}
