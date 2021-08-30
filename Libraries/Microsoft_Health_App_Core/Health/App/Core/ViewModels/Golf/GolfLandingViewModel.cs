// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfLandingViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Find course"})]
  [PageMetadata(PageContainerType.HomeShell)]
  public class GolfLandingViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\GolfLandingViewModel.cs");
    private readonly ISmoothNavService smoothNavService;
    private readonly IConnectedAppsProvider connectedAppsProvider;
    private readonly IEnvironmentService environmentService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IBingHealthAndFitnessClient healthAndFitnessClient;
    private readonly IPermissionService permissionService;
    private bool isPartnerConnected;
    private bool refreshOnBack;
    private ICommand searchCommand;
    private ICommand connectCommand;
    private ICommand manageAppsCommand;
    private ICommand retryCommand;
    private ICommand playVideoCommand;
    private ICommand recentCommand;
    private ICommand nearbyCommand;
    private ICommand browseRegionCommand;
    private ICommand requestCourseCommand;

    public GolfLandingViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IEnvironmentService environmentService,
      IErrorHandlingService errorHandlingService,
      IBingHealthAndFitnessClient healthAndFitnessClient,
      IPermissionService permissionService,
      IConnectedAppsProvider connectedAppsProvider)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.connectedAppsProvider = connectedAppsProvider;
      this.environmentService = environmentService;
      this.errorHandlingService = errorHandlingService;
      this.permissionService = permissionService;
      this.healthAndFitnessClient = healthAndFitnessClient;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null) => this.IsPartnerConnected = (await this.connectedAppsProvider.GetConnectedAppsAsync(CancellationToken.None)).Contains<string>("TMag");

    protected override void OnBackNavigation()
    {
      if (!this.refreshOnBack)
        return;
      this.refreshOnBack = false;
      this.Refresh();
    }

    private async void OpenGolfIntroVideo()
    {
      this.LoadState = LoadState.Loading;
      PixelScreenSize pixelScreenSize = this.environmentService.PixelScreenSize;
      double height = (double) pixelScreenSize.Height;
      double width = (double) pixelScreenSize.Width;
      try
      {
        this.smoothNavService.Navigate(typeof (VideoViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Url",
            await this.healthAndFitnessClient.GetGolfIntroVideoUrlAsync(height, width, CancellationToken.None)
          }
        });
        ApplicationTelemetry.LogGolfWatchIntroVideo();
      }
      catch (Exception ex)
      {
        GolfLandingViewModel.Logger.Error((object) "Could not open Golf intro video.", ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.LoadState = LoadState.Loaded;
    }

    public bool IsPartnerConnected
    {
      get => this.isPartnerConnected;
      set => this.SetProperty<bool>(ref this.isPartnerConnected, value, nameof (IsPartnerConnected));
    }

    public ICommand SearchCommand => this.searchCommand ?? (this.searchCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfCourseSearchViewModel)))));

    public ICommand ConnectCommand => this.connectCommand ?? (this.connectCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.refreshOnBack = true;
      this.smoothNavService.Navigate(typeof (PartnerConnectViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "PartnerName",
          "tmag"
        }
      });
    })));

    public ICommand ManageAppsCommand => this.manageAppsCommand ?? (this.manageAppsCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.refreshOnBack = true;
      this.smoothNavService.Navigate(typeof (ConnectedAppsViewModel));
    })));

    public ICommand RetryCommand => this.retryCommand ?? (this.retryCommand = (ICommand) new HealthCommand((Action) (() => this.Refresh())));

    public ICommand PlayVideoCommand => this.playVideoCommand ?? (this.playVideoCommand = (ICommand) new HealthCommand((Action) (() => this.OpenGolfIntroVideo())));

    public ICommand RecentCommand => this.recentCommand ?? (this.recentCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfCourseRecentViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "RequiresFilterInitialization",
        bool.FalseString
      }
    }))));

    public ICommand NearbyCommand => this.nearbyCommand ?? (this.nearbyCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      try
      {
        await this.permissionService.RequestPermissionsAsync(FeaturePermissions.Golf);
        this.smoothNavService.Navigate(typeof (GolfCourseNearbyViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "RequiresFilterInitialization",
            bool.TrueString
          }
        });
      }
      catch (PermissionDeniedException ex)
      {
        await this.errorHandlingService.HandleExceptionAsync((Exception) ex);
      }
    })));

    public ICommand BrowseRegionCommand => this.browseRegionCommand ?? (this.browseRegionCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfCourseBrowseRegionViewModel)))));

    public ICommand RequestCourseCommand => this.requestCourseCommand ?? (this.requestCourseCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (GolfCourseRequestViewModel)))));
  }
}
