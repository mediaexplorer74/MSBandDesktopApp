// Decompiled with JetBrains decompiler
// Type: NodaTime.Fields.YearsPeriodField
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Calendars;
using NodaTime.Utility;

namespace NodaTime.Fields
{
  internal sealed class YearsPeriodField : IPeriodField
  {
    private readonly YearMonthDayCalculator calculator;

    internal YearsPeriodField(YearMonthDayCalculator calculator) => this.calculator = calculator;

    public LocalInstant Add(LocalInstant localInstant, long value)
    {
      int year = this.calculator.GetYear(localInstant);
      Preconditions.CheckArgumentRange(nameof (value), value, (long) checked (this.calculator.MinYear - year), (long) checked (this.calculator.MaxYear - year));
      int num = checked ((int) value);
      return this.calculator.SetYear(localInstant, checked (num + year));
    }

    public long Subtract(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
    {
      int num = checked (this.calculator.GetYear(minuendInstant) - this.calculator.GetYear(subtrahendInstant));
      LocalInstant localInstant = this.Add(subtrahendInstant, (long) num);
      return subtrahendInstant <= minuendInstant ? (localInstant <= minuendInstant ? (long) num : (long) checked (num - 1)) : (localInstant >= minuendInstant ? (long) num : (long) checked (num + 1));
    }
  }
}
