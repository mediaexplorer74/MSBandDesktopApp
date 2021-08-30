// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.ParseResult`1
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Properties;
using System;
using System.Globalization;

namespace NodaTime.Text
{
  [Immutable]
  public sealed class ParseResult<T>
  {
    private readonly T value;
    private readonly Func<Exception> exceptionProvider;
    private readonly bool continueWithMultiple;
    internal static readonly ParseResult<T> ValueStringEmpty = new ParseResult<T>((Func<Exception>) (() => (Exception) new UnparsableValueException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Messages.Parse_ValueStringEmpty))), false);
    internal static readonly ParseResult<T> FormatOnlyPattern = new ParseResult<T>((Func<Exception>) (() => (Exception) new UnparsableValueException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Messages.Parse_FormatOnlyPattern))), true);

    private ParseResult(Func<Exception> exceptionProvider, bool continueWithMultiple)
    {
      this.exceptionProvider = exceptionProvider;
      this.continueWithMultiple = continueWithMultiple;
    }

    private ParseResult(T value) => this.value = value;

    public T Value => this.GetValueOrThrow();

    public Exception Exception
    {
      get
      {
        if (this.exceptionProvider == null)
          throw new InvalidOperationException("Parse operation succeeded, so no exception is available");
        return this.exceptionProvider();
      }
    }

    public T GetValueOrThrow()
    {
      if (this.exceptionProvider == null)
        return this.value;
      throw this.exceptionProvider();
    }

    public bool TryGetValue(T failureValue, out T result)
    {
      bool flag = this.exceptionProvider == null;
      result = flag ? this.value : failureValue;
      return flag;
    }

    public bool Success => this.exceptionProvider == null;

    internal bool ContinueAfterErrorWithMultipleFormats => this.continueWithMultiple;

    internal ParseResult<TTarget> Convert<TTarget>(Func<T, TTarget> projection) => !this.Success ? new ParseResult<TTarget>(this.exceptionProvider, this.continueWithMultiple) : ParseResult<TTarget>.ForValue(projection(this.Value));

    internal ParseResult<TTarget> ConvertError<TTarget>()
    {
      if (this.Success)
        throw new InvalidOperationException("ConvertError should not be called on a successful parse result");
      return new ParseResult<TTarget>(this.exceptionProvider, this.continueWithMultiple);
    }

    internal static ParseResult<T> ForValue(T value) => new ParseResult<T>(value);

    internal static ParseResult<T> ForException(Func<Exception> exceptionProvider) => new ParseResult<T>(exceptionProvider, false);

    internal static ParseResult<T> ForInvalidValue(
      ValueCursor cursor,
      string formatString,
      params object[] parameters)
    {
      return ParseResult<T>.ForInvalidValue((Func<Exception>) (() => (Exception) new UnparsableValueException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Messages.Parse_UnparsableValue, new object[2]
      {
        (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, formatString, parameters),
        (object) cursor
      }))));
    }

    internal static ParseResult<T> ForInvalidValuePostParse(
      string text,
      string formatString,
      params object[] parameters)
    {
      return ParseResult<T>.ForInvalidValue((Func<Exception>) (() => (Exception) new UnparsableValueException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Messages.Parse_UnparsableValuePostParse, new object[2]
      {
        (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, formatString, parameters),
        (object) text
      }))));
    }

    private static ParseResult<T> ForInvalidValue(Func<Exception> exceptionProvider) => new ParseResult<T>(exceptionProvider, true);

    internal static ParseResult<T> ArgumentNull(string parameter) => new ParseResult<T>((Func<Exception>) (() => (Exception) new ArgumentNullException(parameter)), false);

    internal static ParseResult<T> PositiveSignInvalid(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_PositiveSignInvalid);

    internal static ParseResult<T> CannotParseValue(ValueCursor cursor, string format) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_CannotParseValue, (object) typeof (T), (object) format);

    internal static ParseResult<T> ExtraValueCharacters(
      ValueCursor cursor,
      string remainder)
    {
      return ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_ExtraValueCharacters, (object) remainder);
    }

    internal static ParseResult<T> QuotedStringMismatch(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_QuotedStringMismatch);

    internal static ParseResult<T> EscapedCharacterMismatch(
      ValueCursor cursor,
      char patternCharacter)
    {
      return ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_EscapedCharacterMismatch, (object) patternCharacter);
    }

    internal static ParseResult<T> EndOfString(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_EndOfString);

    internal static ParseResult<T> TimeSeparatorMismatch(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_TimeSeparatorMismatch);

    internal static ParseResult<T> DateSeparatorMismatch(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_DateSeparatorMismatch);

    internal static ParseResult<T> MissingNumber(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MissingNumber);

    internal static ParseResult<T> UnexpectedNegative(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_UnexpectedNegative);

    internal static ParseResult<T> MismatchedNumber(ValueCursor cursor, string pattern) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MismatchedNumber, (object) pattern);

    internal static ParseResult<T> MismatchedCharacter(
      ValueCursor cursor,
      char patternCharacter)
    {
      return ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MismatchedCharacter, (object) patternCharacter);
    }

    internal static ParseResult<T> MismatchedText(ValueCursor cursor, char field) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MismatchedText, (object) field);

    internal static ParseResult<T> NoMatchingFormat(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_NoMatchingFormat);

    internal static ParseResult<T> ValueOutOfRange(ValueCursor cursor, object value) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_ValueOutOfRange, value, (object) typeof (T));

    internal static ParseResult<T> MissingSign(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MissingSign);

    internal static ParseResult<T> MissingAmPmDesignator(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_MissingAmPmDesignator);

    internal static ParseResult<T> NoMatchingCalendarSystem(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_NoMatchingCalendarSystem);

    internal static ParseResult<T> NoMatchingZoneId(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_NoMatchingZoneId);

    internal static ParseResult<T> InvalidHour24(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_InvalidHour24);

    internal static ParseResult<T> FieldValueOutOfRange(
      ValueCursor cursor,
      int value,
      char field)
    {
      return ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_FieldValueOutOfRange, (object) value, (object) field, (object) typeof (T));
    }

    internal static ParseResult<T> FieldValueOutOfRangePostParse(
      string text,
      int value,
      char field)
    {
      return ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_FieldValueOutOfRange, (object) value, (object) field, (object) typeof (T));
    }

    internal static ParseResult<T> InconsistentValues(
      string text,
      char field1,
      char field2)
    {
      return ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_InconsistentValues2, (object) field1, (object) field2, (object) typeof (T));
    }

    internal static ParseResult<T> InconsistentMonthValues(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_InconsistentMonthTextValue);

    internal static ParseResult<T> InconsistentDayOfWeekTextValue(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_InconsistentDayOfWeekTextValue);

    internal static ParseResult<T> ExpectedEndOfString(ValueCursor cursor) => ParseResult<T>.ForInvalidValue(cursor, Messages.Parse_ExpectedEndOfString);

    internal static ParseResult<T> YearOfEraOutOfRange(
      string text,
      int value,
      int eraIndex,
      CalendarSystem calendar)
    {
      return ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_YearOfEraOutOfRange, (object) value, (object) calendar.Eras[eraIndex].Name, (object) calendar.Name);
    }

    internal static ParseResult<T> MonthOutOfRange(string text, int month, int year) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_MonthOutOfRange, (object) month, (object) year);

    internal static ParseResult<T> DayOfMonthOutOfRange(
      string text,
      int day,
      int month,
      int year)
    {
      return ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_DayOfMonthOutOfRange, (object) day, (object) month, (object) year);
    }

    internal static ParseResult<T> InvalidOffset(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_InvalidOffset);

    internal static ParseResult<T> SkippedLocalTime(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_SkippedLocalTime);

    internal static ParseResult<T> AmbiguousLocalTime(string text) => ParseResult<T>.ForInvalidValuePostParse(text, Messages.Parse_AmbiguousLocalTime);
  }
}
