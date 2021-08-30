// Decompiled with JetBrains decompiler
// Type: NodaTime.Fields.MonthsPeriodField
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using System;

namespace NodaTime.Fields
{
  internal sealed class MonthsPeriodField : IPeriodField
  {
    private readonly YearMonthDayCalculator calculator;

    internal MonthsPeriodField(YearMonthDayCalculator calculator) => this.calculator = calculator;

    public LocalInstant Add(LocalInstant localInstant, long value)
    {
      if (value < (long) int.MinValue || value > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (value));
      return this.calculator.AddMonths(localInstant, checked ((int) value));
    }

    public long Subtract(LocalInstant minuendInstant, LocalInstant subtrahendInstant) => (long) this.calculator.MonthsBetween(minuendInstant, subtrahendInstant);
  }
}
