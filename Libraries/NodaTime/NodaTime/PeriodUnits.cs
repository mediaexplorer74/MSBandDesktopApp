// Decompiled with JetBrains decompiler
// Type: NodaTime.PeriodUnits
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime
{
  [Flags]
  public enum PeriodUnits
  {
    None = 0,
    Years = 1,
    Months = 2,
    Weeks = 4,
    Days = 8,
    AllDateUnits = Days | Weeks | Months | Years, // 0x0000000F
    YearMonthDay = Days | Months | Years, // 0x0000000B
    Hours = 16, // 0x00000010
    Minutes = 32, // 0x00000020
    Seconds = 64, // 0x00000040
    Milliseconds = 128, // 0x00000080
    Ticks = 256, // 0x00000100
    HourMinuteSecond = Seconds | Minutes | Hours, // 0x00000070
    AllTimeUnits = HourMinuteSecond | Ticks | Milliseconds, // 0x000001F0
    DateAndTime = AllTimeUnits | YearMonthDay, // 0x000001FB
    AllUnits = DateAndTime | Weeks, // 0x000001FF
  }
}
