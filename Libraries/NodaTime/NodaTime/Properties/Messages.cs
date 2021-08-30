// Decompiled with JetBrains decompiler
// Type: NodaTime.Properties.Messages
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace NodaTime.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
  internal class Messages
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Messages()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Messages.resourceMan, (object) null))
          Messages.resourceMan = new ResourceManager("NodaTime.Properties.Messages", typeof (Messages).Assembly);
        return Messages.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Messages.resourceCulture;
      set => Messages.resourceCulture = value;
    }

    internal static string Format_BadQuote => Messages.ResourceManager.GetString(nameof (Format_BadQuote), Messages.resourceCulture);

    internal static string FormatDefaultExceptionMessage => Messages.ResourceManager.GetString(nameof (FormatDefaultExceptionMessage), Messages.resourceCulture);

    internal static string Noda_ArgumentNull => Messages.ResourceManager.GetString(nameof (Noda_ArgumentNull), Messages.resourceCulture);

    internal static string Noda_CannotChangeReadOnly => Messages.ResourceManager.GetString(nameof (Noda_CannotChangeReadOnly), Messages.resourceCulture);

    internal static string Noda_StringEmpty => Messages.ResourceManager.GetString(nameof (Noda_StringEmpty), Messages.resourceCulture);

    internal static string Noda_StringEmptyOrWhitespace => Messages.ResourceManager.GetString(nameof (Noda_StringEmptyOrWhitespace), Messages.resourceCulture);

    internal static string Parse_AmbiguousLocalTime => Messages.ResourceManager.GetString(nameof (Parse_AmbiguousLocalTime), Messages.resourceCulture);

    internal static string Parse_CalendarAndEra => Messages.ResourceManager.GetString(nameof (Parse_CalendarAndEra), Messages.resourceCulture);

    internal static string Parse_CannotParseValue => Messages.ResourceManager.GetString(nameof (Parse_CannotParseValue), Messages.resourceCulture);

    internal static string Parse_DateSeparatorMismatch => Messages.ResourceManager.GetString(nameof (Parse_DateSeparatorMismatch), Messages.resourceCulture);

    internal static string Parse_DayOfMonthOutOfRange => Messages.ResourceManager.GetString(nameof (Parse_DayOfMonthOutOfRange), Messages.resourceCulture);

    internal static string Parse_DoubleAssignment => Messages.ResourceManager.GetString(nameof (Parse_DoubleAssignment), Messages.resourceCulture);

    internal static string Parse_EmptyFormatsArray => Messages.ResourceManager.GetString(nameof (Parse_EmptyFormatsArray), Messages.resourceCulture);

    internal static string Parse_EmptyPeriod => Messages.ResourceManager.GetString(nameof (Parse_EmptyPeriod), Messages.resourceCulture);

    internal static string Parse_EmptyZPrefixedOffsetPattern => Messages.ResourceManager.GetString(nameof (Parse_EmptyZPrefixedOffsetPattern), Messages.resourceCulture);

    internal static string Parse_EndOfString => Messages.ResourceManager.GetString(nameof (Parse_EndOfString), Messages.resourceCulture);

    internal static string Parse_EraWithoutYearOfEra => Messages.ResourceManager.GetString(nameof (Parse_EraWithoutYearOfEra), Messages.resourceCulture);

    internal static string Parse_EscapeAtEndOfString => Messages.ResourceManager.GetString(nameof (Parse_EscapeAtEndOfString), Messages.resourceCulture);

    internal static string Parse_EscapedCharacterMismatch => Messages.ResourceManager.GetString(nameof (Parse_EscapedCharacterMismatch), Messages.resourceCulture);

    internal static string Parse_ExpectedEndOfString => Messages.ResourceManager.GetString(nameof (Parse_ExpectedEndOfString), Messages.resourceCulture);

    internal static string Parse_ExtraValueCharacters => Messages.ResourceManager.GetString(nameof (Parse_ExtraValueCharacters), Messages.resourceCulture);

    internal static string Parse_FieldValueOutOfRange => Messages.ResourceManager.GetString(nameof (Parse_FieldValueOutOfRange), Messages.resourceCulture);

    internal static string Parse_FormatElementInvalid => Messages.ResourceManager.GetString(nameof (Parse_FormatElementInvalid), Messages.resourceCulture);

    internal static string Parse_FormatInvalid => Messages.ResourceManager.GetString(nameof (Parse_FormatInvalid), Messages.resourceCulture);

    internal static string Parse_FormatOnlyPattern => Messages.ResourceManager.GetString(nameof (Parse_FormatOnlyPattern), Messages.resourceCulture);

    internal static string Parse_FormatStringEmpty => Messages.ResourceManager.GetString(nameof (Parse_FormatStringEmpty), Messages.resourceCulture);

    internal static string Parse_Hour12PatternNotSupported => Messages.ResourceManager.GetString(nameof (Parse_Hour12PatternNotSupported), Messages.resourceCulture);

    internal static string Parse_InconsistentDayOfWeekTextValue => Messages.ResourceManager.GetString(nameof (Parse_InconsistentDayOfWeekTextValue), Messages.resourceCulture);

    internal static string Parse_InconsistentMonthTextValue => Messages.ResourceManager.GetString(nameof (Parse_InconsistentMonthTextValue), Messages.resourceCulture);

    internal static string Parse_InconsistentValues2 => Messages.ResourceManager.GetString(nameof (Parse_InconsistentValues2), Messages.resourceCulture);

    internal static string Parse_InvalidHour24 => Messages.ResourceManager.GetString(nameof (Parse_InvalidHour24), Messages.resourceCulture);

    internal static string Parse_InvalidOffset => Messages.ResourceManager.GetString(nameof (Parse_InvalidOffset), Messages.resourceCulture);

    internal static string Parse_InvalidUnitSpecifier => Messages.ResourceManager.GetString(nameof (Parse_InvalidUnitSpecifier), Messages.resourceCulture);

    internal static string Parse_MismatchedCharacter => Messages.ResourceManager.GetString(nameof (Parse_MismatchedCharacter), Messages.resourceCulture);

    internal static string Parse_MismatchedNumber => Messages.ResourceManager.GetString(nameof (Parse_MismatchedNumber), Messages.resourceCulture);

    internal static string Parse_MismatchedText => Messages.ResourceManager.GetString(nameof (Parse_MismatchedText), Messages.resourceCulture);

    internal static string Parse_MisplacedUnitSpecifier => Messages.ResourceManager.GetString(nameof (Parse_MisplacedUnitSpecifier), Messages.resourceCulture);

    internal static string Parse_MissingAmPmDesignator => Messages.ResourceManager.GetString(nameof (Parse_MissingAmPmDesignator), Messages.resourceCulture);

    internal static string Parse_MissingEmbeddedPatternEnd => Messages.ResourceManager.GetString(nameof (Parse_MissingEmbeddedPatternEnd), Messages.resourceCulture);

    internal static string Parse_MissingEmbeddedPatternStart => Messages.ResourceManager.GetString(nameof (Parse_MissingEmbeddedPatternStart), Messages.resourceCulture);

    internal static string Parse_MissingEndQuote => Messages.ResourceManager.GetString(nameof (Parse_MissingEndQuote), Messages.resourceCulture);

    internal static string Parse_MissingNumber => Messages.ResourceManager.GetString(nameof (Parse_MissingNumber), Messages.resourceCulture);

    internal static string Parse_MissingSign => Messages.ResourceManager.GetString(nameof (Parse_MissingSign), Messages.resourceCulture);

    internal static string Parse_MonthOutOfRange => Messages.ResourceManager.GetString(nameof (Parse_MonthOutOfRange), Messages.resourceCulture);

    internal static string Parse_MultipleCapitalDurationFields => Messages.ResourceManager.GetString(nameof (Parse_MultipleCapitalDurationFields), Messages.resourceCulture);

    internal static string Parse_NoMatchingCalendarSystem => Messages.ResourceManager.GetString(nameof (Parse_NoMatchingCalendarSystem), Messages.resourceCulture);

    internal static string Parse_NoMatchingFormat => Messages.ResourceManager.GetString(nameof (Parse_NoMatchingFormat), Messages.resourceCulture);

    internal static string Parse_NoMatchingZoneId => Messages.ResourceManager.GetString(nameof (Parse_NoMatchingZoneId), Messages.resourceCulture);

    internal static string Parse_PercentAtEndOfString => Messages.ResourceManager.GetString(nameof (Parse_PercentAtEndOfString), Messages.resourceCulture);

    internal static string Parse_PercentDoubled => Messages.ResourceManager.GetString(nameof (Parse_PercentDoubled), Messages.resourceCulture);

    internal static string Parse_PositiveSignInvalid => Messages.ResourceManager.GetString(nameof (Parse_PositiveSignInvalid), Messages.resourceCulture);

    internal static string Parse_PrecisionNotSupported => Messages.ResourceManager.GetString(nameof (Parse_PrecisionNotSupported), Messages.resourceCulture);

    internal static string Parse_QuotedStringMismatch => Messages.ResourceManager.GetString(nameof (Parse_QuotedStringMismatch), Messages.resourceCulture);

    internal static string Parse_RepeatCountExceeded => Messages.ResourceManager.GetString(nameof (Parse_RepeatCountExceeded), Messages.resourceCulture);

    internal static string Parse_RepeatCountUnderMinimum => Messages.ResourceManager.GetString(nameof (Parse_RepeatCountUnderMinimum), Messages.resourceCulture);

    internal static string Parse_RepeatedFieldInPattern => Messages.ResourceManager.GetString(nameof (Parse_RepeatedFieldInPattern), Messages.resourceCulture);

    internal static string Parse_RepeatedUnitSpecifier => Messages.ResourceManager.GetString(nameof (Parse_RepeatedUnitSpecifier), Messages.resourceCulture);

    internal static string Parse_SkippedLocalTime => Messages.ResourceManager.GetString(nameof (Parse_SkippedLocalTime), Messages.resourceCulture);

    internal static string Parse_TimeSeparatorMismatch => Messages.ResourceManager.GetString(nameof (Parse_TimeSeparatorMismatch), Messages.resourceCulture);

    internal static string Parse_UnexpectedEndOfString => Messages.ResourceManager.GetString(nameof (Parse_UnexpectedEndOfString), Messages.resourceCulture);

    internal static string Parse_UnexpectedNegative => Messages.ResourceManager.GetString(nameof (Parse_UnexpectedNegative), Messages.resourceCulture);

    internal static string Parse_UnknownFailure => Messages.ResourceManager.GetString(nameof (Parse_UnknownFailure), Messages.resourceCulture);

    internal static string Parse_UnknownStandardFormat => Messages.ResourceManager.GetString(nameof (Parse_UnknownStandardFormat), Messages.resourceCulture);

    internal static string Parse_UnparsableValue => Messages.ResourceManager.GetString(nameof (Parse_UnparsableValue), Messages.resourceCulture);

    internal static string Parse_UnparsableValuePostParse => Messages.ResourceManager.GetString(nameof (Parse_UnparsableValuePostParse), Messages.resourceCulture);

    internal static string Parse_ValueOutOfRange => Messages.ResourceManager.GetString(nameof (Parse_ValueOutOfRange), Messages.resourceCulture);

    internal static string Parse_ValueStringEmpty => Messages.ResourceManager.GetString(nameof (Parse_ValueStringEmpty), Messages.resourceCulture);

    internal static string Parse_YearOfEraOutOfRange => Messages.ResourceManager.GetString(nameof (Parse_YearOfEraOutOfRange), Messages.resourceCulture);

    internal static string Parse_ZPrefixNotAtStartOfPattern => Messages.ResourceManager.GetString(nameof (Parse_ZPrefixNotAtStartOfPattern), Messages.resourceCulture);
  }
}
