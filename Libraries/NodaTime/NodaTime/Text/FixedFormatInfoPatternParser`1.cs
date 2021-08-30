// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.FixedFormatInfoPatternParser`1
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Globalization;
using NodaTime.Text.Patterns;
using NodaTime.Utility;
using System;
using System.Collections.Generic;

namespace NodaTime.Text
{
  internal sealed class FixedFormatInfoPatternParser<T>
  {
    private const int CacheSize = 50;
    private readonly Cache<string, IPattern<T>> cache;

    internal FixedFormatInfoPatternParser(
      IPatternParser<T> patternParser,
      NodaFormatInfo formatInfo)
    {
      this.cache = new Cache<string, IPattern<T>>(50, (Func<string, IPattern<T>>) (patternText => patternParser.ParsePattern(patternText, formatInfo)), (IEqualityComparer<string>) StringComparer.Ordinal);
    }

    internal IPattern<T> ParsePattern(string pattern) => this.cache.GetOrAdd(pattern);
  }
}
