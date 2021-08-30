// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.GeolocationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Geolocator.Plugin.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class GeolocationService : IGeolocationService
  {
    private const uint LocationAccuracyMeters = 1000;
    private readonly IGeolocator geolocator;

    public GeolocationService(IGeolocator geolocator) => this.geolocator = geolocator;

    public bool IsLocationAvailable => this.geolocator.IsGeolocationAvailable && this.geolocator.IsGeolocationEnabled;

    public async Task<PortableGeoposition> GetGeopositionAsync(
      CancellationToken token)
    {
      this.geolocator.DesiredAccuracy = 1000.0;
      Position positionAsync = await this.geolocator.GetPositionAsync(int.MaxValue, new CancellationToken?(token));
      return new PortableGeoposition(positionAsync.Latitude, positionAsync.Longitude);
    }
  }
}
