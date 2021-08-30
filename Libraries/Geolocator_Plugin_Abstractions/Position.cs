// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.Abstractions.Position
// Assembly: Geolocator.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DCD076A-1B75-4641-A574-7C3E2F83EFFF
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin_Abstractions.dll

using System;

namespace Geolocator.Plugin.Abstractions
{
  public class Position
  {
    public Position()
    {
    }

    public Position(Position position)
    {
      this.Timestamp = position != null ? position.Timestamp : throw new ArgumentNullException(nameof (position));
      this.Latitude = position.Latitude;
      this.Longitude = position.Longitude;
      this.Altitude = position.Altitude;
      this.AltitudeAccuracy = position.AltitudeAccuracy;
      this.Accuracy = position.Accuracy;
      this.Heading = position.Heading;
      this.Speed = position.Speed;
    }

    public DateTimeOffset Timestamp { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Altitude { get; set; }

    public double Accuracy { get; set; }

    public double AltitudeAccuracy { get; set; }

    public double Heading { get; set; }

    public double Speed { get; set; }
  }
}
