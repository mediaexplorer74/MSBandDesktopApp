// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.DesktopApplicationPlatformProvider
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Band.Tiles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Band
{
  internal sealed class DesktopApplicationPlatformProvider : IApplicationPlatformProvider
  {
    private static readonly Guid applicationId = Guid.Parse("00000000000000111100000000000000");
    private static readonly IApplicationPlatformProvider current = (IApplicationPlatformProvider) new DesktopApplicationPlatformProvider();

    public static IApplicationPlatformProvider Current => DesktopApplicationPlatformProvider.current;

    public Task<Guid> GetApplicationIdAsync(CancellationToken token) => Task.FromResult<Guid>(DesktopApplicationPlatformProvider.applicationId);

    public Task<bool> GetAddTileConsentAsync(BandTile tile, CancellationToken token) => Task.FromResult<bool>(true);

    public UserConsent GetCurrentSensorConsent(Type sensorType) => UserConsent.Granted;

    public Task<bool> RequestSensorConsentAsync(
      Type sensorType,
      string prompt,
      CancellationToken token)
    {
      return Task.FromResult<bool>(true);
    }
  }
}
