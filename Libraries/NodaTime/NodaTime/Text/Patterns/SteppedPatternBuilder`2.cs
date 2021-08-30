// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.SteppedPatternBuilder`2
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NodaTime.Text.Patterns
{
  internal sealed class SteppedPatternBuilder<TResult, TBucket> where TBucket : ParseBucket<TResult>
  {
    private readonly NodaFormatInfo formatInfo;
    private readonly List<Action<TResult, StringBuilder>> formatActions;
    private readonly List<SteppedPatternBuilder<TResult, TBucket>.ParseAction> parseActions;
    private readonly Func<TBucket> bucketProvider;
    private PatternFields usedFields;
    private bool formatOnly;

    internal SteppedPatternBuilder(NodaFormatInfo formatInfo, Func<TBucket> bucketProvider)
    {
      this.formatInfo = formatInfo;
      this.formatActions = new List<Action<TResult, StringBuilder>>();
      this.parseActions = new List<SteppedPatternBuilder<TResult, TBucket>.ParseAction>();
      this.bucketProvider = bucketProvider;
    }

    internal NodaFormatInfo FormatInfo => this.formatInfo;

    internal PatternFields UsedFields => this.usedFields;

    internal void SetFormatOnly() => this.formatOnly = true;

    internal void ParseCustomPattern(
      string patternText,
      Dictionary<char, CharacterHandler<TResult, TBucket>> characterHandlers)
    {
      PatternCursor patternCursor = new PatternCursor(patternText);
      while (patternCursor.MoveNext())
      {
        CharacterHandler<TResult, TBucket> characterHandler;
        if (characterHandlers.TryGetValue(patternCursor.Current, out characterHandler))
          characterHandler(patternCursor, this);
        else
          this.AddLiteral(patternCursor.Current, new Func<ValueCursor, char, ParseResult<TResult>>(ParseResult<TResult>.MismatchedCharacter));
      }
    }

    internal void ValidateUsedFields()
    {
      if ((this.usedFields & (PatternFields.YearOfEra | PatternFields.Era)) == PatternFields.Era)
        throw new InvalidPatternException(Messages.Parse_EraWithoutYearOfEra);
      if ((this.usedFields & (PatternFields.Era | PatternFields.Calendar)) == (PatternFields.Era | PatternFields.Calendar))
        throw new InvalidPatternException(Messages.Parse_CalendarAndEra);
    }

    internal IPartialPattern<TResult> Build()
    {
      Action<TResult, StringBuilder> formatActions = (Action<TResult, StringBuilder>) null;
      foreach (Action<TResult, StringBuilder> formatAction in this.formatActions)
      {
        SteppedPatternBuilder<TResult, TBucket>.IPostPatternParseFormatAction target = formatAction.Target as SteppedPatternBuilder<TResult, TBucket>.IPostPatternParseFormatAction;
        formatActions += target == null ? formatAction : target.BuildFormatAction(this.usedFields);
      }
      return (IPartialPattern<TResult>) new SteppedPatternBuilder<TResult, TBucket>.SteppedPattern(formatActions, this.formatOnly ? (SteppedPatternBuilder<TResult, TBucket>.ParseAction[]) null : this.parseActions.ToArray(), this.bucketProvider, this.usedFields);
    }

    internal void AddField(PatternFields field, char characterInPattern)
    {
      PatternFields patternFields = this.usedFields | field;
      this.usedFields = patternFields != this.usedFields ? patternFields : throw new InvalidPatternException(Messages.Parse_RepeatedFieldInPattern, new object[1]
      {
        (object) characterInPattern
      });
    }

    internal void AddParseAction(
      SteppedPatternBuilder<TResult, TBucket>.ParseAction parseAction)
    {
      this.parseActions.Add(parseAction);
    }

    internal void AddFormatAction(Action<TResult, StringBuilder> formatAction) => this.formatActions.Add(formatAction);

    internal void AddParseValueAction(
      int minimumDigits,
      int maximumDigits,
      char patternChar,
      int minimumValue,
      int maximumValue,
      Action<TBucket, int> valueSetter)
    {
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((cursor, bucket) =>
      {
        int index = cursor.Index;
        bool flag = cursor.Match('-');
        if (flag && minimumValue >= 0)
        {
          cursor.Move(index);
          return ParseResult<TResult>.UnexpectedNegative(cursor);
        }
        int result;
        if (!cursor.ParseDigits(minimumDigits, maximumDigits, out result))
        {
          cursor.Move(index);
          return ParseResult<TResult>.MismatchedNumber(cursor, new string(patternChar, minimumDigits));
        }
        if (flag)
          result = checked (-result);
        if (result < minimumValue || result > maximumValue)
        {
          cursor.Move(index);
          return ParseResult<TResult>.FieldValueOutOfRange(cursor, result, patternChar);
        }
        valueSetter(bucket, result);
        return (ParseResult<TResult>) null;
      }));
    }

    internal void AddLiteral(string expectedText, Func<ValueCursor, ParseResult<TResult>> failure)
    {
      if (expectedText.Length == 1)
      {
        char expectedChar = expectedText[0];
        this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) => !str.Match(expectedChar) ? failure(str) : (ParseResult<TResult>) null));
        this.AddFormatAction((Action<TResult, StringBuilder>) ((value, builder) => builder.Append(expectedChar)));
      }
      else
      {
        this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) => !str.Match(expectedText) ? failure(str) : (ParseResult<TResult>) null));
        this.AddFormatAction((Action<TResult, StringBuilder>) ((value, builder) => builder.Append(expectedText)));
      }
    }

    internal static void HandleQuote(
      PatternCursor pattern,
      SteppedPatternBuilder<TResult, TBucket> builder)
    {
      string quotedString = pattern.GetQuotedString(pattern.Current);
      builder.AddLiteral(quotedString, new Func<ValueCursor, ParseResult<TResult>>(ParseResult<TResult>.QuotedStringMismatch));
    }

    internal static void HandleBackslash(
      PatternCursor pattern,
      SteppedPatternBuilder<TResult, TBucket> builder)
    {
      if (!pattern.MoveNext())
        throw new InvalidPatternException(Messages.Parse_EscapeAtEndOfString);
      builder.AddLiteral(pattern.Current, new Func<ValueCursor, char, ParseResult<TResult>>(ParseResult<TResult>.EscapedCharacterMismatch));
    }

    internal static void HandlePercent(
      PatternCursor pattern,
      SteppedPatternBuilder<TResult, TBucket> builder)
    {
      if (!pattern.HasMoreCharacters)
        throw new InvalidPatternException(Messages.Parse_PercentAtEndOfString);
      if (pattern.PeekNext() == '%')
        throw new InvalidPatternException(Messages.Parse_PercentDoubled);
    }

    internal static CharacterHandler<TResult, TBucket> HandlePaddedField(
      int maxCount,
      PatternFields field,
      int minValue,
      int maxValue,
      Func<TResult, int> getter,
      Action<TBucket, int> setter)
    {
      return (CharacterHandler<TResult, TBucket>) ((pattern, builder) =>
      {
        int repeatCount = pattern.GetRepeatCount(maxCount);
        builder.AddField(field, pattern.Current);
        builder.AddParseValueAction(repeatCount, maxCount, pattern.Current, minValue, maxValue, setter);
        builder.AddFormatLeftPad(repeatCount, getter);
      });
    }

    internal void AddLiteral(
      char expectedChar,
      Func<ValueCursor, char, ParseResult<TResult>> failureSelector)
    {
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) => !str.Match(expectedChar) ? failureSelector(str, expectedChar) : (ParseResult<TResult>) null));
      this.AddFormatAction((Action<TResult, StringBuilder>) ((value, builder) => builder.Append(expectedChar)));
    }

    internal void AddParseLongestTextAction(
      char field,
      Action<TBucket, int> setter,
      CompareInfo compareInfo,
      IList<string> textValues)
    {
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
      {
        int bestIndex = -1;
        int longestMatch = 0;
        SteppedPatternBuilder<TResult, TBucket>.FindLongestMatch(compareInfo, str, textValues, ref bestIndex, ref longestMatch);
        if (bestIndex == -1)
          return ParseResult<TResult>.MismatchedText(str, field);
        setter(bucket, bestIndex);
        str.Move(checked (str.Index + longestMatch));
        return (ParseResult<TResult>) null;
      }));
    }

    internal void AddParseLongestTextAction(
      char field,
      Action<TBucket, int> setter,
      CompareInfo compareInfo,
      IList<string> textValues1,
      IList<string> textValues2)
    {
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
      {
        int bestIndex = -1;
        int longestMatch = 0;
        SteppedPatternBuilder<TResult, TBucket>.FindLongestMatch(compareInfo, str, textValues1, ref bestIndex, ref longestMatch);
        SteppedPatternBuilder<TResult, TBucket>.FindLongestMatch(compareInfo, str, textValues2, ref bestIndex, ref longestMatch);
        if (bestIndex == -1)
          return ParseResult<TResult>.MismatchedText(str, field);
        setter(bucket, bestIndex);
        str.Move(checked (str.Index + longestMatch));
        return (ParseResult<TResult>) null;
      }));
    }

    private static void FindLongestMatch(
      CompareInfo compareInfo,
      ValueCursor cursor,
      IList<string> values,
      ref int bestIndex,
      ref int longestMatch)
    {
      int index = 0;
      while (index < values.Count)
      {
        string match = values[index];
        if (match != null && match.Length > longestMatch && cursor.MatchCaseInsensitive(match, compareInfo, false))
        {
          bestIndex = index;
          longestMatch = match.Length;
        }
        checked { ++index; }
      }
    }

    public void AddRequiredSign(
      Action<TBucket, bool> signSetter,
      Func<TResult, bool> nonNegativePredicate)
    {
      string negativeSign = this.formatInfo.NegativeSign;
      string positiveSign = this.formatInfo.PositiveSign;
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
      {
        if (str.Match(negativeSign))
        {
          signSetter(bucket, false);
          return (ParseResult<TResult>) null;
        }
        if (!str.Match(positiveSign))
          return ParseResult<TResult>.MissingSign(str);
        signSetter(bucket, true);
        return (ParseResult<TResult>) null;
      }));
      this.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => sb.Append(nonNegativePredicate(value) ? positiveSign : negativeSign)));
    }

    public void AddNegativeOnlySign(
      Action<TBucket, bool> signSetter,
      Func<TResult, bool> nonNegativePredicate)
    {
      string negativeSign = this.formatInfo.NegativeSign;
      string positiveSign = this.formatInfo.PositiveSign;
      this.AddParseAction((SteppedPatternBuilder<TResult, TBucket>.ParseAction) ((str, bucket) =>
      {
        if (str.Match(negativeSign))
        {
          signSetter(bucket, false);
          return (ParseResult<TResult>) null;
        }
        if (str.Match(positiveSign))
          return ParseResult<TResult>.PositiveSignInvalid(str);
        signSetter(bucket, true);
        return (ParseResult<TResult>) null;
      }));
      this.AddFormatAction((Action<TResult, StringBuilder>) ((value, builder) =>
      {
        if (nonNegativePredicate(value))
          return;
        builder.Append(negativeSign);
      }));
    }

    internal void AddFormatLeftPad(int count, Func<TResult, int> selector) => this.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.LeftPad(selector(value), count, sb)));

    internal void AddFormatFraction(int width, int scale, Func<TResult, int> selector) => this.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.AppendFraction(selector(value), width, scale, sb)));

    internal void AddFormatFractionTruncate(int width, int scale, Func<TResult, int> selector) => this.AddFormatAction((Action<TResult, StringBuilder>) ((value, sb) => FormatHelper.AppendFractionTruncate(selector(value), width, scale, sb)));

    internal delegate ParseResult<TResult> ParseAction(
      ValueCursor cursor,
      TBucket bucket)
      where TBucket : ParseBucket<TResult>;

    internal interface IPostPatternParseFormatAction
    {
      Action<TResult, StringBuilder> BuildFormatAction(PatternFields finalFields);
    }

    private sealed class SteppedPattern : IPartialPattern<TResult>, IPattern<TResult>
    {
      private readonly Action<TResult, StringBuilder> formatActions;
      private readonly SteppedPatternBuilder<TResult, TBucket>.ParseAction[] parseActions;
      private readonly Func<TBucket> bucketProvider;
      private readonly PatternFields usedFields;

      public SteppedPattern(
        Action<TResult, StringBuilder> formatActions,
        SteppedPatternBuilder<TResult, TBucket>.ParseAction[] parseActions,
        Func<TBucket> bucketProvider,
        PatternFields usedFields)
      {
        this.formatActions = formatActions;
        this.parseActions = parseActions;
        this.bucketProvider = bucketProvider;
        this.usedFields = usedFields;
      }

      public ParseResult<TResult> Parse(string text)
      {
        if (this.parseActions == null)
          return ParseResult<TResult>.FormatOnlyPattern;
        switch (text)
        {
          case "":
            return ParseResult<TResult>.ValueStringEmpty;
          case null:
            return ParseResult<TResult>.ArgumentNull(nameof (text));
          default:
            ValueCursor cursor = new ValueCursor(text);
            cursor.MoveNext();
            ParseResult<TResult> partial = this.ParsePartial(cursor);
            return !partial.Success || cursor.Current == char.MinValue ? partial : ParseResult<TResult>.ExtraValueCharacters(cursor, cursor.Remainder);
        }
      }

      public string Format(TResult value)
      {
        StringBuilder stringBuilder = new StringBuilder();
        this.formatActions(value, stringBuilder);
        return stringBuilder.ToString();
      }

      public ParseResult<TResult> ParsePartial(ValueCursor cursor)
      {
        TBucket bucket = this.bucketProvider();
        foreach (SteppedPatternBuilder<TResult, TBucket>.ParseAction parseAction in this.parseActions)
        {
          ParseResult<TResult> parseResult = parseAction(cursor, bucket);
          if (parseResult != null)
            return parseResult;
        }
        return bucket.CalculateValue(this.usedFields, cursor.Value);
      }

      public void FormatPartial(TResult value, StringBuilder builder) => this.formatActions(value, builder);
    }
  }
}
