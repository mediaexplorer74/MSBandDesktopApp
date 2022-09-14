// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.FreshInstallService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class FreshInstallService : IFreshInstallService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\FreshInstallService.cs");
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IConfig config;
    private readonly ISmoothNavService smoothNavService;
    private readonly IApplicationService applicationService;
    private readonly INetworkService networkService;
    private readonly IUserProfileService userProfileService;
    private readonly ITileManagementService tileManagementService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly IOobeService oobeService;
    private readonly IBandThemeManager bandThemeManager;
    private readonly IBackgroundTaskService backgroundTaskService;

    public FreshInstallService(
      IConnectionInfoProvider connectionInfoProvider,
      IConfig config,
      ISmoothNavService smoothNavService,
      IApplicationService applicationService,
      INetworkService networkService,
      IUserProfileService userProfileService,
      ITileManagementService tileManagementService,
      IBandConnectionFactory cargoConnectionFactory,
      IDynamicConfigurationService dynamicConfigurationService,
      IOobeService oobeService,
      IBandThemeManager bandThemeManager,
      IBackgroundTaskService backgroundTaskService)
    {
      this.connectionInfoProvider = connectionInfoProvider;
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.applicationService = applicationService;
      this.networkService = networkService;
      this.userProfileService = userProfileService;
      this.tileManagementService = tileManagementService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.oobeService = oobeService;
      this.bandThemeManager = bandThemeManager;
      this.backgroundTaskService = backgroundTaskService;
    }

    public async Task SetupAsync(
      IProgress<InitializationProgress> progressListener,
      CancellationToken token)
    {
      progressListener?.Report(InitializationProgress.NotStarted);
      this.smoothNavService.DisableNavPanel(typeof (IFreshInstallService));
      this.networkService.EnsureInternetAvailable();
      progressListener?.Report(InitializationProgress.LoggingIn);
      HealthCloudConnectionInfo connectionInfoAsync = await this.connectionInfoProvider.GetConnectionInfoAsync(token, true);
      FreshInstallService.Logger.Debug((object) "<START> handling fresh install startup");
      BandUserProfile cargoUserProfile = await this.FindUserProfileAsync(token);
      progressListener?.Report(InitializationProgress.GettingReady);
      await this.dynamicConfigurationService.InitializeAsync(token);
      if (cargoUserProfile == null || !cargoUserProfile.IsOobeCompleted)
      {
        FreshInstallService.Logger.Debug((object) "<FLAG> sending user to into oobe");
        await this.oobeService.StartAsync(token);
      }
      else
      {
        Task backgroundTaskRegistrationTask = this.backgroundTaskService.RegisterBackgroundTasksAsync(token);
        if (this.userProfileService.IsBandRegistered)
        {
          progressListener?.Report(InitializationProgress.AlmostThere);
          try
          {
            FreshInstallService.Logger.Debug((object) "<START> discovering enabled tiles on fresh install");
            await this.DiscoverEnabledTilesAsync(token);
            FreshInstallService.Logger.Debug((object) "<END> discovering enabled tiles on fresh install");
          }
          catch (Exception ex)
          {
            FreshInstallService.Logger.Error((object) "<FAILED> discovering enabled tiles on fresh install - defaulting to no tiles enabled", ex);
            this.SetNoTilesEnabled();
            this.config.EnabledTilesMatchesBand = false;
          }
          try
          {
            FreshInstallService.Logger.Debug((object) "<START> discovering band theme on fresh install");
            AppBandTheme themeFromBandAsync = await this.bandThemeManager.GetCurrentThemeFromBandAsync(token);
            FreshInstallService.Logger.Debug((object) "<END> discovering band theme on fresh install");
          }
          catch (Exception ex)
          {
            FreshInstallService.Logger.Error((object) "<FAILED> discovering band theme on fresh install - defaulting to default theme", ex);
          }
        }
        else
          this.SetNoTilesEnabled();
        await backgroundTaskRegistrationTask;
        try
        {
          FreshInstallService.Logger.Debug((object) "<START> deleting persisted webtile credentials on fresh install");
          await ServiceLocator.Current.GetInstance<IWebTileService>().DeleteAllStoredResourceCredentialsAsync();
          FreshInstallService.Logger.Debug((object) "<END> deleting persisted webtile credentials on fresh install");
        }
        catch (Exception ex)
        {
          FreshInstallService.Logger.Error((object) "<FAILED> deleting persisted webtile credentials on fresh install", ex);
        }
        this.config.OobeStatus = OobeStatus.Shown;
        this.smoothNavService.GoHome();
        this.smoothNavService.ClearBackStack();
        this.applicationService.StartCoreDelayedLoadingTask(token);
        backgroundTaskRegistrationTask = (Task) null;
      }
      this.smoothNavService.EnableNavPanel(typeof (IFreshInstallService));
      progressListener?.Report(InitializationProgress.Complete);
      FreshInstallService.Logger.Debug((object) "<END> handling fresh install startup");
    }

    private async Task<BandUserProfile> FindUserProfileAsync(
      CancellationToken cancellationToken)
    {
      BandUserProfile cargoUserProfile;
      if (this.userProfileService.CurrentUserProfile != null)
      {
        cargoUserProfile = this.userProfileService.CurrentUserProfile;
      }
      else
      {
        cargoUserProfile = await this.userProfileService.GetCloudUserProfileAsync(cancellationToken);
        await this.userProfileService.SetUserProfileAsync(cargoUserProfile, cancellationToken);
      }
      return cargoUserProfile;
    }

    private async Task DiscoverEnabledTilesAsync(CancellationToken token)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
        this.tileManagementService.EnabledTiles = (IList<AppBandTile>) new List<AppBandTile>((IEnumerable<AppBandTile>) (await cargoConnection.GetStartStripWithoutImagesAsync(token)).Select<AdminBandTile, AppBandTile>((Func<AdminBandTile, AppBandTile>) (cargoTile => this.tileManagementService.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == cargoTile.TileId)))).ToList<AppBandTile>());
      this.BeginEnsureTileUpdatesEnabled();
    }

    private async void BeginEnsureTileUpdatesEnabled() => await Task.Run((Func<Task>) (async () => await this.tileManagementService.EnsureTileUpdatesEnabledAsync().ConfigureAwait(false)));

    private void SetNoTilesEnabled() => this.tileManagementService.EnabledTiles = (IList<AppBandTile>) new List<AppBandTile>();
  }
}
