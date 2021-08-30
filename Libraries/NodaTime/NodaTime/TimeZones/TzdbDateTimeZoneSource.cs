// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.TzdbDateTimeZoneSource
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.TimeZones.Cldr;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class TzdbDateTimeZoneSource : IDateTimeZoneSource
  {
    private readonly ITzdbDataSource source;
    private readonly IDictionary<string, string> timeZoneIdMap;
    private readonly ILookup<string, string> aliases;
    private readonly string version;
    private readonly IList<TzdbZoneLocation> zoneLocations;
    private readonly Dictionary<string, string> guesses = new Dictionary<string, string>();

    public static TzdbDateTimeZoneSource Default => TzdbDateTimeZoneSource.DefaultHolder.builtin;

    public static TzdbDateTimeZoneSource FromStream([NotNull] Stream stream)
    {
      Preconditions.CheckNotNull<Stream>(stream, nameof (stream));
      return new TzdbDateTimeZoneSource((ITzdbDataSource) TzdbStreamData.FromStream(stream));
    }

    private TzdbDateTimeZoneSource(ITzdbDataSource source)
    {
      Preconditions.CheckNotNull<ITzdbDataSource>(source, nameof (source));
      this.source = source;
      this.timeZoneIdMap = (IDictionary<string, string>) new NodaReadOnlyDictionary<string, string>(source.TzdbIdMap);
      this.aliases = this.timeZoneIdMap.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (pair => pair.Key != pair.Value)).OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key), (IComparer<string>) StringComparer.Ordinal).ToLookup<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>) (pair => pair.Value), (Func<KeyValuePair<string, string>, string>) (pair => pair.Key));
      this.version = source.TzdbVersion + " (mapping: " + source.WindowsMapping.Version + ")";
      IList<TzdbZoneLocation> zoneLocations = source.ZoneLocations;
      this.zoneLocations = zoneLocations == null ? (IList<TzdbZoneLocation>) null : (IList<TzdbZoneLocation>) new ReadOnlyCollection<TzdbZoneLocation>(zoneLocations);
    }

    public DateTimeZone ForId(string id)
    {
      string canonicalId;
      if (!this.timeZoneIdMap.TryGetValue(Preconditions.CheckNotNull<string>(id, nameof (id)), out canonicalId))
        throw new ArgumentException("Time zone with ID " + id + " not found in source " + this.version, nameof (id));
      return this.source.CreateZone(id, canonicalId);
    }

    [DebuggerStepThrough]
    public IEnumerable<string> GetIds() => (IEnumerable<string>) this.timeZoneIdMap.Keys;

    public string VersionId => "TZDB: " + this.version;

    public string MapTimeZoneId(TimeZoneInfo zone)
    {
      string standardName = zone.StandardName;
      string str;
      this.source.WindowsMapping.PrimaryMapping.TryGetValue(standardName, out str);
      if (str == null)
        this.source.WindowsAdditionalStandardNameToIdMapping.TryGetValue(standardName, out str);
      if (str == null)
        str = this.GuessZoneIdByTransitions(zone);
      return str;
    }

    private string GuessZoneIdByTransitions(TimeZoneInfo zone)
    {
      lock (this.guesses)
      {
        string str1;
        if (this.guesses.TryGetValue(zone.StandardName, out str1))
          return str1;
        string str2 = this.GuessZoneIdByTransitionsUncached(zone);
        this.guesses[zone.StandardName] = str2;
        return str2;
      }
    }

    internal string GuessZoneIdByTransitionsUncached(TimeZoneInfo zone)
    {
      int year = SystemClock.Instance.Now.InUtc().Year;
      Instant startOfThisYear = Instant.FromUtc(year, 1, 1, 0, 0);
      Instant startOfNextYear = Instant.FromUtc(checked (year + 1), 1, 1, 0, 0);
      List<DateTimeZone> list1 = this.WindowsMapping.PrimaryMapping.Values.Select<string, DateTimeZone>(new Func<string, DateTimeZone>(this.ForId)).ToList<DateTimeZone>();
      List<Instant> list2 = list1.SelectMany<DateTimeZone, ZoneInterval>((Func<DateTimeZone, IEnumerable<ZoneInterval>>) (z => z.GetZoneIntervals(startOfThisYear, startOfNextYear))).Select<ZoneInterval, Instant>((Func<ZoneInterval, Instant>) (zi => Instant.Max(zi.Start, startOfThisYear))).Distinct<Instant>().ToList<Instant>();
      int num1 = -1;
      DateTimeZone dateTimeZone1 = (DateTimeZone) null;
      foreach (DateTimeZone dateTimeZone2 in list1)
      {
        DateTimeZone candidate = dateTimeZone2;
        int num2 = list2.Count<Instant>((Func<Instant, bool>) (instant => Offset.FromTimeSpan(zone.GetUtcOffset(instant.ToDateTimeUtc())) == candidate.GetUtcOffset(instant)));
        if (num2 > num1)
        {
          num1 = num2;
          dateTimeZone1 = candidate;
        }
      }
      return checked (num1 * 100) / list2.Count <= 70 ? (string) null : dateTimeZone1.Id;
    }

    public ILookup<string, string> Aliases => this.aliases;

    public IDictionary<string, string> CanonicalIdMap => this.timeZoneIdMap;

    public IList<TzdbZoneLocation> ZoneLocations => this.zoneLocations != null ? this.zoneLocations : throw new InvalidOperationException("Zone location information is not available in the legacy resource format");

    public string TzdbVersion => this.source.TzdbVersion;

    public WindowsZones WindowsMapping => this.source.WindowsMapping;

    public void Validate()
    {
      foreach (KeyValuePair<string, string> canonicalId in (IEnumerable<KeyValuePair<string, string>>) this.CanonicalIdMap)
      {
        string str;
        if (!this.CanonicalIdMap.TryGetValue(canonicalId.Value, out str))
          throw new InvalidNodaDataException("Mapping for entry " + canonicalId.Key + " (" + canonicalId.Value + ") is missing");
        if (canonicalId.Value != str)
          throw new InvalidNodaDataException("Mapping for entry " + canonicalId.Key + " (" + canonicalId.Value + ") is not canonical (" + canonicalId.Value + " maps to " + str);
      }
      foreach (MapZone mapZone in (IEnumerable<MapZone>) this.source.WindowsMapping.MapZones)
      {
        if (!this.source.WindowsMapping.PrimaryMapping.ContainsKey(mapZone.WindowsId))
          throw new InvalidNodaDataException("Windows mapping for standard ID " + mapZone.WindowsId + " has no primary territory");
      }
      foreach (MapZone mapZone in (IEnumerable<MapZone>) this.source.WindowsMapping.MapZones)
      {
        foreach (string tzdbId in (IEnumerable<string>) mapZone.TzdbIds)
        {
          if (!this.CanonicalIdMap.ContainsKey(tzdbId))
            throw new InvalidNodaDataException("Windows mapping uses canonical ID " + tzdbId + " which is missing");
        }
      }
      IDictionary<string, string> standardNameToIdMapping = this.source.WindowsAdditionalStandardNameToIdMapping;
      if (standardNameToIdMapping != null)
      {
        foreach (string key in (IEnumerable<string>) standardNameToIdMapping.Values)
        {
          if (!this.CanonicalIdMap.ContainsKey(key))
            throw new InvalidNodaDataException("Windows additional standard name mapping uses canonical ID " + key + " which is missing");
        }
      }
      if (this.zoneLocations == null)
        return;
      foreach (TzdbZoneLocation zoneLocation in (IEnumerable<TzdbZoneLocation>) this.zoneLocations)
      {
        if (!this.CanonicalIdMap.ContainsKey(zoneLocation.ZoneId))
          throw new InvalidNodaDataException("Zone location " + zoneLocation.CountryName + " uses zone ID " + zoneLocation.ZoneId + " which is missing");
      }
    }

    private static class DefaultHolder
    {
      internal static readonly TzdbDateTimeZoneSource builtin = new TzdbDateTimeZoneSource(TzdbDateTimeZoneSource.DefaultHolder.LoadDefaultDataSource());

      private static ITzdbDataSource LoadDefaultDataSource()
      {
        using (Stream manifestResourceStream = typeof (TzdbDateTimeZoneSource.DefaultHolder).Assembly.GetManifestResourceStream("NodaTime.TimeZones.Tzdb.nzd"))
          return (ITzdbDataSource) TzdbStreamData.FromStream(manifestResourceStream);
      }
    }
  }
}
