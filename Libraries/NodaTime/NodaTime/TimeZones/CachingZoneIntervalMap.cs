// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.CachingZoneIntervalMap
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System;

namespace NodaTime.TimeZones
{
  internal static class CachingZoneIntervalMap
  {
    internal static IZoneIntervalMap CacheMap(
      IZoneIntervalMap map,
      CachingZoneIntervalMap.CacheType type)
    {
      if (type == CachingZoneIntervalMap.CacheType.Hashtable)
        return (IZoneIntervalMap) new CachingZoneIntervalMap.HashArrayCache(map);
      throw new ArgumentException("The type parameter is invalid", nameof (type));
    }

    internal enum CacheType
    {
      Hashtable,
    }

    private sealed class HashArrayCache : IZoneIntervalMap
    {
      private const int CacheSize = 512;
      private const int CachePeriodMask = 511;
      private const int PeriodShift = 45;
      private readonly CachingZoneIntervalMap.HashArrayCache.HashCacheNode[] instantCache;
      private readonly IZoneIntervalMap map;

      internal HashArrayCache(IZoneIntervalMap map)
      {
        this.map = Preconditions.CheckNotNull<IZoneIntervalMap>(map, nameof (map));
        this.instantCache = new CachingZoneIntervalMap.HashArrayCache.HashCacheNode[512];
      }

      public ZoneInterval GetZoneInterval(Instant instant)
      {
        int period = checked ((int) (instant.Ticks >> 45));
        int index = period & 511;
        CachingZoneIntervalMap.HashArrayCache.HashCacheNode hashCacheNode = this.instantCache[index];
        if (hashCacheNode == null || hashCacheNode.Period != period)
        {
          hashCacheNode = CachingZoneIntervalMap.HashArrayCache.HashCacheNode.CreateNode(period, this.map);
          this.instantCache[index] = hashCacheNode;
        }
        while (hashCacheNode.Interval.Start > instant)
          hashCacheNode = hashCacheNode.Previous;
        return hashCacheNode.Interval;
      }

      private sealed class HashCacheNode
      {
        private readonly ZoneInterval interval;
        private readonly int period;
        private readonly CachingZoneIntervalMap.HashArrayCache.HashCacheNode previous;

        internal ZoneInterval Interval => this.interval;

        internal int Period => this.period;

        internal CachingZoneIntervalMap.HashArrayCache.HashCacheNode Previous => this.previous;

        internal static CachingZoneIntervalMap.HashArrayCache.HashCacheNode CreateNode(
          int period,
          IZoneIntervalMap map)
        {
          Instant instant1 = new Instant((long) period << 45);
          Instant instant2 = new Instant((long) checked (period + 1) << 45);
          ZoneInterval zoneInterval = map.GetZoneInterval(instant1);
          CachingZoneIntervalMap.HashArrayCache.HashCacheNode previous = new CachingZoneIntervalMap.HashArrayCache.HashCacheNode(zoneInterval, period, (CachingZoneIntervalMap.HashArrayCache.HashCacheNode) null);
          while (zoneInterval.End < instant2)
          {
            zoneInterval = map.GetZoneInterval(zoneInterval.End);
            previous = new CachingZoneIntervalMap.HashArrayCache.HashCacheNode(zoneInterval, period, previous);
          }
          return previous;
        }

        private HashCacheNode(
          ZoneInterval interval,
          int period,
          CachingZoneIntervalMap.HashArrayCache.HashCacheNode previous)
        {
          this.period = period;
          this.interval = interval;
          this.previous = previous;
        }
      }
    }
  }
}
