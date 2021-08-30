// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.FormatHelper
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Text;

namespace NodaTime.Text
{
  internal static class FormatHelper
  {
    private const int MaximumPaddingLength = 16;
    private const int MaximumInt64Length = 19;

    internal static void LeftPad(int value, int length, StringBuilder outputBuffer)
    {
      if (length > 16)
        throw new FormatException("Too many digits");
      if (value < 0)
      {
        outputBuffer.Append('-');
        if (value == int.MinValue)
        {
          if (length > 10)
            outputBuffer.Append("000000".Substring(16 - length));
          outputBuffer.Append("2147483648");
        }
        else
          FormatHelper.LeftPad(Math.Abs(value), length, outputBuffer);
      }
      else
      {
        if (length == 1)
        {
          if (value < 10)
          {
            outputBuffer.Append((char) (48 + value));
            return;
          }
          if (value < 100)
          {
            char ch1 = (char) (48 + value / 10);
            char ch2 = (char) (48 + value % 10);
            outputBuffer.Append(ch1).Append(ch2);
            return;
          }
        }
        if (length == 2 && value < 100)
        {
          char ch3 = (char) (48 + value / 10);
          char ch4 = (char) (48 + value % 10);
          outputBuffer.Append(ch3).Append(ch4);
        }
        else if (length == 3 && value < 1000)
        {
          char ch5 = (char) (48 + value / 100 % 10);
          char ch6 = (char) (48 + value / 10 % 10);
          char ch7 = (char) (48 + value % 10);
          outputBuffer.Append(ch5).Append(ch6).Append(ch7);
        }
        else if (length == 4 && value < 10000)
        {
          char ch8 = (char) (48 + value / 1000);
          char ch9 = (char) (48 + value / 100 % 10);
          char ch10 = (char) (48 + value / 10 % 10);
          char ch11 = (char) (48 + value % 10);
          outputBuffer.Append(ch8).Append(ch9).Append(ch10).Append(ch11);
        }
        else if (length == 5 && value < 100000)
        {
          char ch12 = (char) (48 + value / 10000);
          char ch13 = (char) (48 + value / 1000 % 10);
          char ch14 = (char) (48 + value / 100 % 10);
          char ch15 = (char) (48 + value / 10 % 10);
          char ch16 = (char) (48 + value % 10);
          outputBuffer.Append(ch12).Append(ch13).Append(ch14).Append(ch15).Append(ch16);
        }
        else
        {
          char[] chArray = new char[16];
          int startIndex = 16;
          do
          {
            chArray[--startIndex] = (char) (48 + value % 10);
            value /= 10;
          }
          while (value != 0 && startIndex > 0);
          while (16 - startIndex < length)
            chArray[--startIndex] = '0';
          outputBuffer.Append(chArray, startIndex, 16 - startIndex);
        }
      }
    }

    internal static void AppendFraction(
      int value,
      int length,
      int scale,
      StringBuilder outputBuffer)
    {
      int num1 = value;
      while (scale > length)
      {
        num1 /= 10;
        checked { --scale; }
      }
      outputBuffer.Append('0', length);
      int num2 = checked (outputBuffer.Length - 1);
      for (; num1 > 0; num1 /= 10)
        outputBuffer[checked (num2--)] = checked ((char) (48 + unchecked (num1 % 10)));
    }

    internal static void AppendFractionTruncate(
      int value,
      int length,
      int scale,
      StringBuilder outputBuffer)
    {
      int num1 = value;
      while (scale > length)
      {
        num1 /= 10;
        checked { --scale; }
      }
      int repeatCount = length;
      while (repeatCount > 0 && (long) num1 % 10L == 0L)
      {
        num1 /= 10;
        checked { --repeatCount; }
      }
      if (repeatCount > 0)
      {
        outputBuffer.Append('0', repeatCount);
        int num2 = checked (outputBuffer.Length - 1);
        for (; num1 > 0; num1 /= 10)
          outputBuffer[checked (num2--)] = checked ((char) (48 + unchecked (num1 % 10)));
      }
      else
      {
        if (outputBuffer.Length <= 0 || outputBuffer[checked (outputBuffer.Length - 1)] != '.')
          return;
        checked { --outputBuffer.Length; }
      }
    }

    internal static void FormatInvariant(long value, StringBuilder outputBuffer)
    {
      if (value <= 0L)
      {
        if (value == 0L)
          outputBuffer.Append('0');
        else if (value == long.MinValue)
        {
          outputBuffer.Append("-9223372036854775808");
        }
        else
        {
          outputBuffer.Append('-');
          FormatHelper.FormatInvariant(-value, outputBuffer);
        }
      }
      else if (value < 10L)
        outputBuffer.Append((char) (48UL + (ulong) value));
      else if (value < 100L)
      {
        char ch1 = (char) (48UL + (ulong) (value / 10L));
        char ch2 = (char) (48UL + (ulong) (value % 10L));
        outputBuffer.Append(ch1).Append(ch2);
      }
      else if (value < 1000L)
      {
        char ch3 = (char) (48UL + (ulong) (value / 100L % 10L));
        char ch4 = (char) (48UL + (ulong) (value / 10L % 10L));
        char ch5 = (char) (48UL + (ulong) (value % 10L));
        outputBuffer.Append(ch3).Append(ch4).Append(ch5);
      }
      else
      {
        char[] chArray = new char[19];
        int startIndex = 19;
        do
        {
          chArray[--startIndex] = (char) (48UL + (ulong) (value % 10L));
          value /= 10L;
        }
        while (value != 0L);
        outputBuffer.Append(chArray, startIndex, 19 - startIndex);
      }
    }
  }
}
