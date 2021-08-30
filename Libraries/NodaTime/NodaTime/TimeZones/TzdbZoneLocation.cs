// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.TzdbZoneLocation
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class TzdbZoneLocation
  {
    private readonly int latitudeSeconds;
    private readonly int longitudeSeconds;
    private readonly string countryName;
    private readonly string countryCode;
    private readonly string comment;
    private readonly string zoneId;

    public double Latitude => (double) this.latitudeSeconds / 3600.0;

    public double Longitude => (double) this.longitudeSeconds / 3600.0;

    public string CountryName => this.countryName;

    public string CountryCode => this.countryCode;

    public string ZoneId => this.zoneId;

    public string Comment => this.comment;

    public TzdbZoneLocation(
      int latitudeSeconds,
      int longitudeSeconds,
      string countryName,
      string countryCode,
      string zoneId,
      string comment)
    {
      Preconditions.CheckArgumentRange(nameof (latitudeSeconds), latitudeSeconds, -324000, 324000);
      Preconditions.CheckArgumentRange(nameof (longitudeSeconds), longitudeSeconds, -648000, 648000);
      this.latitudeSeconds = latitudeSeconds;
      this.longitudeSeconds = longitudeSeconds;
      this.countryName = Preconditions.CheckNotNull<string>(countryName, nameof (countryName));
      this.countryCode = Preconditions.CheckNotNull<string>(countryCode, nameof (countryCode));
      this.zoneId = Preconditions.CheckNotNull<string>(zoneId, nameof (zoneId));
      this.comment = Preconditions.CheckNotNull<string>(comment, nameof (comment));
    }

    internal void Write(IDateTimeZoneWriter writer)
    {
      writer.WriteSignedCount(this.latitudeSeconds);
      writer.WriteSignedCount(this.longitudeSeconds);
      writer.WriteString(this.countryName);
      writer.WriteString(this.countryCode);
      writer.WriteString(this.zoneId);
      writer.WriteString(this.comment);
    }

    internal static TzdbZoneLocation Read(IDateTimeZoneReader reader)
    {
      int latitudeSeconds = reader.ReadSignedCount();
      int longitudeSeconds = reader.ReadSignedCount();
      string countryName = reader.ReadString();
      string countryCode = reader.ReadString();
      string zoneId = reader.ReadString();
      string comment = reader.ReadString();
      try
      {
        return new TzdbZoneLocation(latitudeSeconds, longitudeSeconds, countryName, countryCode, zoneId, comment);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidNodaDataException("Invalid zone location data in stream", (Exception) ex);
      }
    }
  }
}
