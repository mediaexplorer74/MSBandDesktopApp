// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.DesignFirstTimeUseViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public sealed class DesignFirstTimeUseViewModel : TileFirstTimeUseViewModel
  {
    public DesignFirstTimeUseViewModel()
      : base((ILauncherService) new DesignFirstTimeUseViewModel.DesignPromptService(), (IUserProfileService) new DesignFirstTimeUseViewModel.DesignUserProfileService())
    {
      this.IsSupported = true;
      this.Message = "Microsoft Band guides you through workouts; telling you what's next and tracking your performance in real time.";
      this.Type = TileFirstTimeUseViewModel.FirstTimeUseType.Run;
    }

    private sealed class DesignPromptService : ILauncherService
    {
      public Task<IList<EmailAddress>> PromptEmailAsync() => throw new NotImplementedException();

      public void ShowUserWebBrowser(Uri uri) => throw new NotImplementedException();

      public void ShowUserMotionSettings() => throw new NotImplementedException();

      public void CallPhoneNumber(string phoneNumber) => throw new NotImplementedException();

      public void MapAddress(params string[] addressLines) => throw new NotImplementedException();

      public void MapAddress(IEnumerable<string> addressLines) => throw new NotImplementedException();

      public void MapGeoposition(
        double latitude,
        double longitude,
        string poiTitle,
        int zoomLevel = 5)
      {
        throw new NotImplementedException();
      }
    }

    private sealed class DesignUserProfileService : IUserProfileService, INotifyPropertyChanged
    {
      public BandUserProfile CurrentUserProfile => throw new NotImplementedException();

      public bool IsBandRegistered => throw new NotImplementedException();

      public bool IsRegisteredBandPaired => throw new NotImplementedException();

      public DistanceUnitType DistanceUnitType => throw new NotImplementedException();

      public MassUnitType MassUnitType => throw new NotImplementedException();

      public TemperatureUnitType TemperatureUnitType => throw new NotImplementedException();

      public string ThirdPartyPartnersPortalEndpoint => throw new NotImplementedException();

      public DateTimeOffset? CreatedOn => throw new NotImplementedException();

      public bool HasAnyBandBeenRegistered => false;

      public int LastKnownAge => throw new NotImplementedException();

      public bool IsMemoryCachingEnabled { get; set; }

      public Task<BandUserProfile> GetCloudUserProfileAsync(
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task<BandUserProfile> GetUserProfileAsync(
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task ImportUserProfileAsync(
        BandUserProfile userProfile,
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task SaveCloudUserProfileAsync(
        BandUserProfile userProfile,
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task SaveCloudAndBandUserProfileAsync(
        BandUserProfile userProfile,
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task SaveUserProfileAsync(
        BandUserProfile userProfile,
        CancellationToken cancellationToken,
        bool useCloudOnly = false)
      {
        throw new NotImplementedException();
      }

      public Task SetUserProfileAsync(
        BandUserProfile userProfile,
        CancellationToken cancellationToken)
      {
        throw new NotImplementedException();
      }

      public Task LinkBandToProfileAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

      public Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

      public Task RefreshUserProfileAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

      public event PropertyChangedEventHandler PropertyChanged
      {
        add
        {
        }
        remove
        {
        }
      }
    }
  }
}
