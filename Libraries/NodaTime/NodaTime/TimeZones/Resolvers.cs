// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.Resolvers
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Utility;
using System;

namespace NodaTime.TimeZones
{
  public static class Resolvers
  {
    public static readonly AmbiguousTimeResolver ReturnEarlier = (AmbiguousTimeResolver) ((earlier, later) => earlier);
    public static readonly AmbiguousTimeResolver ReturnLater = (AmbiguousTimeResolver) ((earlier, later) => later);
    public static readonly AmbiguousTimeResolver ThrowWhenAmbiguous = (AmbiguousTimeResolver) ((earlier, later) =>
    {
      throw new AmbiguousTimeException(earlier, later);
    });
    public static readonly SkippedTimeResolver ReturnEndOfIntervalBefore = (SkippedTimeResolver) ((local, zone, before, after) => new ZonedDateTime(new LocalDateTime(before.LocalEnd - Duration.Epsilon, local.Calendar), before.WallOffset, zone));
    public static readonly SkippedTimeResolver ReturnStartOfIntervalAfter = (SkippedTimeResolver) ((local, zone, before, after) =>
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Preconditions.CheckNotNull<ZoneInterval>(before, nameof (before));
      Preconditions.CheckNotNull<ZoneInterval>(after, nameof (after));
      return new ZonedDateTime(new LocalDateTime(after.LocalStart, local.Calendar), after.WallOffset, zone);
    });
    public static readonly SkippedTimeResolver ThrowWhenSkipped = (SkippedTimeResolver) ((local, zone, before, after) =>
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Preconditions.CheckNotNull<ZoneInterval>(before, nameof (before));
      Preconditions.CheckNotNull<ZoneInterval>(after, nameof (after));
      throw new SkippedTimeException(local, zone);
    });
    public static readonly ZoneLocalMappingResolver StrictResolver = Resolvers.CreateMappingResolver(Resolvers.ThrowWhenAmbiguous, Resolvers.ThrowWhenSkipped);
    public static readonly ZoneLocalMappingResolver LenientResolver = Resolvers.CreateMappingResolver(Resolvers.ReturnLater, Resolvers.ReturnStartOfIntervalAfter);

    public static ZoneLocalMappingResolver CreateMappingResolver(
      [NotNull] AmbiguousTimeResolver ambiguousTimeResolver,
      [NotNull] SkippedTimeResolver skippedTimeResolver)
    {
      Preconditions.CheckNotNull<AmbiguousTimeResolver>(ambiguousTimeResolver, nameof (ambiguousTimeResolver));
      Preconditions.CheckNotNull<SkippedTimeResolver>(skippedTimeResolver, nameof (skippedTimeResolver));
      return (ZoneLocalMappingResolver) (mapping =>
      {
        Preconditions.CheckNotNull<ZoneLocalMapping>(mapping, nameof (mapping));
        switch (mapping.Count)
        {
          case 0:
            return skippedTimeResolver(mapping.LocalDateTime, mapping.Zone, mapping.EarlyInterval, mapping.LateInterval);
          case 1:
            return mapping.First();
          case 2:
            return ambiguousTimeResolver(mapping.First(), mapping.Last());
          default:
            throw new InvalidOperationException("Mapping has count outside range 0-2; should not happen.");
        }
      });
    }
  }
}
