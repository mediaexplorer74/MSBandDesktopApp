// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.Cldr.MapZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NodaTime.TimeZones.Cldr
{
  [Immutable]
  public sealed class MapZone
  {
    public const string PrimaryTerritory = "001";
    public const string FixedOffsetTerritory = "ZZ";
    private readonly string windowsId;
    private readonly string territory;
    private readonly IList<string> tzdbIds;

    public string WindowsId => this.windowsId;

    public string Territory => this.territory;

    public IList<string> TzdbIds => this.tzdbIds;

    public MapZone(string windowsId, string territory, IList<string> tzdbIds)
      : this(Preconditions.CheckNotNull<string>(windowsId, nameof (windowsId)), Preconditions.CheckNotNull<string>(territory, nameof (territory)), new ReadOnlyCollection<string>((IList<string>) new List<string>((IEnumerable<string>) Preconditions.CheckNotNull<IList<string>>(tzdbIds, nameof (tzdbIds)))))
    {
    }

    private MapZone(string windowsId, string territory, ReadOnlyCollection<string> tzdbIds)
    {
      this.windowsId = windowsId;
      this.territory = territory;
      this.tzdbIds = (IList<string>) tzdbIds;
    }

    internal static MapZone Read(IDateTimeZoneReader reader)
    {
      string windowsId = reader.ReadString();
      string territory = reader.ReadString();
      int length = reader.ReadCount();
      string[] strArray = new string[length];
      int index = 0;
      while (index < length)
      {
        strArray[index] = reader.ReadString();
        checked { ++index; }
      }
      return new MapZone(windowsId, territory, new ReadOnlyCollection<string>((IList<string>) strArray));
    }

    internal void Write(IDateTimeZoneWriter writer)
    {
      writer.WriteString(this.windowsId);
      writer.WriteString(this.territory);
      writer.WriteCount(this.tzdbIds.Count);
      foreach (string tzdbId in (IEnumerable<string>) this.tzdbIds)
        writer.WriteString(tzdbId);
    }
  }
}
