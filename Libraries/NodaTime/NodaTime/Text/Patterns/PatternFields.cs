// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.Patterns.PatternFields
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Text.Patterns
{
  [Flags]
  internal enum PatternFields
  {
    None = 0,
    Sign = 1,
    Hours12 = 2,
    Hours24 = 4,
    Minutes = 8,
    Seconds = 16, // 0x00000010
    FractionalSeconds = 32, // 0x00000020
    AmPm = 64, // 0x00000040
    Year = 128, // 0x00000080
    YearTwoDigits = 256, // 0x00000100
    YearOfEra = 512, // 0x00000200
    MonthOfYearNumeric = 1024, // 0x00000400
    MonthOfYearText = 2048, // 0x00000800
    DayOfMonth = 4096, // 0x00001000
    DayOfWeek = 8192, // 0x00002000
    Era = 16384, // 0x00004000
    Calendar = 32768, // 0x00008000
    Zone = 65536, // 0x00010000
    ZoneAbbreviation = 131072, // 0x00020000
    EmbeddedOffset = 262144, // 0x00040000
    TotalDuration = 524288, // 0x00080000
    AllTimeFields = AmPm | FractionalSeconds | Seconds | Minutes | Hours24 | Hours12, // 0x0000007E
    AllDateFields = Calendar | Era | DayOfWeek | DayOfMonth | MonthOfYearText | MonthOfYearNumeric | YearOfEra | YearTwoDigits | Year, // 0x0000FF80
  }
}
