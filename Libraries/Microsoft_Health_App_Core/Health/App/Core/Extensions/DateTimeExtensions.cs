// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.DateTimeExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class DateTimeExtensions
  {
    public static DateTime RoundDown(this DateTime dateTime, TimeSpan interval)
    {
      long ticks1 = dateTime.Ticks;
      long ticks2 = interval.Ticks;
      long num = ticks2;
      return new DateTime(ticks1 / num * ticks2, dateTime.Kind);
    }

    public static DateTime RoundUp(this DateTime dateTime, TimeSpan interval)
    {
      long ticks1 = dateTime.Ticks;
      long ticks2 = interval.Ticks;
      long num = ticks2;
      return new DateTime((ticks1 + num - 1L) / ticks2 * ticks2, dateTime.Kind);
    }

    public static int GetAge(this DateTime dateTime)
    {
      if (!(dateTime != DateTime.MinValue))
        return -1;
      DateTime today = DateTime.Today;
      int num = today.Year - dateTime.Year;
      if (dateTime > today.AddYears(-num))
        --num;
      return num;
    }

    public static int DaysFrom(this DateTime dateTime, DateTime otherDateTime) => (int) (otherDateTime - dateTime).TotalDays;
  }
}
