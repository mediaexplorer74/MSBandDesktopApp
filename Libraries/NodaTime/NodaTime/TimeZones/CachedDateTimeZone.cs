// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.CachedDateTimeZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones.IO;
using NodaTime.Utility;

namespace NodaTime.TimeZones
{
  internal sealed class CachedDateTimeZone : DateTimeZone
  {
    private readonly IZoneIntervalMap map;
    private readonly DateTimeZone timeZone;

    private CachedDateTimeZone(DateTimeZone timeZone, IZoneIntervalMap map)
      : base(timeZone.Id, false, timeZone.MinOffset, timeZone.MaxOffset)
    {
      this.timeZone = timeZone;
      this.map = map;
    }

    internal DateTimeZone TimeZone => this.timeZone;

    internal static DateTimeZone ForZone(DateTimeZone timeZone)
    {
      Preconditions.CheckNotNull<DateTimeZone>(timeZone, nameof (timeZone));
      return timeZone is CachedDateTimeZone || timeZone.IsFixed ? timeZone : (DateTimeZone) new CachedDateTimeZone(timeZone, CachingZoneIntervalMap.CacheMap((IZoneIntervalMap) timeZone, CachingZoneIntervalMap.CacheType.Hashtable));
    }

    public override ZoneInterval GetZoneInterval(Instant instant) => this.map.GetZoneInterval(instant);

    internal void WriteLegacy(LegacyDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneWriter>(writer, nameof (writer));
      writer.WriteTimeZone(this.timeZone);
    }

    internal static DateTimeZone ReadLegacy(LegacyDateTimeZoneReader reader, string id)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneReader>(reader, nameof (reader));
      return CachedDateTimeZone.ForZone(reader.ReadTimeZone(id));
    }

    protected override bool EqualsImpl(DateTimeZone zone) => this.TimeZone.Equals(((CachedDateTimeZone) zone).TimeZone);

    public override int GetHashCode() => this.TimeZone.GetHashCode();
  }
}
