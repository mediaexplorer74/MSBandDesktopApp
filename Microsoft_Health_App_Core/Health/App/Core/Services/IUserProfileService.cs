// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IUserProfileService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IUserProfileService : INotifyPropertyChanged
  {
    BandUserProfile CurrentUserProfile { get; }

    bool IsBandRegistered { get; }

    bool IsRegisteredBandPaired { get; }

    DistanceUnitType DistanceUnitType { get; }

    MassUnitType MassUnitType { get; }

    TemperatureUnitType TemperatureUnitType { get; }

    string ThirdPartyPartnersPortalEndpoint { get; }

    DateTimeOffset? CreatedOn { get; }

    bool HasAnyBandBeenRegistered { get; }

    int LastKnownAge { get; }

    bool IsMemoryCachingEnabled { get; set; }

    Task<BandUserProfile> GetCloudUserProfileAsync(
      CancellationToken cancellationToken);

    Task<BandUserProfile> GetUserProfileAsync(CancellationToken cancellationToken);

    Task ImportUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken);

    Task SaveCloudUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken);

    Task SaveCloudAndBandUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken);

    Task SaveUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken,
      bool useCloudOnly = false);

    Task SetUserProfileAsync(BandUserProfile userProfile, CancellationToken cancellationToken);

    Task LinkBandToProfileAsync(CancellationToken cancellationToken);

    Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken);

    Task RefreshUserProfileAsync(CancellationToken cancellationToken);
  }
}
