// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.ValueCursor
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Globalization;

namespace NodaTime.Text
{
  internal sealed class ValueCursor : TextCursor
  {
    internal ValueCursor(string value)
      : base(value)
    {
    }

    internal bool Match(char character)
    {
      if ((int) this.Current != (int) character)
        return false;
      this.MoveNext();
      return true;
    }

    internal bool Match(string match)
    {
      if (string.CompareOrdinal(this.Value, this.Index, match, 0, match.Length) != 0)
        return false;
      this.Move(this.Index + match.Length);
      return true;
    }

    internal bool MatchCaseInsensitive(string match, CompareInfo compareInfo, bool moveOnSuccess)
    {
      if (match.Length > this.Value.Length - this.Index || compareInfo.Compare(this.Value, this.Index, match.Length, match, 0, match.Length, CompareOptions.IgnoreCase) != 0)
        return false;
      if (moveOnSuccess)
        this.Move(this.Index + match.Length);
      return true;
    }

    internal int CompareOrdinal(string match)
    {
      int length = checked (this.Value.Length - this.Index);
      if (match.Length <= length)
        return string.CompareOrdinal(this.Value, this.Index, match, 0, match.Length);
      int num = string.CompareOrdinal(this.Value, this.Index, match, 0, length);
      return num != 0 ? num : -1;
    }

    internal ParseResult<T> ParseInt64<T>(out long result)
    {
      result = 0L;
      int index = this.Index;
      bool flag = this.Current == '-';
      if (flag && !this.MoveNext())
      {
        this.Move(index);
        return ParseResult<T>.EndOfString(this);
      }
      int num = 0;
      int digit1;
      while (result < 922337203685477580L && (digit1 = this.GetDigit()) != -1)
      {
        result = result * 10L + (long) digit1;
        ++num;
        if (!this.MoveNext())
          break;
      }
      if (num == 0)
      {
        this.Move(index);
        return ParseResult<T>.MissingNumber(this);
      }
      int digit2;
      if (result >= 922337203685477580L && (digit2 = this.GetDigit()) != -1)
      {
        if (result > 922337203685477580L)
          return this.BuildNumberOutOfRangeResult<T>(index);
        if (flag && digit2 == 8)
        {
          this.MoveNext();
          result = long.MinValue;
          return (ParseResult<T>) null;
        }
        if (digit2 > 7)
          return this.BuildNumberOutOfRangeResult<T>(index);
        result = result * 10L + (long) digit2;
        this.MoveNext();
        if (this.GetDigit() != -1)
          return this.BuildNumberOutOfRangeResult<T>(index);
      }
      if (flag)
        result = -result;
      return (ParseResult<T>) null;
    }

    private ParseResult<T> BuildNumberOutOfRangeResult<T>(int startIndex)
    {
      this.Move(startIndex);
      if (this.Current == '-')
        this.MoveNext();
      while (this.GetDigit() != -1)
        this.MoveNext();
      string str = this.Value.Substring(startIndex, checked (this.Index - startIndex));
      this.Move(startIndex);
      return ParseResult<T>.ValueOutOfRange(this, (object) str);
    }

    internal bool ParseDigits(int minimumDigits, int maximumDigits, out int result)
    {
      result = 0;
      int index = this.Index;
      int num1 = index + maximumDigits;
      if (num1 >= this.Length)
        num1 = this.Length;
      for (; index < num1; ++index)
      {
        int num2 = (int) this.Value[index] - 48;
        switch (num2)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
            result = result * 10 + num2;
            continue;
          default:
            goto label_5;
        }
      }
label_5:
      if (index - this.Index < minimumDigits)
        return false;
      this.Move(index);
      return true;
    }

    internal bool ParseFraction(int maximumDigits, int scale, out int result, bool allRequired)
    {
      if (scale < maximumDigits)
        scale = maximumDigits;
      result = 0;
      int index = this.Index;
      int num1 = index + maximumDigits;
      if (num1 > this.Length)
      {
        if (allRequired)
          return false;
        num1 = this.Length;
      }
      for (; index < num1; ++index)
      {
        int num2 = (int) this.Value[index] - 48;
        switch (num2)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
            result = result * 10 + num2;
            continue;
          default:
            goto label_9;
        }
      }
label_9:
      int num3 = index - this.Index;
      if (num3 == 0)
        return false;
      result = (int) ((double) result * Math.Pow(10.0, (double) (scale - num3)));
      bool flag = !allRequired || index == num1;
      if (flag)
        this.Move(index);
      return flag;
    }

    private int GetDigit()
    {
      int current = (int) this.Current;
      switch (current)
      {
        case 48:
        case 49:
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 56:
        case 57:
          return current - 48;
        default:
          return -1;
      }
    }
  }
}
