// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Parse.MvxParser
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cirrious.CrossCore.Parse
{
  public abstract class MvxParser
  {
    protected string FullText { get; private set; }

    protected int CurrentIndex { get; private set; }

    protected virtual void Reset(string textToParse)
    {
      this.FullText = textToParse;
      this.CurrentIndex = 0;
    }

    protected bool IsComplete => this.CurrentIndex >= this.FullText.Length;

    protected char CurrentChar => this.FullText[this.CurrentIndex];

    protected string ReadQuotedString()
    {
      bool flag = false;
      char currentChar1 = this.CurrentChar;
      switch (currentChar1)
      {
        case '"':
        case '\'':
          this.MoveNext();
          if (this.IsComplete)
            throw new MvxException("Error parsing string indexer - unterminated in text {0}", new object[1]
            {
              (object) this.FullText
            });
          StringBuilder stringBuilder = new StringBuilder();
          while (!this.IsComplete)
          {
            if (flag)
            {
              stringBuilder.Append(this.ReadEscapedCharacter());
              flag = false;
            }
            else
            {
              char currentChar2 = this.CurrentChar;
              this.MoveNext();
              if (currentChar2 == '\\')
              {
                flag = true;
              }
              else
              {
                if ((int) currentChar2 == (int) currentChar1)
                  return stringBuilder.ToString();
                stringBuilder.Append(currentChar2);
              }
            }
          }
          throw new MvxException("Error parsing string indexer - unterminated in text {0}", new object[1]
          {
            (object) this.FullText
          });
        default:
          throw new MvxException("Error parsing string indexer - unexpected quote character {0} in text {1}", new object[2]
          {
            (object) currentChar1,
            (object) this.FullText
          });
      }
    }

    protected uint ReadUnsignedInteger()
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (!this.IsComplete && char.IsDigit(this.CurrentChar))
      {
        stringBuilder.Append(this.CurrentChar);
        this.MoveNext();
      }
      string s = stringBuilder.ToString();
      uint result;
      if (!uint.TryParse(s, out result))
        throw new MvxException("Unable to parse integer text from {0} in {1}", new object[2]
        {
          (object) s,
          (object) this.FullText
        });
      return result;
    }

    protected char ReadEscapedCharacter()
    {
      char currentChar = this.CurrentChar;
      this.MoveNext();
      switch (currentChar)
      {
        case '"':
          return '"';
        case '\'':
          return '\'';
        case '0':
          return char.MinValue;
        case 'U':
          if (this.ReadNDigits(4) != "0000")
            throw new MvxException("\\U unicode character does not start with 0000 in {1}", new object[1]
            {
              (object) this.FullText
            });
          return this.ReadFourDigitUnicodeCharacter();
        case '\\':
          return '\\';
        case 'a':
          return '\a';
        case 'b':
          return '\b';
        case 'f':
          return '\f';
        case 'n':
          return '\n';
        case 'r':
          return '\r';
        case 't':
          return '\t';
        case 'u':
          return this.ReadFourDigitUnicodeCharacter();
        case 'v':
          return '\v';
        case 'x':
          throw new MvxException("We don't support string literals containing \\x - suggest using \\u escaped characters instead");
        default:
          throw new MvxException("Sorry we don't currently support escaped characters like \\{0}", new object[1]
          {
            (object) currentChar
          });
      }
    }

    private char ReadFourDigitUnicodeCharacter()
    {
      uint num = uint.Parse(this.ReadNDigits(4), NumberStyles.HexNumber);
      return num <= (uint) ushort.MaxValue ? (char) num : throw new MvxException("\\u unicode character {0} out of range in {1}", new object[2]
      {
        (object) num,
        (object) this.FullText
      });
    }

    private string ReadNDigits(int count)
    {
      StringBuilder stringBuilder = new StringBuilder(count);
      for (int index = 0; index < count; ++index)
      {
        if (this.IsComplete)
          throw new MvxException("Error while reading {0} of {1} digits in {2}", new object[3]
          {
            (object) (index + 1),
            (object) count,
            (object) this.FullText
          });
        char currentChar = this.CurrentChar;
        if (!char.IsDigit(currentChar))
          throw new MvxException("Error while reading {0} of {1} digits in {2} - not a char {3}", new object[4]
          {
            (object) (index + 1),
            (object) count,
            (object) this.FullText,
            (object) currentChar
          });
        stringBuilder.Append(currentChar);
        this.MoveNext();
      }
      return stringBuilder.ToString();
    }

    protected void MoveNext(uint increment = 1) => this.CurrentIndex += (int) increment;

    protected void SkipWhitespaceAndCharacters(params char[] toSkip) => this.SkipWhitespaceAndCharacters((IEnumerable<char>) toSkip);

    protected void SkipWhitespaceAndCharacters(IEnumerable<char> toSkip)
    {
      while (!this.IsComplete && MvxParser.IsWhiteSpaceOrCharacter(this.CurrentChar, toSkip))
        this.MoveNext();
    }

    protected void SkipWhitespaceAndCharacters(Dictionary<char, bool> toSkip)
    {
      while (!this.IsComplete && MvxParser.IsWhiteSpaceOrCharacter(this.CurrentChar, toSkip))
        this.MoveNext();
    }

    protected void SkipWhitespace()
    {
      while (!this.IsComplete && char.IsWhiteSpace(this.CurrentChar))
        this.MoveNext();
    }

    private static bool IsWhiteSpaceOrCharacter(char charToTest, Dictionary<char, bool> toSkip) => char.IsWhiteSpace(charToTest) || toSkip.ContainsKey(charToTest);

    private static bool IsWhiteSpaceOrCharacter(char charToTest, IEnumerable<char> toSkip) => char.IsWhiteSpace(charToTest) || toSkip.Contains<char>(charToTest);

    protected object ReadValue()
    {
      object obj;
      if (!this.TryReadValue(MvxParser.AllowNonQuotedText.Allow, out obj))
        throw new MvxException("Unable to read value");
      return obj;
    }

    protected bool TryReadValue(MvxParser.AllowNonQuotedText allowNonQuotedText, out object value)
    {
      this.SkipWhitespace();
      if (this.IsComplete)
        throw new MvxException("Unexpected termination while reading value in {0}", new object[1]
        {
          (object) this.FullText
        });
      char currentChar = this.CurrentChar;
      switch (currentChar)
      {
        case '"':
        case '\'':
          value = (object) this.ReadQuotedString();
          return true;
        default:
          if (char.IsDigit(currentChar) || currentChar == '-')
          {
            value = (object) this.ReadNumber();
            return true;
          }
          bool booleanValue;
          if (this.TryReadBoolean(out booleanValue))
          {
            value = (object) booleanValue;
            return true;
          }
          if (this.TryReadNull())
          {
            value = (object) null;
            return true;
          }
          if (allowNonQuotedText == MvxParser.AllowNonQuotedText.Allow)
          {
            value = (object) this.ReadTextUntil(',', ';');
            return true;
          }
          value = (object) null;
          return false;
      }
    }

    protected bool TestKeywordInPeekString(string uppercaseKeyword, string peekString) => peekString.Length >= uppercaseKeyword.Length && (peekString.Length == uppercaseKeyword.Length || !this.IsValidMidCharacterOfCSharpName(peekString[uppercaseKeyword.Length])) && peekString.StartsWith(uppercaseKeyword);

    protected bool TryReadNull()
    {
      if (!this.TestKeywordInPeekString("NULL", this.SafePeekString(5).ToUpperInvariant()))
        return false;
      this.MoveNext(4U);
      return true;
    }

    protected bool TryReadBoolean(out bool booleanValue)
    {
      string upperInvariant = this.SafePeekString(6).ToUpperInvariant();
      if (this.TestKeywordInPeekString("TRUE", upperInvariant))
      {
        this.MoveNext(4U);
        booleanValue = true;
        return true;
      }
      if (this.TestKeywordInPeekString("FALSE", upperInvariant))
      {
        this.MoveNext(5U);
        booleanValue = false;
        return true;
      }
      booleanValue = false;
      return false;
    }

    protected string SafePeekString(int length)
    {
      int length1 = Math.Min(length, this.FullText.Length - this.CurrentIndex);
      return length1 == 0 ? string.Empty : this.FullText.Substring(this.CurrentIndex, length1);
    }

    protected ValueType ReadNumber()
    {
      StringBuilder stringBuilder = new StringBuilder();
      char currentChar1 = this.CurrentChar;
      if (currentChar1 == '-')
      {
        stringBuilder.Append(currentChar1);
        this.MoveNext();
      }
      bool decimalPeriodSeen = false;
      while (!this.IsComplete)
      {
        char currentChar2 = this.CurrentChar;
        if (currentChar2 == '.')
          decimalPeriodSeen = !decimalPeriodSeen ? true : throw new MvxException("Multiple decimal places seen in number in {0} at position {1}", new object[2]
          {
            (object) this.FullText,
            (object) this.CurrentIndex
          });
        else if (!char.IsDigit(currentChar2))
          break;
        stringBuilder.Append(currentChar2);
        this.MoveNext();
      }
      return this.NumberFromText(stringBuilder.ToString(), decimalPeriodSeen);
    }

    protected ValueType NumberFromText(string numberText) => this.NumberFromText(numberText, numberText.Contains("."));

    protected ValueType NumberFromText(string numberText, bool decimalPeriodSeen)
    {
      if (decimalPeriodSeen)
      {
        double result;
        if (double.TryParse(numberText, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return (ValueType) result;
        throw new MvxException("Failed to parse double from {0} in {1}", new object[2]
        {
          (object) numberText,
          (object) this.FullText
        });
      }
      long result1;
      if (long.TryParse(numberText, NumberStyles.AllowLeadingSign, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
        return (ValueType) result1;
      throw new MvxException("Failed to parse Int64 from {0} in {1}", new object[2]
      {
        (object) numberText,
        (object) this.FullText
      });
    }

    protected object ReadEnumerationValue(Type enumerationType, bool ignoreCase = true)
    {
      string str = this.ReadValidCSharpName();
      try
      {
        return Enum.Parse(enumerationType, str, ignoreCase);
      }
      catch (ArgumentException ex)
      {
        throw ex.MvxWrap("Problem parsing {0} from {1} in {2}", (object) enumerationType.Name, (object) str, (object) this.FullText);
      }
    }

    protected string ReadTextUntilWhitespaceOr(params char[] terminatingCharacters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (!this.IsComplete)
      {
        char currentChar = this.CurrentChar;
        if (!((IEnumerable<char>) terminatingCharacters).Contains<char>(currentChar) && !char.IsWhiteSpace(currentChar))
        {
          stringBuilder.Append(currentChar);
          this.MoveNext();
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    protected string ReadTextUntil(params char[] terminatingCharacters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (!this.IsComplete)
      {
        char currentChar = this.CurrentChar;
        if (!((IEnumerable<char>) terminatingCharacters).Contains<char>(currentChar))
        {
          stringBuilder.Append(currentChar);
          this.MoveNext();
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    protected string ReadValidCSharpName()
    {
      this.SkipWhitespace();
      char currentChar1 = this.CurrentChar;
      if (!this.IsValidFirstCharacterOfCSharpName(currentChar1))
        throw new MvxException("PropertyName must start with letter - position {0} in {1} - char {2}", new object[3]
        {
          (object) this.CurrentIndex,
          (object) this.FullText,
          (object) currentChar1
        });
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(currentChar1);
      this.MoveNext();
      while (!this.IsComplete)
      {
        char currentChar2 = this.CurrentChar;
        if (char.IsLetterOrDigit(currentChar2) || currentChar2 == '_')
        {
          stringBuilder.Append(currentChar2);
          this.MoveNext();
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    protected bool IsValidFirstCharacterOfCSharpName(char firstChar) => char.IsLetter(firstChar) || firstChar == '_';

    protected bool IsValidMidCharacterOfCSharpName(char firstChar) => char.IsLetterOrDigit(firstChar) || firstChar == '_';

    public enum AllowNonQuotedText
    {
      Allow,
      DoNotAllow,
    }
  }
}
