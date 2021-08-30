// Decompiled with JetBrains decompiler
// Type: NodaTime.SkippedTimeException
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using System;

namespace NodaTime
{
  [Mutable]
  public sealed class SkippedTimeException : ArgumentOutOfRangeException
  {
    private readonly LocalDateTime localDateTime;
    private readonly DateTimeZone zone;

    public LocalDateTime LocalDateTime => this.localDateTime;

    public DateTimeZone Zone => this.zone;

    public SkippedTimeException(LocalDateTime localDateTime, DateTimeZone zone)
      : base("Local time " + (object) localDateTime + " is invalid in time zone " + zone.Id)
    {
      this.localDateTime = localDateTime;
      this.zone = zone;
    }
  }
}
