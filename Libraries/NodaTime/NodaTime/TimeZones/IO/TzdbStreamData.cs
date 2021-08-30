// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.IO.TzdbStreamData
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones.Cldr;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace NodaTime.TimeZones.IO
{
  internal sealed class TzdbStreamData : ITzdbDataSource
  {
    private const int AcceptedVersion = 0;
    private static readonly Dictionary<TzdbStreamFieldId, Action<TzdbStreamData.Builder, TzdbStreamField>> FieldHanders = new Dictionary<TzdbStreamFieldId, Action<TzdbStreamData.Builder, TzdbStreamField>>()
    {
      {
        TzdbStreamFieldId.StringPool,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleStringPoolField(field))
      },
      {
        TzdbStreamFieldId.TimeZone,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleZoneField(field))
      },
      {
        TzdbStreamFieldId.TzdbIdMap,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleTzdbIdMapField(field))
      },
      {
        TzdbStreamFieldId.TzdbVersion,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleTzdbVersionField(field))
      },
      {
        TzdbStreamFieldId.CldrSupplementalWindowsZones,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleSupplementalWindowsZonesField(field))
      },
      {
        TzdbStreamFieldId.WindowsAdditionalStandardNameToIdMapping,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleWindowsAdditionalStandardNameToIdMappingField(field))
      },
      {
        TzdbStreamFieldId.ZoneLocations,
        (Action<TzdbStreamData.Builder, TzdbStreamField>) ((builder, field) => builder.HandleZoneLocationsField(field))
      }
    };
    private readonly IList<string> stringPool;
    private readonly string tzdbVersion;
    private readonly IDictionary<string, string> tzdbIdMap;
    private readonly WindowsZones windowsMapping;
    private readonly IDictionary<string, TzdbStreamField> zoneFields;
    private readonly IList<TzdbZoneLocation> zoneLocations;
    private readonly IDictionary<string, string> windowsAdditionalStandardNameToIdMapping;

    private TzdbStreamData(TzdbStreamData.Builder builder)
    {
      this.stringPool = TzdbStreamData.CheckNotNull<IList<string>>(builder.stringPool, "string pool");
      this.tzdbIdMap = TzdbStreamData.CheckNotNull<IDictionary<string, string>>(builder.tzdbIdMap, "TZDB alias map");
      this.tzdbVersion = TzdbStreamData.CheckNotNull<string>(builder.tzdbVersion, "TZDB version");
      this.windowsMapping = TzdbStreamData.CheckNotNull<WindowsZones>(builder.windowsMapping, "CLDR Supplemental Windows Zones");
      this.zoneFields = builder.zoneFields;
      this.zoneLocations = builder.zoneLocations;
      foreach (string key in (IEnumerable<string>) this.zoneFields.Keys)
        this.tzdbIdMap[key] = key;
      this.windowsAdditionalStandardNameToIdMapping = TzdbStreamData.CheckNotNull<IDictionary<string, string>>(builder.windowsAdditionalStandardNameToIdMapping, "Windows additional standard name to ID mapping");
    }

    public string TzdbVersion => this.tzdbVersion;

    public IDictionary<string, string> TzdbIdMap => this.tzdbIdMap;

    public WindowsZones WindowsMapping => this.windowsMapping;

    public IList<TzdbZoneLocation> ZoneLocations => this.zoneLocations;

    public DateTimeZone CreateZone(string id, string canonicalId)
    {
      using (Stream stream = this.zoneFields[canonicalId].CreateStream())
      {
        DateTimeZoneReader dateTimeZoneReader = new DateTimeZoneReader(stream, this.stringPool);
        dateTimeZoneReader.ReadString();
        DateTimeZoneWriter.DateTimeZoneType dateTimeZoneType = (DateTimeZoneWriter.DateTimeZoneType) dateTimeZoneReader.ReadByte();
        switch (dateTimeZoneType)
        {
          case DateTimeZoneWriter.DateTimeZoneType.Fixed:
            return FixedDateTimeZone.Read((IDateTimeZoneReader) dateTimeZoneReader, id);
          case DateTimeZoneWriter.DateTimeZoneType.Precalculated:
            return CachedDateTimeZone.ForZone(PrecalculatedDateTimeZone.Read((IDateTimeZoneReader) dateTimeZoneReader, id));
          default:
            throw new InvalidNodaDataException("Unknown time zone type " + (object) dateTimeZoneType);
        }
      }
    }

    public IDictionary<string, string> WindowsAdditionalStandardNameToIdMapping => this.windowsAdditionalStandardNameToIdMapping;

    private static T CheckNotNull<T>(T input, string name) where T : class => (object) input != null ? input : throw new InvalidNodaDataException("Incomplete TZDB data. Missing field: " + name);

    internal static TzdbStreamData FromStream(Stream stream)
    {
      Preconditions.CheckNotNull<Stream>(stream, nameof (stream));
      int num = new BinaryReader(stream).ReadInt32();
      if (num != 0)
        throw new InvalidNodaDataException("Unable to read stream with version " + (object) num);
      TzdbStreamData.Builder builder = new TzdbStreamData.Builder();
      foreach (TzdbStreamField readField in TzdbStreamField.ReadFields(stream))
      {
        Action<TzdbStreamData.Builder, TzdbStreamField> action;
        if (TzdbStreamData.FieldHanders.TryGetValue(readField.Id, out action))
          action(builder, readField);
      }
      return new TzdbStreamData(builder);
    }

    private class Builder
    {
      internal IList<string> stringPool;
      internal string tzdbVersion;
      internal IDictionary<string, string> tzdbIdMap;
      internal IList<TzdbZoneLocation> zoneLocations;
      internal WindowsZones windowsMapping;
      internal readonly IDictionary<string, TzdbStreamField> zoneFields = (IDictionary<string, TzdbStreamField>) new Dictionary<string, TzdbStreamField>();
      internal IDictionary<string, string> windowsAdditionalStandardNameToIdMapping;

      internal void HandleStringPoolField(TzdbStreamField field)
      {
        this.CheckSingleField(field, (object) this.stringPool);
        using (Stream stream = field.CreateStream())
        {
          DateTimeZoneReader dateTimeZoneReader = new DateTimeZoneReader(stream, (IList<string>) null);
          int length = dateTimeZoneReader.ReadCount();
          this.stringPool = (IList<string>) new string[length];
          int index = 0;
          while (index < length)
          {
            this.stringPool[index] = dateTimeZoneReader.ReadString();
            checked { ++index; }
          }
        }
      }

      internal void HandleZoneField(TzdbStreamField field)
      {
        this.CheckStringPoolPresence(field);
        using (Stream stream = field.CreateStream())
        {
          string key = new DateTimeZoneReader(stream, this.stringPool).ReadString();
          if (this.zoneFields.ContainsKey(key))
            throw new InvalidNodaDataException("Multiple definitions for zone " + key);
          this.zoneFields[key] = field;
        }
      }

      internal void HandleTzdbVersionField(TzdbStreamField field)
      {
        this.CheckSingleField(field, (object) this.tzdbVersion);
        this.tzdbVersion = field.ExtractSingleValue<string>((Func<DateTimeZoneReader, string>) (reader => reader.ReadString()), (IList<string>) null);
      }

      internal void HandleTzdbIdMapField(TzdbStreamField field)
      {
        this.CheckSingleField(field, (object) this.tzdbIdMap);
        this.tzdbIdMap = field.ExtractSingleValue<IDictionary<string, string>>((Func<DateTimeZoneReader, IDictionary<string, string>>) (reader => reader.ReadDictionary()), this.stringPool);
      }

      internal void HandleSupplementalWindowsZonesField(TzdbStreamField field)
      {
        this.CheckSingleField(field, (object) this.windowsMapping);
        this.windowsMapping = field.ExtractSingleValue<WindowsZones>(new Func<DateTimeZoneReader, WindowsZones>(WindowsZones.Read), this.stringPool);
      }

      internal void HandleWindowsAdditionalStandardNameToIdMappingField(TzdbStreamField field)
      {
        if (this.windowsMapping == null)
          throw new InvalidNodaDataException("Field " + (object) field.Id + " without earlier Windows mapping field");
        this.windowsAdditionalStandardNameToIdMapping = field.ExtractSingleValue<IDictionary<string, string>>((Func<DateTimeZoneReader, IDictionary<string, string>>) (reader => reader.ReadDictionary()), this.stringPool);
      }

      internal void HandleZoneLocationsField(TzdbStreamField field)
      {
        this.CheckSingleField(field, (object) this.zoneLocations);
        this.CheckStringPoolPresence(field);
        using (Stream stream = field.CreateStream())
        {
          DateTimeZoneReader dateTimeZoneReader = new DateTimeZoneReader(stream, this.stringPool);
          int length = dateTimeZoneReader.ReadCount();
          TzdbZoneLocation[] tzdbZoneLocationArray = new TzdbZoneLocation[length];
          int index = 0;
          while (index < length)
          {
            tzdbZoneLocationArray[index] = TzdbZoneLocation.Read((IDateTimeZoneReader) dateTimeZoneReader);
            checked { ++index; }
          }
          this.zoneLocations = (IList<TzdbZoneLocation>) tzdbZoneLocationArray;
        }
      }

      private void CheckSingleField(TzdbStreamField field, object expectedNullField)
      {
        if (expectedNullField != null)
          throw new InvalidNodaDataException("Multiple fields of ID " + (object) field.Id);
      }

      private void CheckStringPoolPresence(TzdbStreamField field)
      {
        if (this.stringPool == null)
          throw new InvalidNodaDataException("String pool must be present before field " + (object) field.Id);
      }
    }
  }
}
