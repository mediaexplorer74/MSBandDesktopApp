// Decompiled with JetBrains decompiler
// Type: NodaTime.Fields.FixedDurationPeriodField
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Fields
{
  internal sealed class FixedDurationPeriodField : IPeriodField
  {
    internal static readonly IPeriodField Ticks = (IPeriodField) new FixedDurationPeriodField(1UL);
    internal static readonly IPeriodField Milliseconds = (IPeriodField) new FixedDurationPeriodField(10000UL);
    internal static readonly IPeriodField Seconds = (IPeriodField) new FixedDurationPeriodField(10000000UL);
    internal static readonly IPeriodField Minutes = (IPeriodField) new FixedDurationPeriodField(600000000UL);
    internal static readonly IPeriodField Hours = (IPeriodField) new FixedDurationPeriodField(36000000000UL);
    internal static readonly IPeriodField HalfDays = (IPeriodField) new FixedDurationPeriodField(432000000000UL);
    internal static readonly IPeriodField Days = (IPeriodField) new FixedDurationPeriodField(864000000000UL);
    internal static readonly IPeriodField Weeks = (IPeriodField) new FixedDurationPeriodField(6048000000000UL);
    private readonly ulong unitTicks;

    private FixedDurationPeriodField(ulong unitTicks) => this.unitTicks = unitTicks;

    public LocalInstant Add(LocalInstant localInstant, long value)
    {
      if (value > 0L)
      {
        ulong num = checked ((ulong) value * this.unitTicks);
        long ticks = localInstant.Ticks + (long) num;
        if (ticks < localInstant.Ticks)
          throw new OverflowException("Period addition overflowed.");
        return new LocalInstant(ticks);
      }
      ulong num1 = checked (unchecked (value == long.MinValue) ? 9223372036854775808UL : (ulong) Math.Abs(value) * this.unitTicks);
      long ticks1 = localInstant.Ticks - (long) num1;
      if (ticks1 > localInstant.Ticks)
        throw new OverflowException("Period addition overflowed.");
      return new LocalInstant(ticks1);
    }

    public long Subtract(LocalInstant minuendInstant, LocalInstant subtrahendInstant) => minuendInstant < subtrahendInstant ? checked (-this.Subtract(subtrahendInstant, minuendInstant)) : checked ((long) unchecked ((ulong) (minuendInstant.Ticks - subtrahendInstant.Ticks) / this.unitTicks));
  }
}
