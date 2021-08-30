// Decompiled with JetBrains decompiler
// Type: NodaTime.DateTimeZoneProviders
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones;
using NodaTime.Utility;

namespace NodaTime
{
  public static class DateTimeZoneProviders
  {
    private static readonly object SerializationProviderLock = new object();
    private static IDateTimeZoneProvider serializationProvider;

    public static IDateTimeZoneProvider Tzdb => (IDateTimeZoneProvider) DateTimeZoneProviders.TzdbHolder.TzdbImpl;

    public static IDateTimeZoneProvider Serialization
    {
      get
      {
        lock (DateTimeZoneProviders.SerializationProviderLock)
          return DateTimeZoneProviders.serializationProvider ?? (DateTimeZoneProviders.serializationProvider = DateTimeZoneProviders.Tzdb);
      }
      set
      {
        lock (DateTimeZoneProviders.SerializationProviderLock)
          DateTimeZoneProviders.serializationProvider = Preconditions.CheckNotNull<IDateTimeZoneProvider>(value, nameof (value));
      }
    }

    private static class TzdbHolder
    {
      internal static readonly DateTimeZoneCache TzdbImpl = new DateTimeZoneCache((IDateTimeZoneSource) TzdbDateTimeZoneSource.Default);
    }
  }
}
