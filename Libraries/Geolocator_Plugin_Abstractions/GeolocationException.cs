// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.Abstractions.GeolocationException
// Assembly: Geolocator.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DCD076A-1B75-4641-A574-7C3E2F83EFFF
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin_Abstractions.dll

using System;

namespace Geolocator.Plugin.Abstractions
{
  public class GeolocationException : Exception
  {
    public GeolocationException(GeolocationError error)
      : base("A geolocation error occured: " + (object) error)
    {
      this.Error = Enum.IsDefined(typeof (GeolocationError), (object) error) ? error : throw new ArgumentException("error is not a valid GelocationError member", nameof (error));
    }

    public GeolocationException(GeolocationError error, Exception innerException)
      : base("A geolocation error occured: " + (object) error, innerException)
    {
      this.Error = Enum.IsDefined(typeof (GeolocationError), (object) error) ? error : throw new ArgumentException("error is not a valid GelocationError member", nameof (error));
    }

    public GeolocationError Error { get; private set; }
  }
}
