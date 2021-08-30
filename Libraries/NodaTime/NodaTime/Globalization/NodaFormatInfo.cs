// Decompiled with JetBrains decompiler
// Type: NodaTime.Globalization.NodaFormatInfo
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Calendars;
using NodaTime.Properties;
using NodaTime.Text;
using NodaTime.Text.Patterns;
using NodaTime.TimeZones;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace NodaTime.Globalization
{
  internal sealed class NodaFormatInfo
  {
    private static readonly string[] ShortInvariantMonthNames = (string[]) CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedMonthNames.Clone();
    private static readonly string[] LongInvariantMonthNames = (string[]) CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Clone();
    private readonly object fieldLock = new object();
    private FixedFormatInfoPatternParser<Duration> durationPatternParser;
    private FixedFormatInfoPatternParser<Offset> offsetPatternParser;
    private FixedFormatInfoPatternParser<Instant> instantPatternParser;
    private FixedFormatInfoPatternParser<LocalTime> localTimePatternParser;
    private FixedFormatInfoPatternParser<LocalDate> localDatePatternParser;
    private FixedFormatInfoPatternParser<LocalDateTime> localDateTimePatternParser;
    private FixedFormatInfoPatternParser<OffsetDateTime> offsetDateTimePatternParser;
    private FixedFormatInfoPatternParser<ZonedDateTime> zonedDateTimePatternParser;
    public static readonly NodaFormatInfo InvariantInfo = new NodaFormatInfo(CultureInfo.InvariantCulture);
    private static readonly NodaTime.Utility.Cache<CultureInfo, NodaFormatInfo> Cache = new NodaTime.Utility.Cache<CultureInfo, NodaFormatInfo>(500, (Func<CultureInfo, NodaFormatInfo>) (culture => new NodaFormatInfo(culture)), (IEqualityComparer<CultureInfo>) new ReferenceEqualityComparer<CultureInfo>());
    private readonly CultureInfo cultureInfo;
    private readonly string dateSeparator;
    private readonly string timeSeparator;
    private IList<string> longMonthNames;
    private IList<string> longMonthGenitiveNames;
    private IList<string> longDayNames;
    private IList<string> shortMonthNames;
    private IList<string> shortMonthGenitiveNames;
    private IList<string> shortDayNames;
    private readonly Dictionary<Era, NodaFormatInfo.EraDescription> eraDescriptions;

    internal NodaFormatInfo(CultureInfo cultureInfo)
    {
      Preconditions.CheckNotNull<CultureInfo>(cultureInfo, nameof (cultureInfo));
      this.cultureInfo = cultureInfo;
      this.eraDescriptions = new Dictionary<Era, NodaFormatInfo.EraDescription>();
      this.dateSeparator = DateTime.MinValue.ToString("%/", (IFormatProvider) cultureInfo);
      this.timeSeparator = DateTime.MinValue.ToString("%:", (IFormatProvider) cultureInfo);
    }

    private void EnsureMonthsInitialized()
    {
      lock (this.fieldLock)
      {
        if (this.longMonthNames != null)
          return;
        this.longMonthNames = NodaFormatInfo.ConvertMonthArray(this.cultureInfo.DateTimeFormat.MonthNames);
        this.shortMonthNames = NodaFormatInfo.ConvertMonthArray(this.cultureInfo.DateTimeFormat.AbbreviatedMonthNames);
        this.longMonthGenitiveNames = this.ConvertGenitiveMonthArray(this.longMonthNames, this.cultureInfo.DateTimeFormat.MonthGenitiveNames, NodaFormatInfo.LongInvariantMonthNames);
        this.shortMonthGenitiveNames = this.ConvertGenitiveMonthArray(this.shortMonthNames, this.cultureInfo.DateTimeFormat.AbbreviatedMonthGenitiveNames, NodaFormatInfo.ShortInvariantMonthNames);
      }
    }

    private static IList<string> ConvertMonthArray(string[] monthNames)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) monthNames);
      stringList.Insert(0, (string) null);
      return (IList<string>) new ReadOnlyCollection<string>((IList<string>) stringList);
    }

    private void EnsureDaysInitialized()
    {
      lock (this.fieldLock)
      {
        if (this.longDayNames != null)
          return;
        this.longDayNames = NodaFormatInfo.ConvertDayArray(this.cultureInfo.DateTimeFormat.DayNames);
        this.shortDayNames = NodaFormatInfo.ConvertDayArray(this.cultureInfo.DateTimeFormat.AbbreviatedDayNames);
      }
    }

    private static IList<string> ConvertDayArray(string[] dayNames)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) dayNames)
      {
        dayNames[0]
      };
      stringList[0] = (string) null;
      return (IList<string>) new ReadOnlyCollection<string>((IList<string>) stringList);
    }

    private IList<string> ConvertGenitiveMonthArray(
      IList<string> nonGenitiveNames,
      string[] bclNames,
      string[] invariantNames)
    {
      if (int.TryParse(bclNames[0], out int _))
        return nonGenitiveNames;
      int index = 0;
      while (index < bclNames.Length)
      {
        if (bclNames[index] != nonGenitiveNames[checked (index + 1)] && bclNames[index] != invariantNames[index])
          return NodaFormatInfo.ConvertMonthArray(bclNames);
        checked { ++index; }
      }
      return nonGenitiveNames;
    }

    public CultureInfo CultureInfo => this.cultureInfo;

    public CompareInfo CompareInfo => this.cultureInfo.CompareInfo;

    internal FixedFormatInfoPatternParser<Duration> DurationPatternParser => this.EnsureFixedFormatInitialized<Duration>(ref this.durationPatternParser, (Func<IPatternParser<Duration>>) (() => (IPatternParser<Duration>) new NodaTime.Text.DurationPatternParser()));

    internal FixedFormatInfoPatternParser<Offset> OffsetPatternParser => this.EnsureFixedFormatInitialized<Offset>(ref this.offsetPatternParser, (Func<IPatternParser<Offset>>) (() => (IPatternParser<Offset>) new NodaTime.Text.OffsetPatternParser()));

    internal FixedFormatInfoPatternParser<Instant> InstantPatternParser => this.EnsureFixedFormatInitialized<Instant>(ref this.instantPatternParser, (Func<IPatternParser<Instant>>) (() => (IPatternParser<Instant>) new NodaTime.Text.InstantPatternParser("MinInstant", "MaxInstant")));

    internal FixedFormatInfoPatternParser<LocalTime> LocalTimePatternParser => this.EnsureFixedFormatInitialized<LocalTime>(ref this.localTimePatternParser, (Func<IPatternParser<LocalTime>>) (() => (IPatternParser<LocalTime>) new NodaTime.Text.LocalTimePatternParser(LocalTime.Midnight)));

    internal FixedFormatInfoPatternParser<LocalDate> LocalDatePatternParser => this.EnsureFixedFormatInitialized<LocalDate>(ref this.localDatePatternParser, (Func<IPatternParser<LocalDate>>) (() => (IPatternParser<LocalDate>) new NodaTime.Text.LocalDatePatternParser(LocalDatePattern.DefaultTemplateValue)));

    internal FixedFormatInfoPatternParser<LocalDateTime> LocalDateTimePatternParser => this.EnsureFixedFormatInitialized<LocalDateTime>(ref this.localDateTimePatternParser, (Func<IPatternParser<LocalDateTime>>) (() => (IPatternParser<LocalDateTime>) new NodaTime.Text.LocalDateTimePatternParser(LocalDateTimePattern.DefaultTemplateValue)));

    internal FixedFormatInfoPatternParser<OffsetDateTime> OffsetDateTimePatternParser => this.EnsureFixedFormatInitialized<OffsetDateTime>(ref this.offsetDateTimePatternParser, (Func<IPatternParser<OffsetDateTime>>) (() => (IPatternParser<OffsetDateTime>) new NodaTime.Text.OffsetDateTimePatternParser(OffsetDateTimePattern.DefaultTemplateValue)));

    internal FixedFormatInfoPatternParser<ZonedDateTime> ZonedDateTimePatternParser => this.EnsureFixedFormatInitialized<ZonedDateTime>(ref this.zonedDateTimePatternParser, (Func<IPatternParser<ZonedDateTime>>) (() => (IPatternParser<ZonedDateTime>) new NodaTime.Text.ZonedDateTimePatternParser(ZonedDateTimePattern.DefaultTemplateValue, Resolvers.StrictResolver, (IDateTimeZoneProvider) null)));

    private FixedFormatInfoPatternParser<T> EnsureFixedFormatInitialized<T>(
      ref FixedFormatInfoPatternParser<T> field,
      Func<IPatternParser<T>> patternParserFactory)
    {
      lock (this.fieldLock)
      {
        if (field == null)
          field = new FixedFormatInfoPatternParser<T>(patternParserFactory(), this);
        return field;
      }
    }

    public IList<string> LongMonthNames
    {
      get
      {
        this.EnsureMonthsInitialized();
        return this.longMonthNames;
      }
    }

    public IList<string> ShortMonthNames
    {
      get
      {
        this.EnsureMonthsInitialized();
        return this.shortMonthNames;
      }
    }

    public IList<string> LongMonthGenitiveNames
    {
      get
      {
        this.EnsureMonthsInitialized();
        return this.longMonthGenitiveNames;
      }
    }

    public IList<string> ShortMonthGenitiveNames
    {
      get
      {
        this.EnsureMonthsInitialized();
        return this.shortMonthGenitiveNames;
      }
    }

    public IList<string> LongDayNames
    {
      get
      {
        this.EnsureDaysInitialized();
        return this.longDayNames;
      }
    }

    public IList<string> ShortDayNames
    {
      get
      {
        this.EnsureDaysInitialized();
        return this.shortDayNames;
      }
    }

    public NumberFormatInfo NumberFormat => this.cultureInfo.NumberFormat;

    public DateTimeFormatInfo DateTimeFormat => this.cultureInfo.DateTimeFormat;

    public string PositiveSign => this.NumberFormat.PositiveSign;

    public string NegativeSign => this.NumberFormat.NegativeSign;

    public string TimeSeparator => this.timeSeparator;

    public string DateSeparator => this.dateSeparator;

    public string AMDesignator => this.DateTimeFormat.AMDesignator;

    public string PMDesignator => this.DateTimeFormat.PMDesignator;

    public IList<string> GetEraNames([NotNull] Era era)
    {
      Preconditions.CheckNotNull<Era>(era, nameof (era));
      return (IList<string>) this.GetEraDescription(era).AllNames;
    }

    public string GetEraPrimaryName([NotNull] Era era)
    {
      Preconditions.CheckNotNull<Era>(era, nameof (era));
      return this.GetEraDescription(era).PrimaryName;
    }

    private NodaFormatInfo.EraDescription GetEraDescription(Era era)
    {
      lock (this.eraDescriptions)
      {
        NodaFormatInfo.EraDescription eraDescription;
        if (!this.eraDescriptions.TryGetValue(era, out eraDescription))
        {
          eraDescription = NodaFormatInfo.EraDescription.ForEra(era, this.cultureInfo);
          this.eraDescriptions[era] = eraDescription;
        }
        return eraDescription;
      }
    }

    public static NodaFormatInfo CurrentInfo => NodaFormatInfo.GetInstance((IFormatProvider) Thread.CurrentThread.CurrentCulture);

    public string OffsetPatternFull => PatternResources.ResourceManager.GetString(nameof (OffsetPatternFull), this.cultureInfo);

    public string OffsetPatternLong => PatternResources.ResourceManager.GetString(nameof (OffsetPatternLong), this.cultureInfo);

    public string OffsetPatternMedium => PatternResources.ResourceManager.GetString(nameof (OffsetPatternMedium), this.cultureInfo);

    public string OffsetPatternShort => PatternResources.ResourceManager.GetString(nameof (OffsetPatternShort), this.cultureInfo);

    internal static void ClearCache() => NodaFormatInfo.Cache.Clear();

    internal static NodaFormatInfo GetFormatInfo(CultureInfo cultureInfo)
    {
      Preconditions.CheckNotNull<CultureInfo>(cultureInfo, nameof (cultureInfo));
      if (cultureInfo == CultureInfo.InvariantCulture)
        return NodaFormatInfo.InvariantInfo;
      return !cultureInfo.IsReadOnly ? new NodaFormatInfo(cultureInfo) : NodaFormatInfo.Cache.GetOrAdd(cultureInfo);
    }

    public static NodaFormatInfo GetInstance(IFormatProvider provider) => provider != null && provider is CultureInfo cultureInfo ? NodaFormatInfo.GetFormatInfo(cultureInfo) : NodaFormatInfo.GetInstance((IFormatProvider) CultureInfo.CurrentCulture);

    public override string ToString() => "NodaFormatInfo[" + this.cultureInfo.Name + "]";

    private class EraDescription
    {
      private readonly string primaryName;
      private readonly ReadOnlyCollection<string> allNames;

      internal string PrimaryName => this.primaryName;

      internal ReadOnlyCollection<string> AllNames => this.allNames;

      private EraDescription(string primaryName, ReadOnlyCollection<string> allNames)
      {
        this.primaryName = primaryName;
        this.allNames = allNames;
      }

      internal static NodaFormatInfo.EraDescription ForEra(
        Era era,
        CultureInfo cultureInfo)
      {
        string str = PatternResources.ResourceManager.GetString(era.ResourceIdentifier, cultureInfo);
        string[] array;
        string primaryName;
        if (str == null)
        {
          array = new string[0];
          primaryName = "";
        }
        else
        {
          array = str.Split('|');
          primaryName = array[0];
          Array.Sort<string>(array, (Comparison<string>) ((x, y) => y.Length.CompareTo(x.Length)));
        }
        return new NodaFormatInfo.EraDescription(primaryName, new ReadOnlyCollection<string>((IList<string>) array));
      }
    }
  }
}
