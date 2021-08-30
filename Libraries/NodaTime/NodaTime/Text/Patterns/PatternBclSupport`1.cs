// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.PatternBclSupport`1
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using System;

namespace NodaTime.Text.Patterns
{
  internal sealed class PatternBclSupport<T>
  {
    private readonly Func<NodaFormatInfo, FixedFormatInfoPatternParser<T>> patternParser;
    private readonly string defaultFormatPattern;

    internal PatternBclSupport(
      string defaultFormatPattern,
      Func<NodaFormatInfo, FixedFormatInfoPatternParser<T>> patternParser)
    {
      this.patternParser = patternParser;
      this.defaultFormatPattern = defaultFormatPattern;
    }

    internal string Format(T value, string patternText, IFormatProvider formatProvider)
    {
      if (string.IsNullOrEmpty(patternText))
        patternText = this.defaultFormatPattern;
      return this.patternParser(NodaFormatInfo.GetInstance(formatProvider)).ParsePattern(patternText).Format(value);
    }
  }
}
