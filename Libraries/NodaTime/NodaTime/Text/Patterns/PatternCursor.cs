// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.PatternCursor
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Properties;
using System.Text;

namespace NodaTime.Text.Patterns
{
  internal sealed class PatternCursor : TextCursor
  {
    internal PatternCursor(string pattern)
      : base(pattern)
    {
    }

    internal string GetQuotedString(char closeQuote)
    {
      StringBuilder stringBuilder = new StringBuilder(checked (this.Length - this.Index));
      bool flag = false;
      while (this.MoveNext())
      {
        if ((int) this.Current == (int) closeQuote)
        {
          this.MoveNext();
          flag = true;
          break;
        }
        if (this.Current == '\\' && !this.MoveNext())
          throw new InvalidPatternException(Messages.Parse_EscapeAtEndOfString);
        stringBuilder.Append(this.Current);
      }
      if (!flag)
        throw new InvalidPatternException(Messages.Parse_MissingEndQuote, new object[1]
        {
          (object) closeQuote
        });
      this.MovePrevious();
      return stringBuilder.ToString();
    }

    internal int GetRepeatCount(int maximumCount)
    {
      char current = this.Current;
      int index = this.Index;
      do
        ;
      while (this.MoveNext() && (int) this.Current == (int) current);
      int num = checked (this.Index - index);
      this.MovePrevious();
      return num <= maximumCount ? num : throw new InvalidPatternException(Messages.Parse_RepeatCountExceeded, new object[2]
      {
        (object) current,
        (object) maximumCount
      });
    }

    internal string GetEmbeddedPattern(char startPattern, char endPattern)
    {
      if (!this.MoveNext() || (int) this.Current != (int) startPattern)
        throw new InvalidPatternException(string.Format(Messages.Parse_MissingEmbeddedPatternStart, new object[1]
        {
          (object) startPattern
        }));
      int startIndex = checked (this.Index + 1);
      while (this.MoveNext())
      {
        char current = this.Current;
        if ((int) current == (int) endPattern)
          return this.Value.Substring(startIndex, checked (this.Index - startIndex));
        switch (current)
        {
          case '"':
          case '\'':
            this.GetQuotedString(current);
            continue;
          case '\\':
            if (!this.MoveNext())
              throw new InvalidPatternException(Messages.Parse_EscapeAtEndOfString);
            continue;
          default:
            continue;
        }
      }
      throw new InvalidPatternException(string.Format(Messages.Parse_MissingEmbeddedPatternEnd, new object[1]
      {
        (object) endPattern
      }));
    }
  }
}
