// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.TickArithmetic
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal static class TickArithmetic
  {
    internal static int FastTicksToDays(long ticks) => (int) ((ticks >> 14) / 52734375L);

    internal static int TicksToDays(long ticks) => ticks < 0L ? (int) ((ticks - 863999999999L) / 864000000000L) : TickArithmetic.FastTicksToDays(ticks);
  }
}
