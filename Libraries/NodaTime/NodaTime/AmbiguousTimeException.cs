// Decompiled with JetBrains decompiler
// Type: NodaTime.AmbiguousTimeException
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Utility;
using System;

namespace NodaTime
{
  [Mutable]
  public sealed class AmbiguousTimeException : ArgumentOutOfRangeException
  {
    private readonly ZonedDateTime earlierMapping;
    private readonly ZonedDateTime laterMapping;

    internal LocalDateTime LocalDateTime => this.earlierMapping.LocalDateTime;

    public DateTimeZone Zone => this.earlierMapping.Zone;

    public ZonedDateTime EarlierMapping => this.earlierMapping;

    public ZonedDateTime LaterMapping => this.laterMapping;

    public AmbiguousTimeException(ZonedDateTime earlierMapping, ZonedDateTime laterMapping)
      : base("Local time " + (object) earlierMapping.LocalDateTime + " is ambiguous in time zone " + earlierMapping.Zone.Id)
    {
      this.earlierMapping = earlierMapping;
      this.laterMapping = laterMapping;
      Preconditions.CheckArgument(earlierMapping.Zone == laterMapping.Zone, nameof (laterMapping), "Ambiguous possible values must use the same time zone");
      Preconditions.CheckArgument(earlierMapping.LocalDateTime == laterMapping.LocalDateTime, nameof (laterMapping), "Ambiguous possible values must have the same local date/time");
    }
  }
}
