// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.UserProfileService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public sealed class UserProfileService : 
    HealthObservableObject,
    IUserProfileService,
    INotifyPropertyChanged
  {
    private const string CachedUserProfileKey = "CachedUserProfile";
    private const string IsCachedUserProfileProtectedKey = "UserProfileManager.IsCachedUserProfileProtected";
    private const string IsBandRegisteredKey = "UserProfileManager.IsDeviceRegistered";
    private const string IsRegisteredBandPairedKey = "UserProfileManager.IsRegisterBandPaired";
    private const string ThirdPartyPartnersPortalEndpointKey = "UserProfileManager.ThirdPartyPartnersPortalEndpoint";
    private const string CreatedOnKey = "UserProfileManager.CreatedOn";
    private const string HasAnyBandBeenRegisteredKey = "UserProfileManager.HasAnyBandBeenRegistered";
    private const string LastKnownAgeKey = "UserProfileManager.LastKnownAge";
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConfigProvider configProvider;
    private readonly IDispatchService dispatchService;
    private readonly IMessageSender messageSender;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly IRegionService regionServce;
    private readonly IPolicyEnforcingBandClientFactory singleDeviceService;
    private BandUserProfile cachedUserProfile;

    public UserProfileService(
      IBandConnectionFactory cargoConnectionFactory,
      IConfigProvider configProvider,
      IDispatchService dispatchService,
      IMessageSender messageSender,
      IDynamicConfigurationService dynamicConfigurationService,
      IRegionService regionServce,
      IPolicyEnforcingBandClientFactory singleDeviceService)
    {
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) dispatchService, nameof (dispatchService));
      Assert.ParamIsNotNull((object) messageSender, nameof (messageSender));
      Assert.ParamIsNotNull((object) messageSender, nameof (dynamicConfigurationService));
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.configProvider = configProvider;
      this.dispatchService = dispatchService;
      this.messageSender = messageSender;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.regionServce = regionServce;
      this.singleDeviceService = singleDeviceService;
    }

    public BandUserProfile CurrentUserProfile
    {
      get
      {
        if (this.IsMemoryCachingEnabled && this.cachedUserProfile != null)
          return this.cachedUserProfile;
        this.cachedUserProfile = this.configProvider.GetComplexValue<BandUserProfile>("CachedUserProfile", (BandUserProfile) null, this.configProvider.Get<bool>("UserProfileManager.IsCachedUserProfileProtected", false));
        return this.cachedUserProfile;
      }
      private set
      {
        this.configProvider.SetComplexValue<BandUserProfile>("CachedUserProfile", value, true);
        this.configProvider.Set<bool>("UserProfileManager.IsCachedUserProfileProtected", true);
        if (this.IsMemoryCachingEnabled)
          this.cachedUserProfile = value;
        this.RaisePropertyChanged(nameof (CurrentUserProfile));
        this.RaisePropertyChanged("DistanceUnitType");
        this.RaisePropertyChanged("MassUnitType");
        this.RaisePropertyChanged("TemperatureUnitType");
      }
    }

    public bool IsMemoryCachingEnabled { get; set; } = true;

    public bool IsBandRegistered
    {
      get => this.configProvider.Get<bool>("UserProfileManager.IsDeviceRegistered", true);
      private set
      {
        if (this.IsBandRegistered != value)
        {
          this.configProvider.Set<bool>("UserProfileManager.IsDeviceRegistered", value);
          this.RaisePropertyChanged(nameof (IsBandRegistered));
          this.messageSender.Send<BandRegistrationChangedMessage>(new BandRegistrationChangedMessage());
        }
        if (value)
          return;
        this.IsRegisteredBandPaired = false;
      }
    }

    public bool IsRegisteredBandPaired
    {
      get => this.configProvider.Get<bool>("UserProfileManager.IsRegisterBandPaired", true);
      private set
      {
        if (this.IsRegisteredBandPaired == value)
          return;
        this.configProvider.Set<bool>("UserProfileManager.IsRegisterBandPaired", value);
        this.RaisePropertyChanged(nameof (IsRegisteredBandPaired));
        this.messageSender.Send<BandRegistrationChangedMessage>(new BandRegistrationChangedMessage());
      }
    }

    public string ThirdPartyPartnersPortalEndpoint
    {
      get => this.configProvider.Get<string>("UserProfileManager.ThirdPartyPartnersPortalEndpoint", string.Empty);
      private set
      {
        if (!(this.ThirdPartyPartnersPortalEndpoint != value))
          return;
        this.configProvider.Set<string>("UserProfileManager.ThirdPartyPartnersPortalEndpoint", value);
        this.RaisePropertyChanged(nameof (ThirdPartyPartnersPortalEndpoint));
      }
    }

    public DateTimeOffset? CreatedOn
    {
      get
      {
        DateTimeOffset dateTimeOffset;
        return this.configProvider.TryGetValue<DateTimeOffset>("UserProfileManager.CreatedOn", out dateTimeOffset) && dateTimeOffset != DateTimeOffset.MinValue ? new DateTimeOffset?(dateTimeOffset) : new DateTimeOffset?();
      }
      private set
      {
        DateTimeOffset? createdOn = this.CreatedOn;
        DateTimeOffset? nullable = value;
        if ((createdOn.HasValue == nullable.HasValue ? (createdOn.HasValue ? (createdOn.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        IConfigProvider configProvider = this.configProvider;
        nullable = value;
        DateTimeOffset dateTimeOffset = nullable ?? DateTimeOffset.MinValue;
        configProvider.Set<DateTimeOffset>("UserProfileManager.CreatedOn", dateTimeOffset);
        this.RaisePropertyChanged(nameof (CreatedOn));
      }
    }

    public bool HasAnyBandBeenRegistered
    {
      get => this.configProvider.Get<bool>("UserProfileManager.HasAnyBandBeenRegistered", true);
      private set
      {
        if (this.HasAnyBandBeenRegistered == value)
          return;
        this.configProvider.Set<bool>("UserProfileManager.HasAnyBandBeenRegistered", value);
        this.RaisePropertyChanged(nameof (HasAnyBandBeenRegistered));
        this.messageSender.Send<AnyBandRegisteredMessage>(new AnyBandRegisteredMessage(value));
      }
    }

    public int LastKnownAge
    {
      get => this.configProvider.Get<int>("UserProfileManager.LastKnownAge", -1);
      private set
      {
        if (this.LastKnownAge == value)
          return;
        this.configProvider.Set<int>("UserProfileManager.LastKnownAge", value);
        this.RaisePropertyChanged(nameof (LastKnownAge));
      }
    }

    public Task<BandUserProfile> GetCloudUserProfileAsync(
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return Task.Run<BandUserProfile>((Func<Task<BandUserProfile>>) (async () =>
      {
        BandUserProfile userProfileAsync;
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
          userProfileAsync = await cargoConnection.GetCloudUserProfileAsync((Func<IUserProfile, Task<BandUserProfile>>) (async cargoClientUserProfile =>
          {
            await this.CacheUserProfilePropertiesAsync(cargoClientUserProfile).ConfigureAwait(false);
            return new BandUserProfile(cargoClientUserProfile, this.regionServce.CurrentRegion);
          }));
        return userProfileAsync;
      }), cancellationToken);
    }

    public Task<BandUserProfile> GetUserProfileAsync(
      CancellationToken cancellationToken)
    {
      return Task.Run<BandUserProfile>((Func<Task<BandUserProfile>>) (async () =>
      {
        BandUserProfile userProfileAsync;
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
          userProfileAsync = await cargoConnection.GetUserProfileAsync((Func<IUserProfile, Task<BandUserProfile>>) (async cargoClientUserProfile =>
          {
            await this.CacheUserProfilePropertiesAsync(cargoClientUserProfile).ConfigureAwait(false);
            return new BandUserProfile(cargoClientUserProfile, this.regionServce.CurrentRegion);
          }), cancellationToken);
        return userProfileAsync;
      }), cancellationToken);
    }

    public Task ImportUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Func<IUserProfile, Task> func;
      return Task.Run((Func<Task>) (async () =>
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
          await cargoConnection.ImportUserProfileAsync(func ?? (func = (Func<IUserProfile, Task>) (async cargoClientUserProfile =>
          {
            await this.CacheUserProfilePropertiesAsync(cargoClientUserProfile).ConfigureAwait(false);
            userProfile.ApplyToProfile(cargoClientUserProfile);
          })));
      }), cancellationToken);
    }

    public async Task SaveCloudUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.SaveCloudUserProfileAsync((Func<IUserProfile, Task>) (async cargoClientUserProfile =>
        {
          await this.CacheUserProfilePropertiesAsync(cargoClientUserProfile).ConfigureAwait(false);
          userProfile.ApplyToProfile(cargoClientUserProfile);
        }));
      await this.SetUserProfileAsync(userProfile, cancellationToken).ConfigureAwait(false);
      this.RemoveCacheItem("Weights");
      this.messageSender.Send<WeightChangedMessage>(new WeightChangedMessage((WeightSensor) null));
    }

    public async Task SaveCloudAndBandUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.SaveUserProfileAsync((Func<IUserProfile, Task>) (async cargoClientUserProfile =>
        {
          await this.CacheUserProfilePropertiesAsync(cargoClientUserProfile).ConfigureAwait(false);
          userProfile.ApplyToProfile(cargoClientUserProfile);
        }), cancellationToken).ConfigureAwait(false);
      await this.SetUserProfileAsync(userProfile, cancellationToken).ConfigureAwait(false);
      this.RemoveCacheItem("Weights");
      this.messageSender.Send<WeightChangedMessage>(new WeightChangedMessage((WeightSensor) null));
    }

    public Task SaveUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken,
      bool useCloudOnly = false)
    {
      return this.IsBandRegistered && !useCloudOnly ? this.SaveCloudAndBandUserProfileAsync(userProfile, cancellationToken) : this.SaveCloudUserProfileAsync(userProfile, cancellationToken);
    }

    public Task SetUserProfileAsync(
      BandUserProfile userProfile,
      CancellationToken cancellationToken)
    {
      return this.dispatchService.RunOnUIThreadAsync((Action) (() => this.CurrentUserProfile = userProfile));
    }

    public async Task LinkBandToProfileAsync(CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.LinkBandToProfileAsync(cancellationToken).ConfigureAwait(false);
      await this.SetIsBandRegisteredAsync(true).ConfigureAwait(false);
      await this.SetIsRegisteredBandPairedAsync(true).ConfigureAwait(false);
    }

    public async Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.UnlinkBandFromProfileAsync(cancellationToken).ConfigureAwait(false);
      await this.SetIsBandRegisteredAsync(false).ConfigureAwait(false);
      await this.SetIsRegisteredBandPairedAsync(false).ConfigureAwait(false);
      this.singleDeviceService.Invalidate();
    }

    public async Task RefreshUserProfileAsync(CancellationToken cancellationToken) => await this.SetUserProfileAsync(await this.GetCloudUserProfileAsync(cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);

    private CargoLocaleSettings UnitPreferences => this.CurrentUserProfile?.LocaleSettings;

    private bool UnitPreferencesAreAvailable => this.UnitPreferences != null;

    private DistanceUnitType NoAvailablePreferenceDistanceUnitType => LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults).DistanceLongUnits;

    private MassUnitType NoAvailablePreferenceMassUnitType => LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults).MassUnits;

    private TemperatureUnitType NoAvailablePreferenceTemperatureUnitType => LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults).TemperatureUnits;

    public DistanceUnitType DistanceUnitType => this.UnitPreferencesAreAvailable ? this.UnitPreferences.DistanceLongUnits : this.NoAvailablePreferenceDistanceUnitType;

    public MassUnitType MassUnitType => this.UnitPreferencesAreAvailable ? this.UnitPreferences.MassUnits : this.NoAvailablePreferenceMassUnitType;

    public TemperatureUnitType TemperatureUnitType => this.UnitPreferencesAreAvailable ? this.UnitPreferences.TemperatureUnits : this.NoAvailablePreferenceTemperatureUnitType;

    private Task SetIsBandRegisteredAsync(IUserProfile userProfile) => this.SetIsBandRegisteredAsync(userProfile.ApplicationSettings.PairedDeviceId != Guid.Empty);

    private Task SetIsBandRegisteredAsync(bool isBandRegistered) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.IsBandRegistered = isBandRegistered));

    private async Task SetIsRegisteredBandPairedAsync(IUserProfile userProfile)
    {
      bool isBandRegistered = this.IsBandRegistered;
      bool paired = isBandRegistered && this.IsRegisteredBandPaired;
      if (isBandRegistered && !paired)
      {
        IDevice instance = ServiceLocator.Current.GetInstance<IDevice>("Band");
        try
        {
          using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10.0)))
            paired = await (instance?.CanSyncAsync(cts.Token) ?? Task.FromResult<bool>(false));
        }
        catch
        {
        }
      }
      await this.SetIsRegisteredBandPairedAsync(paired);
    }

    private Task SetIsRegisteredBandPairedAsync(bool isRegisteredBandPaired) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.IsRegisteredBandPaired = isRegisteredBandPaired));

    private Task SetThirdPartyPartnersPortalEndpointAsync(IUserProfile userProfile) => userProfile != null && userProfile.ApplicationSettings != null && !string.IsNullOrEmpty(userProfile.ApplicationSettings.ThirdPartyPartnersPortalEndpoint) ? this.dispatchService.RunOnUIThreadAsync((Action) (() => this.ThirdPartyPartnersPortalEndpoint = userProfile.ApplicationSettings.ThirdPartyPartnersPortalEndpoint)) : (Task) Task.FromResult<bool>(true);

    private Task SetCreatedOnAsync(IUserProfile userProfile) => userProfile != null && userProfile.CreatedOn.HasValue ? this.dispatchService.RunOnUIThreadAsync((Action) (() => this.CreatedOn = userProfile.CreatedOn)) : (Task) Task.FromResult<bool>(true);

    private Task SetHasAnyBandBeenRegisteredAsync(IUserProfile userProfile) => this.SetHasAnyBandBeenRegisteredAsync(userProfile.AllDeviceSettings != null && userProfile.AllDeviceSettings.Any<KeyValuePair<Guid, DeviceSettings>>((Func<KeyValuePair<Guid, DeviceSettings>, bool>) (p => p.Key != Guid.Empty)));

    private Task SetHasAnyBandBeenRegisteredAsync(bool hasAnyBandBeenRegistered) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.HasAnyBandBeenRegistered = hasAnyBandBeenRegistered));

    private Task SetLastKnownAgeAsync(IUserProfile userProfile) => this.SetLastKnownAgeAsync(userProfile.Birthdate.GetAge());

    private Task SetLastKnownAgeAsync(int lastKnownAge) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.LastKnownAge = lastKnownAge));

    private async Task CacheUserProfilePropertiesAsync(IUserProfile userProfile)
    {
      bool wasRegistered = this.IsBandRegistered;
      await this.SetIsBandRegisteredAsync(userProfile).ConfigureAwait(false);
      await this.SetThirdPartyPartnersPortalEndpointAsync(userProfile).ConfigureAwait(false);
      await this.SetCreatedOnAsync(userProfile).ConfigureAwait(false);
      await this.SetHasAnyBandBeenRegisteredAsync(userProfile).ConfigureAwait(false);
      await this.SetLastKnownAgeAsync(userProfile).ConfigureAwait(false);
      if (wasRegistered || !this.IsBandRegistered)
        return;
      await this.SetIsRegisteredBandPairedAsync(userProfile).ConfigureAwait(false);
    }

    private void RemoveCacheItem(string tag) => ServiceLocator.Current.GetInstance<IHttpCacheService>()?.RemoveTagsAsync(tag);
  }
}
