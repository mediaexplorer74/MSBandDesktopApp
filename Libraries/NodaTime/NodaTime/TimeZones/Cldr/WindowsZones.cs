// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.Cldr.WindowsZones
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NodaTime.TimeZones.Cldr
{
  [Immutable]
  public sealed class WindowsZones
  {
    private readonly string version;
    private readonly string tzdbVersion;
    private readonly string windowsVersion;
    private readonly ReadOnlyCollection<MapZone> mapZones;
    private readonly NodaReadOnlyDictionary<string, string> primaryMapping;

    public string Version => this.version;

    public string TzdbVersion => this.tzdbVersion;

    public string WindowsVersion => this.windowsVersion;

    public IList<MapZone> MapZones => (IList<MapZone>) this.mapZones;

    public IDictionary<string, string> PrimaryMapping => (IDictionary<string, string>) this.primaryMapping;

    internal WindowsZones(
      string version,
      string tzdbVersion,
      string windowsVersion,
      IList<MapZone> mapZones)
      : this(Preconditions.CheckNotNull<string>(version, nameof (version)), Preconditions.CheckNotNull<string>(tzdbVersion, nameof (tzdbVersion)), Preconditions.CheckNotNull<string>(windowsVersion, nameof (windowsVersion)), new ReadOnlyCollection<MapZone>((IList<MapZone>) new List<MapZone>((IEnumerable<MapZone>) Preconditions.CheckNotNull<IList<MapZone>>(mapZones, nameof (mapZones)))))
    {
    }

    private WindowsZones(
      string version,
      string tzdbVersion,
      string windowsVersion,
      ReadOnlyCollection<MapZone> mapZones)
    {
      this.version = version;
      this.tzdbVersion = tzdbVersion;
      this.windowsVersion = windowsVersion;
      this.mapZones = mapZones;
      this.primaryMapping = new NodaReadOnlyDictionary<string, string>((IDictionary<string, string>) mapZones.Where<MapZone>((Func<MapZone, bool>) (z => z.Territory == "001")).ToDictionary<MapZone, string, string>((Func<MapZone, string>) (z => z.WindowsId), (Func<MapZone, string>) (z => z.TzdbIds.Single<string>())));
    }

    private WindowsZones(
      string version,
      NodaReadOnlyDictionary<string, string> primaryMapping)
    {
      this.version = version;
      this.windowsVersion = "Unknown";
      this.tzdbVersion = "Unknown";
      this.primaryMapping = primaryMapping;
      List<MapZone> mapZoneList = new List<MapZone>(primaryMapping.Count);
      foreach (KeyValuePair<string, string> keyValuePair in primaryMapping)
        mapZoneList.Add(new MapZone(keyValuePair.Key, "001", (IList<string>) new string[1]
        {
          keyValuePair.Value
        }));
      this.mapZones = new ReadOnlyCollection<MapZone>((IList<MapZone>) mapZoneList);
    }

    internal static WindowsZones FromPrimaryMapping(
      string version,
      IDictionary<string, string> mappings)
    {
      return new WindowsZones(Preconditions.CheckNotNull<string>(version, nameof (version)), new NodaReadOnlyDictionary<string, string>(Preconditions.CheckNotNull<IDictionary<string, string>>(mappings, nameof (mappings))));
    }

    internal static WindowsZones Read(IDateTimeZoneReader reader)
    {
      string version = reader.ReadString();
      string tzdbVersion = reader.ReadString();
      string windowsVersion = reader.ReadString();
      int length = reader.ReadCount();
      MapZone[] mapZoneArray = new MapZone[length];
      int index = 0;
      while (index < length)
      {
        mapZoneArray[index] = MapZone.Read(reader);
        checked { ++index; }
      }
      return new WindowsZones(version, tzdbVersion, windowsVersion, new ReadOnlyCollection<MapZone>((IList<MapZone>) mapZoneArray));
    }

    internal void Write(IDateTimeZoneWriter writer)
    {
      writer.WriteString(this.version);
      writer.WriteString(this.tzdbVersion);
      writer.WriteString(this.windowsVersion);
      writer.WriteCount(this.mapZones.Count);
      foreach (MapZone mapZone in this.mapZones)
        mapZone.Write(writer);
    }
  }
}
