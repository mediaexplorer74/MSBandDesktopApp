// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.CompositePattern`1
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.Text.Patterns
{
  internal sealed class CompositePattern<T> : IPartialPattern<T>, IPattern<T>
  {
    private readonly List<IPartialPattern<T>> parsePatterns;
    private readonly Func<T, IPartialPattern<T>> formatPatternPicker;

    internal CompositePattern(
      IEnumerable<IPartialPattern<T>> parsePatterns,
      Func<T, IPartialPattern<T>> formatPatternPicker)
    {
      this.parsePatterns = new List<IPartialPattern<T>>(parsePatterns);
      this.formatPatternPicker = formatPatternPicker;
    }

    public ParseResult<T> Parse(string text)
    {
      foreach (IPattern<T> parsePattern in this.parsePatterns)
      {
        ParseResult<T> parseResult = parsePattern.Parse(text);
        if (parseResult.Success || !parseResult.ContinueAfterErrorWithMultipleFormats)
          return parseResult;
      }
      return ParseResult<T>.NoMatchingFormat(new ValueCursor(text));
    }

    public string Format(T value) => this.formatPatternPicker(value).Format(value);

    public ParseResult<T> ParsePartial(ValueCursor cursor)
    {
      int index = cursor.Index;
      foreach (IPartialPattern<T> parsePattern in this.parsePatterns)
      {
        cursor.Move(index);
        ParseResult<T> partial = parsePattern.ParsePartial(cursor);
        if (partial.Success || !partial.ContinueAfterErrorWithMultipleFormats)
          return partial;
      }
      cursor.Move(index);
      return ParseResult<T>.NoMatchingFormat(cursor);
    }

    public void FormatPartial(T value, StringBuilder builder) => this.formatPatternPicker(value).FormatPartial(value, builder);
  }
}
