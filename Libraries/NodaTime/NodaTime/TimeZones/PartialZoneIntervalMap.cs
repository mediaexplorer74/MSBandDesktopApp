// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.PartialZoneIntervalMap
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Collections.Generic;

namespace NodaTime.TimeZones
{
  internal sealed class PartialZoneIntervalMap
  {
    private readonly IZoneIntervalMap map;
    private readonly Instant start;
    private readonly Instant end;

    internal Instant Start => this.start;

    internal Instant End => this.end;

    internal PartialZoneIntervalMap(Instant start, Instant end, IZoneIntervalMap map)
    {
      this.start = start;
      this.end = end;
      this.map = map;
    }

    internal static PartialZoneIntervalMap ForZoneInterval(
      string name,
      Instant start,
      Instant end,
      Offset wallOffset,
      Offset savings)
    {
      return PartialZoneIntervalMap.ForZoneInterval(new ZoneInterval(name, start, end, wallOffset, savings));
    }

    internal static PartialZoneIntervalMap ForZoneInterval(
      ZoneInterval interval)
    {
      return new PartialZoneIntervalMap(interval.Start, interval.End, (IZoneIntervalMap) new SingleZoneIntervalMap(interval));
    }

    internal ZoneInterval GetZoneInterval(Instant instant)
    {
      ZoneInterval zoneInterval = this.map.GetZoneInterval(instant);
      if (zoneInterval.Start < this.Start)
        zoneInterval = zoneInterval.WithStart(this.Start);
      if (zoneInterval.End > this.End)
        zoneInterval = zoneInterval.WithEnd(this.End);
      return zoneInterval;
    }

    private bool IsSingleInterval => this.map.GetZoneInterval(this.Start).End >= this.End;

    internal PartialZoneIntervalMap WithStart(Instant start) => new PartialZoneIntervalMap(start, this.End, this.map);

    internal PartialZoneIntervalMap WithEnd(Instant end) => new PartialZoneIntervalMap(this.Start, end, this.map);

    internal static IZoneIntervalMap ConvertToFullMap(
      IEnumerable<PartialZoneIntervalMap> maps)
    {
      List<PartialZoneIntervalMap> partialZoneIntervalMapList = new List<PartialZoneIntervalMap>();
      PartialZoneIntervalMap partialZoneIntervalMap = (PartialZoneIntervalMap) null;
      foreach (PartialZoneIntervalMap map in maps)
      {
        if (partialZoneIntervalMap == null)
        {
          partialZoneIntervalMap = map;
        }
        else
        {
          ZoneInterval zoneInterval1 = partialZoneIntervalMap.GetZoneInterval(partialZoneIntervalMap.End - Duration.Epsilon);
          ZoneInterval zoneInterval2 = map.GetZoneInterval(map.Start);
          if (!zoneInterval1.EqualIgnoreBounds(zoneInterval2))
          {
            partialZoneIntervalMapList.Add(partialZoneIntervalMap);
            partialZoneIntervalMap = map;
          }
          else if (partialZoneIntervalMap.IsSingleInterval && map.IsSingleInterval)
            partialZoneIntervalMap = PartialZoneIntervalMap.ForZoneInterval(zoneInterval1.WithEnd(map.End));
          else if (partialZoneIntervalMap.IsSingleInterval)
          {
            partialZoneIntervalMapList.Add(PartialZoneIntervalMap.ForZoneInterval(zoneInterval1.WithEnd(zoneInterval2.End)));
            partialZoneIntervalMap = map.WithStart(zoneInterval2.End);
          }
          else if (map.IsSingleInterval)
          {
            partialZoneIntervalMapList.Add(partialZoneIntervalMap.WithEnd(zoneInterval1.Start));
            partialZoneIntervalMap = PartialZoneIntervalMap.ForZoneInterval(zoneInterval2.WithStart(zoneInterval1.Start));
          }
          else
          {
            partialZoneIntervalMapList.Add(partialZoneIntervalMap.WithEnd(zoneInterval1.Start));
            partialZoneIntervalMapList.Add(PartialZoneIntervalMap.ForZoneInterval(zoneInterval1.WithEnd(zoneInterval2.End)));
            partialZoneIntervalMap = map.WithStart(zoneInterval2.End);
          }
        }
      }
      partialZoneIntervalMapList.Add(partialZoneIntervalMap);
      return (IZoneIntervalMap) new PartialZoneIntervalMap.CombinedPartialZoneIntervalMap(partialZoneIntervalMapList.ToArray());
    }

    private class CombinedPartialZoneIntervalMap : IZoneIntervalMap
    {
      private readonly PartialZoneIntervalMap[] partialMaps;

      internal CombinedPartialZoneIntervalMap(PartialZoneIntervalMap[] partialMaps) => this.partialMaps = partialMaps;

      public ZoneInterval GetZoneInterval(Instant instant)
      {
        foreach (PartialZoneIntervalMap partialMap in this.partialMaps)
        {
          if (instant < partialMap.End)
            return partialMap.GetZoneInterval(instant);
        }
        throw new InvalidOperationException("Instant not contained in any map");
      }
    }
  }
}
