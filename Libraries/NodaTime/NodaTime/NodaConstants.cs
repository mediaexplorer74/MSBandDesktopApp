// Decompiled with JetBrains decompiler
// Type: NodaTime.NodaConstants
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime
{
  public static class NodaConstants
  {
    public const long TicksPerMillisecond = 10000;
    public const long TicksPerSecond = 10000000;
    public const long TicksPerMinute = 600000000;
    public const long TicksPerHour = 36000000000;
    public const long TicksPerStandardDay = 864000000000;
    public const long TicksPerStandardWeek = 6048000000000;
    public const int MillisecondsPerSecond = 1000;
    public const int MillisecondsPerMinute = 60000;
    public const int MillisecondsPerHour = 3600000;
    public const int MillisecondsPerStandardDay = 86400000;
    public const int MillisecondsPerStandardWeek = 604800000;
    public const int SecondsPerMinute = 60;
    public const int SecondsPerHour = 3600;
    public const int SecondsPerStandardDay = 86400;
    public const int SecondsPerWeek = 604800;
    public const int MinutesPerHour = 60;
    public const int MinutesPerStandardDay = 1440;
    public const int MinutesPerStandardWeek = 10080;
    public const int HoursPerStandardDay = 24;
    public const int HoursPerStandardWeek = 168;
    public const int DaysPerStandardWeek = 7;
    public static readonly Instant UnixEpoch = new Instant(0L);
    public static readonly Instant BclEpoch = Instant.FromUtc(1, 1, 1, 0, 0);
  }
}
