// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.BclConversions
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Utility
{
  public static class BclConversions
  {
    public static DayOfWeek ToDayOfWeek(IsoDayOfWeek isoDayOfWeek)
    {
      if (isoDayOfWeek < IsoDayOfWeek.Monday || isoDayOfWeek > IsoDayOfWeek.Sunday)
        throw new ArgumentOutOfRangeException(nameof (isoDayOfWeek));
      return isoDayOfWeek != IsoDayOfWeek.Sunday ? (DayOfWeek) isoDayOfWeek : DayOfWeek.Sunday;
    }

    public static IsoDayOfWeek ToIsoDayOfWeek(DayOfWeek dayOfWeek)
    {
      if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
        throw new ArgumentOutOfRangeException(nameof (dayOfWeek));
      return dayOfWeek != DayOfWeek.Sunday ? (IsoDayOfWeek) dayOfWeek : IsoDayOfWeek.Sunday;
    }
  }
}
