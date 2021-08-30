// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.IsoYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Calendars
{
  internal sealed class IsoYearMonthDayCalculator : GregorianYearMonthDayCalculator
  {
    internal override int GetCenturyOfEra(LocalInstant localInstant) => Math.Abs(this.GetYear(localInstant)) / 100;

    internal override int GetYearOfCentury(LocalInstant localInstant) => Math.Abs(this.GetYear(localInstant)) % 100;
  }
}
