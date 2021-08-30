// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.YearMonthDay
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal struct YearMonthDay
  {
    private readonly int year;
    private readonly int month;
    private readonly int day;

    internal int Year => this.year;

    internal int Month => this.month;

    internal int Day => this.day;

    internal YearMonthDay(int year, int month, int day)
    {
      this.year = year;
      this.month = month;
      this.day = day;
    }
  }
}
