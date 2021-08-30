// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.DateTimeZoneCache
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class DateTimeZoneCache : IDateTimeZoneProvider
  {
    private readonly object accessLock = new object();
    private readonly IDateTimeZoneSource source;
    private readonly ReadOnlyCollection<string> ids;
    private readonly IDictionary<string, DateTimeZone> timeZoneMap = (IDictionary<string, DateTimeZone>) new Dictionary<string, DateTimeZone>();
    private readonly string providerVersionId;

    public DateTimeZoneCache([NotNull] IDateTimeZoneSource source)
    {
      this.source = Preconditions.CheckNotNull<IDateTimeZoneSource>(source, nameof (source));
      this.providerVersionId = source.VersionId;
      if (this.providerVersionId == null)
        throw new InvalidDateTimeZoneSourceException("Source-returned version ID was null");
      List<string> stringList = new List<string>(source.GetIds() ?? throw new InvalidDateTimeZoneSourceException("Source-returned ID sequence was null"));
      stringList.Sort((IComparer<string>) StringComparer.Ordinal);
      this.ids = new ReadOnlyCollection<string>((IList<string>) stringList);
      foreach (string id in this.ids)
      {
        if (id == null)
          throw new InvalidDateTimeZoneSourceException("Source-returned ID sequence contained a null reference");
        this.timeZoneMap[id] = (DateTimeZone) null;
      }
    }

    public string VersionId => this.providerVersionId;

    public DateTimeZone GetSystemDefault()
    {
      TimeZoneInfo local = TimeZoneInfo.Local;
      return this[this.source.MapTimeZoneId(local) ?? throw new DateTimeZoneNotFoundException("TimeZoneInfo name " + local.StandardName + " is unknown to source " + this.providerVersionId)];
    }

    public ReadOnlyCollection<string> Ids => this.ids;

    public DateTimeZone GetZoneOrNull(string id)
    {
      Preconditions.CheckNotNull<string>(id, nameof (id));
      DateTimeZone fixedZoneOrNull = FixedDateTimeZone.GetFixedZoneOrNull(id);
      if (fixedZoneOrNull != null)
        return fixedZoneOrNull;
      lock (this.accessLock)
      {
        DateTimeZone dateTimeZone;
        if (!this.timeZoneMap.TryGetValue(id, out dateTimeZone))
          return (DateTimeZone) null;
        if (dateTimeZone == null)
        {
          dateTimeZone = this.source.ForId(id);
          this.timeZoneMap[id] = dateTimeZone != null ? dateTimeZone : throw new InvalidDateTimeZoneSourceException("Time zone " + id + " is supported by source " + this.providerVersionId + " but not returned");
        }
        return dateTimeZone;
      }
    }

    public DateTimeZone this[string id] => this.GetZoneOrNull(id) ?? throw new DateTimeZoneNotFoundException("Time zone " + id + " is unknown to source " + this.providerVersionId);
  }
}
