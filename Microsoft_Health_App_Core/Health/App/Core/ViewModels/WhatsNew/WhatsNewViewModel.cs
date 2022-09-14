// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WhatsNew.WhatsNewViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.WhatsNew
{
  [PageMetadata(PageContainerType.HomeShell)]
  public class WhatsNewViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WhatsNew\\WhatsNewViewModel.cs");
    private readonly IWhatsNewService whatsNewService;
    private readonly ILauncherService launcherService;
    private readonly ISmoothNavService navService;
    private readonly IEnvironmentService environmentService;
    private readonly IBandThemeManager bandThemeManager;
    private readonly IUserProfileService userProfileService;
    private readonly ITileManagementService tileManagementService;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly ISocialEngagementService socialEngagementService;
    private readonly IPagePicker pagePicker;
    private IList<WhatsNewPivotItemViewModel> pivots;
    private int selectedPivot;
    private WhatsNewPivotColor selectedPivotColorTheme;

    public IList<WhatsNewPivotItemViewModel> Pivots
    {
      get => this.pivots;
      private set
      {
        this.SetProperty<IList<WhatsNewPivotItemViewModel>>(ref this.pivots, value, nameof (Pivots));
        this.RaisePropertyChanged("PageCount");
      }
    }

    public int SelectedPivotIndex
    {
      get => this.selectedPivot;
      set
      {
        int selectedPivot = this.selectedPivot;
        if (!this.SetProperty<int>(ref this.selectedPivot, value, nameof (SelectedPivotIndex)))
          return;
        this.UpdatePivotsState(selectedPivot, value);
      }
    }

    public int PageCount => this.Pivots.Count;

    public WhatsNewPivotColor SelectedPivotColorTheme
    {
      get => this.selectedPivotColorTheme;
      set => this.SetProperty<WhatsNewPivotColor>(ref this.selectedPivotColorTheme, value, nameof (SelectedPivotColorTheme));
    }

    public WhatsNewViewModel(
      INetworkService networkService,
      ISmoothNavService navService,
      IWhatsNewService whatsNewService,
      IEnvironmentService environmentService,
      ILauncherService launcherService,
      IUserProfileService userProfileService,
      IBandThemeManager bandThemeManager,
      ITileManagementService tileManagementService,
      IDynamicConfigurationService dynamicConfigurationService,
      ISocialEngagementService socialEngagementService,
      IPagePicker pagePicker)
      : base(networkService)
    {
      this.environmentService = environmentService;
      this.whatsNewService = whatsNewService;
      this.launcherService = launcherService;
      this.navService = navService;
      this.bandThemeManager = bandThemeManager;
      this.userProfileService = userProfileService;
      this.tileManagementService = tileManagementService;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.socialEngagementService = socialEngagementService;
      this.pagePicker = pagePicker;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      ApplicationTelemetry.LogWhatsNewView();
      if (!this.whatsNewService.HasBeenViewed)
      {
        ApplicationTelemetry.LogWhatsNewSessionCount(this.whatsNewService.GetApplicationSessions().ToString());
        this.whatsNewService.HasBeenViewed = true;
      }
      this.Pivots = this.CreatePivotItems();
      this.UpdatePivotsState(0, 0);
      return (Task) Task.FromResult<bool>(true);
    }

    private IList<WhatsNewPivotItemViewModel> CreatePivotItems()
    {
      BandClass bandClass = this.bandThemeManager.CurrentTheme.BandClass;
      HealthCommand healthCommand1 = new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(new Uri("https://go.microsoft.com/fwlink/?LinkID=532705"))));
      HealthCommand healthCommand2 = new HealthCommand((Action) (() => this.navService.Navigate(typeof (ManageTilesViewModel))));
      HealthCommand healthCommand3 = new HealthCommand((Action) (() => this.navService.Navigate(typeof (TilesViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "Tile",
          "HikeTileViewModel"
        }
      })));
      string navigationButtonText = AppResources.WhatsNewPivotButtonLabel1;
      bool flag1 = this.tileManagementService.IsTileEnabled("e9e376af-fa3d-486d-8351-959cc20f4d8f");
      int num = this.userProfileService.IsBandRegistered ? 1 : 0;
      bool flag2 = bandClass.Equals((object) BandClass.Envoy);
      if (num == 0 || !flag2)
      {
        healthCommand3 = healthCommand1;
        navigationButtonText = AppResources.WhatsNewPivotButtonLabel2bAnd3B;
      }
      else if (flag2 && !flag1)
      {
        healthCommand3 = healthCommand2;
        navigationButtonText = AppResources.WhatsNewPivotButtonLabel1b;
      }
      List<WhatsNewPivotItemViewModel> pivotItemViewModelList = new List<WhatsNewPivotItemViewModel>();
      if (this.environmentService.OperatingSystemType.Equals((object) OperatingSystemType.Android))
        pivotItemViewModelList.Add(new WhatsNewPivotItemViewModel("Hike", AppResources.WhatsNewPivotHeader1, AppResources.WhatsNewPivotSubheader1, EmbeddedAsset.Hike, WhatsNewPivotColor.Primary));
      else
        pivotItemViewModelList.Add(new WhatsNewPivotItemViewModel("Hike", AppResources.WhatsNewPivotHeader1, AppResources.WhatsNewPivotSubheader1, EmbeddedAsset.Hike, WhatsNewPivotColor.Primary, navigationButtonText, (ICommand) healthCommand3));
      if (this.environmentService.OperatingSystemType.Equals((object) OperatingSystemType.Windows) && !this.environmentService.IsUwpAppOnDesktop())
      {
        WhatsNewPivotItemViewModel pivotItemViewModel = new WhatsNewPivotItemViewModel("UWP", AppResources.WhatsNewPivotHeader3, AppResources.WhatsNewPivotSubheader3, EmbeddedAsset.UWP, WhatsNewPivotColor.Primary);
        pivotItemViewModelList.Add(pivotItemViewModel);
      }
      if (this.dynamicConfigurationService.Configuration.Features.SummaryEmail.IsEnabled)
      {
        WhatsNewPivotItemViewModel pivotItemViewModel = new WhatsNewPivotItemViewModel("View of you", AppResources.WhatsNewPivotHeader2, AppResources.WhatsNewPivotSubheader2, EmbeddedAsset.Email, WhatsNewPivotColor.Primary, AppResources.WhatsNewPivotButtonLabel2, (ICommand) new HealthCommand((Action) (() => this.navService.Navigate(typeof (SettingsProfileViewModel)))));
        pivotItemViewModelList.Add(pivotItemViewModel);
      }
      WhatsNewPivotItemViewModel pivotItemViewModel1 = new WhatsNewPivotItemViewModel("Social", AppResources.WhatsNewPivotHeader4, AppResources.WhatsNewPivotSubheader4, EmbeddedAsset.Social, WhatsNewPivotColor.Primary, AppResources.WhatsNewPivotButtonLabel4, (ICommand) new HealthCommand((Action) (() =>
      {
        if (this.socialEngagementService.IsSocialEnabled)
          this.navService.Navigate(typeof (TilesViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Tile",
              "SocialTileViewModel"
            }
          });
        else
          this.navService.Navigate(this.pagePicker.Preferences);
      })));
      pivotItemViewModelList.Add(pivotItemViewModel1);
      return (IList<WhatsNewPivotItemViewModel>) pivotItemViewModelList;
    }

    private void UpdatePivotsState(int oldIndex, int newIndex)
    {
      if (oldIndex >= 0 && oldIndex < this.Pivots.Count)
        this.Pivots[oldIndex].IsActive = false;
      if (newIndex < 0 || newIndex >= this.Pivots.Count)
        return;
      WhatsNewPivotItemViewModel pivot = this.Pivots[newIndex];
      pivot.IsActive = true;
      this.SelectedPivotColorTheme = pivot.ColorTheme;
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      int selectedPivotIndex = this.SelectedPivotIndex;
      try
      {
        this.SelectedPivotIndex = -1;
        this.SelectedPivotIndex = selectedPivotIndex;
      }
      catch (Exception ex)
      {
        WhatsNewViewModel.Logger.Warn((object) string.Format("Error resetting the What's New FlipView to index [{0}].", new object[1]
        {
          (object) selectedPivotIndex
        }), ex);
      }
    }
  }
}
