// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ManageTilesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.MvvmCross.ViewModels;
using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.EarlyUpdate;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.TileSettings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles"})]
  [PageMetadata(PageContainerType.HomeShell)]
  public class ManageTilesViewModel : PageViewModelBase, IHomeShellViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\ManageTilesViewModel.cs");
    private readonly IGeolocationService geolocationService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IConfig config;
    private readonly IDispatchService dispatchService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly ITileManagementService tileManagementService;
    private readonly IWebTileService webTileService;
    private readonly ITileUpdateService syncManager;
    private readonly IPhoneOsUpdateService phoneOsUpdateService;
    private readonly ITileNotificationService tileNotificationService;
    private readonly ISupportedTileService supportedTileService;
    private readonly IBandHardwareService bandHardwareService;
    private readonly ILauncherService launcherService;
    private readonly IPagePicker pagePicker;
    private readonly IBandThemeManager bandThemeManager;
    private uint maxTiles;
    private HealthCommand confirmCommand;
    private StartStrip originalTiles;
    private HealthCommand launchWebTileGalleryCommand;
    private HealthCommand<AppBandTile> showPreferencesCommand;
    private HealthCommand cancelCommand;
    private HealthCommand goToRearrangeTilesCommand;
    private HealthCommand<AppBandTile> toggleTileCommand;
    private HealthCommand<AppBandTile> toggleThirdPartyTileCommand;
    private HealthCommand earlyUpdateCommand;
    private bool hasThirdPartyTiles;
    private int tilesLeft;
    private bool suppressTileChanges;
    private bool isMaxTiles;
    private ManageTilesViewModel.ManageTilesPageState pageState;
    private IList<AppBandTile> baseTiles;
    private IList<AppBandTile> thirdPartyTiles;
    private EarlyUpdateStatus earlyUpdateStatus;
    private BandClass band;
    private bool showConfirmCancel;

    public ManageTilesViewModel(
      IBandConnectionFactory cargoConnectionFactory,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IMessageBoxService messageBoxService,
      IConfig config,
      ITileManagementService tileManagementService,
      IWebTileService webTileService,
      ITileUpdateService syncManager,
      IDispatchService dispatchService,
      INetworkService networkService,
      IGeolocationService geolocationService,
      IMessageSender messageSender,
      IPhoneOsUpdateService phoneOsUpdateService,
      ITileNotificationService tileNotificationService,
      ISupportedTileService supportedTileService,
      IBandHardwareService bandHardwareService,
      IPagePicker pagePicker,
      IBandThemeManager bandThemeManager,
      ILauncherService launcherService)
      : base(networkService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.messageBoxService = messageBoxService;
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.config = config;
      this.dispatchService = dispatchService;
      this.geolocationService = geolocationService;
      this.messageSender = messageSender;
      this.phoneOsUpdateService = phoneOsUpdateService;
      this.syncManager = syncManager;
      this.tileManagementService = tileManagementService;
      this.webTileService = webTileService;
      this.tileNotificationService = tileNotificationService;
      this.supportedTileService = supportedTileService;
      this.bandHardwareService = bandHardwareService;
      this.pagePicker = pagePicker;
      this.bandThemeManager = bandThemeManager;
      this.launcherService = launcherService;
      this.BaseTiles = (IList<AppBandTile>) new ObservableCollection<AppBandTile>();
      this.ThirdPartyTiles = (IList<AppBandTile>) new ObservableCollection<AppBandTile>();
      this.pageState = ManageTilesViewModel.ManageTilesPageState.Loading;
    }

    private async void OnBackKeyPressed(BackButtonPressedMessage message)
    {
      switch (this.PageState)
      {
        case ManageTilesViewModel.ManageTilesPageState.Loading:
          break;
        case ManageTilesViewModel.ManageTilesPageState.Loaded:
          if (!this.ShowConfirmCancel)
            break;
          ManageTilesViewModel.Logger.Info((object) "Back key pressed, showing confirmation UI.");
          message.CancelAction();
          await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () =>
          {
            if (await this.messageBoxService.ShowAsync(AppResources.ManageTilesConfirmationBody, AppResources.ManageTilesConfirmationTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
            {
              this.RevertChanges();
              this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
              {
                Enabled = true
              });
              this.smoothNavService.GoBack();
            }
            else
              this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
              {
                Enabled = false
              });
          }), false);
          break;
        case ManageTilesViewModel.ManageTilesPageState.Saving:
          message.CancelAction();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void OnPanelRefresh(PanelRefreshMessage message) => this.RevertChanges();

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.EnsureMessageRegistration();
      this.RefreshEarlyUpdateStatus();
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      ManageTilesViewModel.Logger.Debug((object) "<START> loading manage tiles page");
      try
      {
        this.PageState = ManageTilesViewModel.ManageTilesPageState.Loading;
        AppBandTheme themeFromBandAsync = await this.bandThemeManager.GetCurrentThemeFromBandAsync(CancellationToken.None);
        List<AppBandTile> tilesToUpdate = (List<AppBandTile>) null;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
          {
            ManageTilesViewModel manageTilesViewModel = this;
            int maxTiles = (int) manageTilesViewModel.maxTiles;
            int maxTileCountAsync = (int) await cargoConnection.GetMaxTileCountAsync();
            manageTilesViewModel.maxTiles = (uint) maxTileCountAsync;
            manageTilesViewModel = (ManageTilesViewModel) null;
            manageTilesViewModel = this;
            StartStrip originalTiles = manageTilesViewModel.originalTiles;
            StartStrip startStripAsync = await cargoConnection.GetStartStripAsync(cancellationTokenSource.Token);
            manageTilesViewModel.originalTiles = startStripAsync;
            manageTilesViewModel = (ManageTilesViewModel) null;
            await this.bandHardwareService.ClearDeviceTypeAsync(cancellationTokenSource.Token);
            manageTilesViewModel = this;
            int band = (int) manageTilesViewModel.band;
            int deviceTypeAsync = (int) await this.bandHardwareService.GetDeviceTypeAsync(cancellationTokenSource.Token);
            manageTilesViewModel.band = (BandClass) deviceTypeAsync;
            manageTilesViewModel = (ManageTilesViewModel) null;
          }
        }
        if (this.originalTiles != null)
        {
          this.PopulateTiles();
          tilesToUpdate = new List<AppBandTile>((IEnumerable<AppBandTile>) this.BaseTiles);
          this.TilesLeft = (int) this.maxTiles - this.tileManagementService.PendingTiles.Count<AppBandTile>();
          if (this.TilesLeft == 0)
            this.DisableTileModification();
          else
            this.EnableTileModification();
          this.tileManagementService.PendingNotifications = (IList<string>) null;
        }
        this.tileManagementService.RevertPendingSettings();
        if (tilesToUpdate != null)
          this.StartConfiguringTileUpdates((IEnumerable<AppBandTile>) tilesToUpdate);
        AppBandTile appBandTile = this.BaseTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId.ToString() == "a708f02a-03cd-4da0-bb33-be904e6a2924"));
        if (appBandTile != (AppBandTile) null)
          appBandTile.HasSettings = this.band == BandClass.Envoy;
        this.RemoveUnsupportedBandTiles();
        int num = await this.tileNotificationService.VerifyNotificationRequirementsAsync((IEnumerable<AppBandTile>) this.tileManagementService.EnabledTiles) ? 1 : 0;
        this.PageState = ManageTilesViewModel.ManageTilesPageState.Loaded;
        ManageTilesViewModel.Logger.Debug((object) "<END> loading manage tiles page");
        tilesToUpdate = (List<AppBandTile>) null;
      }
      catch (Exception ex)
      {
        ManageTilesViewModel.Logger.Error(ex, "<FAILED> loading manage tiles page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.smoothNavService.GoBack();
      }
    }

    private void RemoveUnsupportedBandTiles()
    {
      if (this.band != BandClass.Cargo || !this.BaseTiles.Contains(this.tileManagementService.KnownTiles.HikeTile))
        return;
      this.BaseTiles.Remove(this.tileManagementService.KnownTiles.HikeTile);
    }

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      if (this.tileManagementService.HaveSettingsChanged || this.tileManagementService.HaveTilesChanged || this.tileManagementService.PendingNotifications.Any<string>())
      {
        if (this.ShowConfirmCancel)
          this.ShowAppBar();
        else
          this.ShowConfirmCancel = true;
        this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
        {
          Enabled = false
        });
      }
      else
        this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
        {
          Enabled = true
        });
      this.EnsureMessageRegistration();
    }

    private async void StartConfiguringTileUpdates(IEnumerable<AppBandTile> tilesToUpdate)
    {
      try
      {
        await Task.Delay(TimeSpan.FromSeconds(1.0));
        await Task.Run((Func<Task>) (async () => await this.tileManagementService.ConfigureTileUpdatesAsync(tilesToUpdate).ConfigureAwait(false)));
      }
      catch (Exception ex)
      {
        ManageTilesViewModel.Logger.Error(ex, "Failed to configure tile updates.");
      }
    }

    private void RefreshEarlyUpdateStatus() => this.EarlyUpdateStatus = this.phoneOsUpdateService.UpdateStatus;

    private void PopulateTiles()
    {
      this.UnsubscribeFromTileChanges();
      this.suppressTileChanges = true;
      this.ThirdPartyTiles.Clear();
      this.ClearTileCollections();
      this.tileManagementService.EnabledTiles.Clear();
      foreach (AdminBandTile originalTile in this.originalTiles)
      {
        AdminBandTile tileFromBand = originalTile;
        AppBandTile tile = this.tileManagementService.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == tileFromBand.TileId));
        if (tile != (AppBandTile) null)
        {
          if (this.supportedTileService.IsSupportedByPlatform(tile))
          {
            AppBandTile appBandTile = tile.Copy();
            appBandTile.ShowTile = true;
            appBandTile.Settings = tileFromBand.SettingsMask;
            this.tileManagementService.PendingTiles.Add(appBandTile);
          }
        }
        else if (this.tileManagementService.KnownTiles.DisabledTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == tileFromBand.TileId)) == (AppBandTile) null)
        {
          bool flag1 = false;
          bool flag2;
          if (tileFromBand.IsWebTile)
          {
            IWebTile webTile = this.webTileService.GetWebTileManager.GetWebTile(tileFromBand.TileId);
            flag2 = webTile.HasNotifications;
            flag1 = webTile.NotificationEnabled;
          }
          else
            flag2 = false;
          AppBandTile appBandTile = new AppBandTile()
          {
            TileId = tileFromBand.TileId,
            Title = tileFromBand.Name,
            ThirdPartyIcon = tileFromBand.Image,
            ThirdPartyOwnerId = tileFromBand.OwnerId,
            ShowTile = true,
            Settings = tileFromBand.SettingsMask,
            HasSettings = flag2,
            IsThirdParty = true,
            IsWebTile = tileFromBand.IsWebTile,
            IsWebTileNotificationEnabled = flag1
          };
          if (appBandTile.HasSettings)
          {
            appBandTile.DefaultOffSettings = tileFromBand.SettingsMask - (ushort) 1;
            appBandTile.DefaultOnSettings = tileFromBand.SettingsMask;
          }
          this.tileManagementService.PendingTiles.Add(appBandTile);
        }
      }
      this.tileManagementService.EnabledTiles = (IList<AppBandTile>) new List<AppBandTile>((IEnumerable<AppBandTile>) this.tileManagementService.PendingTiles);
      foreach (AppBandTile tileFromApp in this.tileManagementService.KnownTiles.Where<AppBandTile>(new Func<AppBandTile, bool>(this.supportedTileService.IsSupportedByPlatform)))
        this.BaseTiles.Add(this.CopyPendingTiles(tileFromApp));
      foreach (AppBandTile tileFromApp in (IEnumerable<AppBandTile>) this.tileManagementService.PendingTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (s => s.IsThirdParty)).OrderBy<AppBandTile, string>((Func<AppBandTile, string>) (p => p.Title)))
        this.ThirdPartyTiles.Add(this.CopyPendingTiles(tileFromApp));
      this.VerifyHomeTiles();
      this.RefreshThirdPartyStatus();
      this.SubscribeToTileChanges();
      this.suppressTileChanges = false;
    }

    public bool ShowConfirmCancel
    {
      get => this.showConfirmCancel;
      set
      {
        if (!this.SetProperty<bool>(ref this.showConfirmCancel, value, nameof (ShowConfirmCancel)))
          return;
        if (this.showConfirmCancel)
          this.ShowAppBar();
        else
          this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      }
    }

    private ManageTilesViewModel.ManageTilesPageState PageState
    {
      get => this.pageState;
      set
      {
        this.pageState = value;
        this.RaisePropertyChanged("HeaderType");
        this.RaisePropertyChanged("ShowProgress");
        this.RaisePropertyChanged("LoadingText");
      }
    }

    public HeaderType HeaderType => this.PageState == ManageTilesViewModel.ManageTilesPageState.Loaded ? HeaderType.Normal : HeaderType.None;

    public bool ShowProgress => this.PageState != ManageTilesViewModel.ManageTilesPageState.Loaded;

    public string LoadingText
    {
      get
      {
        switch (this.pageState)
        {
          case ManageTilesViewModel.ManageTilesPageState.Loading:
            return AppResources.BandLoadingText;
          case ManageTilesViewModel.ManageTilesPageState.Saving:
            return AppResources.BandUpdateLoadingText;
          default:
            return string.Empty;
        }
      }
    }

    public IList<AppBandTile> BaseTiles
    {
      get => this.baseTiles;
      private set => this.SetProperty<IList<AppBandTile>>(ref this.baseTiles, value, nameof (BaseTiles));
    }

    public IList<AppBandTile> ThirdPartyTiles
    {
      get => this.thirdPartyTiles;
      private set => this.SetProperty<IList<AppBandTile>>(ref this.thirdPartyTiles, value, nameof (ThirdPartyTiles));
    }

    public bool HasThirdPartyTiles
    {
      get => this.hasThirdPartyTiles;
      private set
      {
        this.SetProperty<bool>(ref this.hasThirdPartyTiles, value, nameof (HasThirdPartyTiles));
        this.RaisePropertyChanged("WebTileGalleryText");
      }
    }

    public int TilesLeft
    {
      get => this.tilesLeft;
      set
      {
        if (value == 0)
          this.DisableTileModification();
        else if (this.isMaxTiles)
          this.EnableTileModification();
        this.SetProperty<int>(ref this.tilesLeft, value, nameof (TilesLeft));
        this.RaisePropertyChanged("TilesLeftLabel");
      }
    }

    public EarlyUpdateStatus EarlyUpdateStatus
    {
      get => this.earlyUpdateStatus;
      set => this.SetProperty<EarlyUpdateStatus>(ref this.earlyUpdateStatus, value, nameof (EarlyUpdateStatus));
    }

    public string TilesLeftLabel => string.Format(AppResources.ManageTilesAppSpaceLabel, new object[1]
    {
      (object) this.TilesLeft
    });

    public DateTimeOffset CurrentTime => DateTimeOffset.Now.ToLocalTime();

    public string WebTileGalleryText => this.HasThirdPartyTiles ? AppResources.Tiles_Gallery_Text : AppResources.Tiles_Gallery_Text_No_Developer_Tiles;

    public string WebTileGalleryUrl => HealthAppConstants.GetWebTileGalleryUrl();

    public ICommand LaunchWebTileGalleryCommand => (ICommand) this.launchWebTileGalleryCommand ?? (ICommand) (this.launchWebTileGalleryCommand = new HealthCommand((Action) (() => this.launcherService.ShowUserWebBrowser(new Uri(this.WebTileGalleryUrl)))));

    public ICommand ToggleTileCommand => (ICommand) this.toggleTileCommand ?? (ICommand) (this.toggleTileCommand = new HealthCommand<AppBandTile>((Action<AppBandTile>) (async tile =>
    {
      bool wasToggled = false;
      if (tile.ShowTile)
      {
        if (await this.VerifyAppRequirementsAsync(tile))
        {
          if (!this.tileManagementService.PendingNotifications.Contains(tile.TileId.ToString()))
            this.tileManagementService.PendingNotifications.Add(tile.TileId.ToString());
          tile.Settings = tile.DefaultOnSettings;
          this.tileManagementService.PendingTiles.Add(tile.Copy());
          --this.TilesLeft;
          wasToggled = true;
        }
      }
      else
      {
        this.tileManagementService.PendingTiles.Remove(tile);
        ++this.TilesLeft;
        wasToggled = true;
      }
      if (wasToggled)
      {
        this.ShowConfirmCancel = true;
        this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
        {
          Enabled = false
        });
      }
      else
      {
        this.suppressTileChanges = true;
        tile.ShowTile = !tile.ShowTile;
        this.suppressTileChanges = false;
      }
      this.tileManagementService.HaveTilesChanged = true;
    })));

    public ICommand ToggleThirdPartyTileCommand => (ICommand) this.toggleThirdPartyTileCommand ?? (ICommand) (this.toggleThirdPartyTileCommand = new HealthCommand<AppBandTile>((Action<AppBandTile>) (tile =>
    {
      if (tile.ShowTile)
      {
        this.tileManagementService.PendingTiles.Add(tile.Copy());
        --this.TilesLeft;
      }
      else
      {
        this.tileManagementService.PendingTiles.Remove(tile);
        ++this.TilesLeft;
      }
      this.ShowConfirmCancel = true;
      this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
      {
        Enabled = false
      });
      this.tileManagementService.HaveTilesChanged = true;
    })));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.RevertChanges)));

    public ICommand ShowPreferencesCommand => (ICommand) this.showPreferencesCommand ?? (ICommand) (this.showPreferencesCommand = new HealthCommand<AppBandTile>((Action<AppBandTile>) (tile =>
    {
      if (!tile.HasSettings || !this.tileManagementService.PendingTiles.Contains(tile) || !tile.ShowTile || !tile.IsCurrentlySupported)
        return;
      string str = tile.TileId.ToString();
      switch (str)
      {
        case "22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1":
        case "b4edbc35-027b-4d10-a797-1099cd2ad98a":
          this.smoothNavService.Navigate(this.pagePicker.QuickResponseSettings, (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "TileId",
              str
            }
          });
          break;
        case "2e76a806-f509-4110-9c03-43dd2359d2ad":
        case "76b08699-2f2e-9041-96c2-1f4bfc7eab10":
        case "ec149021-ce45-40e9-aeee-08f86e4746a7":
        case "fd06b486-bbda-4da5-9014-124936386237":
          this.smoothNavService.Navigate(typeof (BasicSettingsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "TileId",
              str
            }
          });
          break;
        case "4076b009-0455-4af7-a705-6d4acd45a556":
          this.smoothNavService.Navigate(typeof (NotificationCenterSettingsViewModel));
          break;
        case "5992928a-bd79-4bb5-9678-f08246d03e68":
          this.smoothNavService.Navigate(typeof (FinanceSettingsViewModel));
          break;
        case "64a29f65-70bb-4f32-99a2-0f250a05d427":
          this.smoothNavService.Navigate(typeof (StarbucksSettingsViewModel));
          break;
        case "65bd93db-4293-46af-9a28-bdd6513b4677":
          this.smoothNavService.Navigate(typeof (RunSettingsViewModel));
          break;
        case "823ba55a-7c98-4261-ad5e-929031289c6e":
          this.smoothNavService.Navigate(typeof (MailSettingsViewModel));
          break;
        case "96430fcb-0060-41cb-9de2-e00cac97f85d":
          this.smoothNavService.Navigate(typeof (BikeSettingsViewModel));
          break;
        case "a708f02a-03cd-4da0-bb33-be904e6a2924":
          this.smoothNavService.Navigate(typeof (ExerciseSettingsViewModel));
          break;
        case "d7fb5ff5-906a-4f2c-8269-dde6a75138c4":
          this.smoothNavService.Navigate(typeof (CortanaSettingsViewModel));
          break;
        default:
          this.smoothNavService.Navigate(typeof (BasicSettingsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "TileId",
              str
            },
            {
              "TileTitle",
              tile.Title
            }
          });
          break;
      }
    })));

    public ICommand GoToRearrangeTilesCommand => (ICommand) this.goToRearrangeTilesCommand ?? (ICommand) (this.goToRearrangeTilesCommand = new HealthCommand(new Action(this.GoToRearrangeTiles)));

    private void GoToRearrangeTiles() => this.smoothNavService.Navigate(typeof (RearrangeTilesViewModel));

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.SaveChanges)));

    private async void SaveChanges()
    {
      if (await this.CancelDisablingThirdPartyTilesAsync())
        return;
      if (this.ShowConfirmCancel)
        this.ShowConfirmCancel = false;
      this.PageState = ManageTilesViewModel.ManageTilesPageState.Saving;
      using (ITimedTelemetryEvent timedEvent = ApplicationTelemetry.TimeSaveTileSettings())
      {
        try
        {
          if ((long) this.tileManagementService.PendingTiles.Count<AppBandTile>() <= (long) this.maxTiles)
          {
            await this.SaveManageTilesSettingsAsync();
          }
          else
          {
            int num = (int) await this.messageBoxService.ShowAsync(AppResources.ManageTilesMaxBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
          }
          this.MarkDisabledThirdPartyTilesAsDirty();
        }
        catch (Exception ex)
        {
          ManageTilesViewModel.Logger.Error(ex, "<FAILED> saving manage tiles");
          if (ex.Find<AppoinmentStoreException>() != null)
          {
            int num = (int) await this.messageBoxService.ShowAsync(AppResources.CalendarAccessErrorBodyText, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
            this.ShowConfirmCancel = false;
          }
          else
          {
            if (ex is InvalidTileException)
            {
              int num = (int) await this.messageBoxService.ShowAsync(AppResources.GenericErrorMessage, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
            }
            else
              await this.errorHandlingService.HandleExceptionAsync(ex);
            if (this.NavigationState == PageNavigationState.Current)
              this.ShowConfirmCancel = true;
          }
          timedEvent.Cancel();
        }
      }
      this.PageState = ManageTilesViewModel.ManageTilesPageState.Loaded;
      this.messageSender.Send<InvalidateAllViewModelsOfTypeInBackCacheMessage>(new InvalidateAllViewModelsOfTypeInBackCacheMessage(typeof (TilesViewModel)));
      this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
      {
        Enabled = true
      });
    }

    private async Task SaveManageTilesSettingsAsync()
    {
      ManageTilesViewModel.Logger.Debug((object) "<START> checking for Bluetooth connection");
      BandClass currentDevice;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        currentDevice = await this.bandHardwareService.GetDeviceTypeAsync(cancellationTokenSource.Token);
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
          await cargoConnection.CheckConnectionWorkingAsync(cancellationTokenSource.Token);
      }
      ManageTilesViewModel.Logger.Debug((object) "<END> checking for Bluetooth connection");
      ManageTilesViewModel.Logger.Debug((object) "<START> saving manage tiles choices");
      await this.SetTilesAsync(currentDevice);
      ManageTilesViewModel.Logger.Debug((object) "<END> saving manage tiles choices");
      ManageTilesViewModel.Logger.Debug((object) "<START> saving asynchronous manage tiles settings");
      await this.syncManager.UpdateTilesAsync(CancellationToken.None);
      await this.SetNotificationsSettingsAsync();
      await this.tileManagementService.ApplyPendingSettingsAsync();
      this.tileManagementService.PendingNotifications = (IList<string>) null;
      ManageTilesViewModel.Logger.Debug((object) "<END> saving asynchronous manage tiles settings");
    }

    private async Task<bool> CancelDisablingThirdPartyTilesAsync()
    {
      if (this.ThirdPartyTiles.Any<AppBandTile>((Func<AppBandTile, bool>) (s => s.IsThirdParty && !s.IsDirty && !s.ShowTile)))
      {
        if (await this.messageBoxService.ShowAsync(AppResources.ManageTilesDisableThirdPartyBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
          return true;
      }
      return false;
    }

    public ICommand EarlyUpdateCommand => (ICommand) this.earlyUpdateCommand ?? (ICommand) (this.earlyUpdateCommand = new HealthCommand(new Action(this.GoToEarlyUpdate)));

    private void GoToEarlyUpdate()
    {
      ApplicationTelemetry.LogEarlyUpdateSignUp();
      this.smoothNavService.Navigate(typeof (EarlyUpdateInfoViewModel));
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Save, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void EnableTileModification()
    {
      foreach (AppBandTile appBandTile in this.BaseTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty && !tile.IsDirty)))
        appBandTile.CanBeModified = true;
      foreach (AppBandTile appBandTile in this.ThirdPartyTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty && !tile.IsDirty)))
        appBandTile.CanBeModified = true;
      this.isMaxTiles = false;
    }

    private void DisableTileModification()
    {
      foreach (AppBandTile appBandTile in this.BaseTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty && !tile.ShowTile)))
        appBandTile.CanBeModified = false;
      foreach (AppBandTile appBandTile in this.ThirdPartyTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty && !tile.ShowTile && !tile.IsDirty)))
        appBandTile.CanBeModified = false;
      this.isMaxTiles = true;
    }

    private void MarkDisabledThirdPartyTilesAsDirty()
    {
      foreach (AppBandTile appBandTile in this.ThirdPartyTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty && !tile.ShowTile)))
      {
        appBandTile.CanBeModified = false;
        appBandTile.IsDirty = true;
      }
    }

    private async Task SetNotificationsSettingsAsync()
    {
      if (!this.tileManagementService.PendingNotifications.Any<string>())
        return;
      foreach (string pendingNotification in (IEnumerable<string>) this.tileManagementService.PendingNotifications)
      {
        string tileId = pendingNotification;
        AppBandTile enabledTile = this.tileManagementService.PendingTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId.ToString() == tileId));
        if (enabledTile != (AppBandTile) null)
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
            await cargoConnection.SetTileSettingsAsync(enabledTile.TileId, enabledTile.Settings);
        }
        enabledTile = (AppBandTile) null;
      }
    }

    private async Task SetTilesAsync(BandClass bandClass)
    {
      IEnumerable<AppBandTile> source1 = this.tileManagementService.EnabledTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty));
      if (!(source1 is IList<AppBandTile> appBandTileList1))
        appBandTileList1 = (IList<AppBandTile>) source1.ToList<AppBandTile>();
      IList<AppBandTile> first = appBandTileList1;
      IEnumerable<AppBandTile> source2 = this.tileManagementService.PendingTiles.Except<AppBandTile>((IEnumerable<AppBandTile>) first);
      IEnumerable<AppBandTile> source3 = first.Except<AppBandTile>((IEnumerable<AppBandTile>) this.tileManagementService.PendingTiles);
      if (!source2.Any<AppBandTile>() && !source3.Any<AppBandTile>() && !this.tileManagementService.HaveTilesBeenReordered)
        return;
      IEnumerable<AppBandTile> source4 = await this.tileManagementService.SetTilesAsync((ICollection<AppBandTile>) this.tileManagementService.PendingTiles, (ICollection<AdminBandTile>) this.originalTiles, bandClass);
      if (source4 == null)
        return;
      if (!(source4 is IList<AppBandTile> appBandTileList2))
        appBandTileList2 = (IList<AppBandTile>) source4.ToList<AppBandTile>();
      IList<AppBandTile> source5 = appBandTileList2;
      this.tileManagementService.PendingTiles = (IList<AppBandTile>) source5.ToList<AppBandTile>();
      this.tileManagementService.EnabledTiles = (IList<AppBandTile>) source5.ToList<AppBandTile>();
      this.TilesLeft = (int) this.maxTiles - this.tileManagementService.PendingTiles.Count<AppBandTile>();
      ApplicationTelemetry.LogTilesChanged(this.tileManagementService.PendingTiles.Count, this.tileManagementService.HaveTilesBeenReordered, this.tileManagementService.PendingTiles.Select<AppBandTile, string>((Func<AppBandTile, string>) (p =>
      {
        if (!p.IsThirdParty)
          return p.Title;
        return string.Format("{0} {1} {2}", new object[3]
        {
          (object) p.Title,
          (object) p.TileId,
          (object) p.ThirdPartyOwnerId
        });
      })));
      this.tileManagementService.HaveTilesBeenReordered = false;
    }

    private async Task<bool> VerifyAppRequirementsAsync(AppBandTile tile)
    {
      if (!tile.RequiresLocationServices || this.geolocationService.IsLocationAvailable)
        return await this.tileNotificationService.VerifyNotificationRequirementsAsync(tile);
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.EnableLocationServicesPromptBody, AppResources.EnableLocationServicesPromptTitle, PortableMessageBoxButton.OK);
      return false;
    }

    private void EnsureMessageRegistration()
    {
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.messageSender.Unregister<PanelRefreshMessage>((object) this, new Action<PanelRefreshMessage>(this.OnPanelRefresh));
      this.messageSender.Register<PanelRefreshMessage>((object) this, new Action<PanelRefreshMessage>(this.OnPanelRefresh));
    }

    private void RevertChanges()
    {
      this.ShowConfirmCancel = false;
      this.UnsubscribeFromTileChanges();
      this.suppressTileChanges = true;
      this.ClearTileCollections();
      foreach (AppBandTile enabledTile in (IEnumerable<AppBandTile>) this.tileManagementService.EnabledTiles)
      {
        if (!this.tileManagementService.PendingTiles.Contains(enabledTile) && enabledTile.TileId != Guid.Empty)
          this.tileManagementService.PendingTiles.Add(enabledTile.Copy());
      }
      foreach (AppBandTile tileFromApp in this.tileManagementService.KnownTiles.Where<AppBandTile>(new Func<AppBandTile, bool>(this.supportedTileService.IsSupportedByPlatform)))
        this.BaseTiles.Add(this.CopyPendingTiles(tileFromApp));
      foreach (AppBandTile appBandTile1 in this.tileManagementService.PendingTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (s => s.IsThirdParty)))
      {
        AppBandTile tileFromApp = appBandTile1;
        AppBandTile appBandTile2 = this.ThirdPartyTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (thirdParty => thirdParty.TileId == tileFromApp.TileId));
        if (appBandTile2 != (AppBandTile) null)
          appBandTile2.ShowTile = tileFromApp.ShowTile;
      }
      this.TilesLeft = (int) this.maxTiles - this.tileManagementService.PendingTiles.Count<AppBandTile>();
      this.tileManagementService.RevertPendingSettings();
      this.tileManagementService.HaveTilesChanged = false;
      this.tileManagementService.PendingNotifications = (IList<string>) null;
      this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
      {
        Enabled = true
      });
      this.SubscribeToTileChanges();
      this.suppressTileChanges = false;
    }

    private void VerifyHomeTiles()
    {
      if (this.config.EnabledTilesMatchesBand)
        return;
      this.messageSender.Send<InvalidateAllViewModelsOfTypeInBackCacheMessage>(new InvalidateAllViewModelsOfTypeInBackCacheMessage(typeof (TilesViewModel)));
      this.config.EnabledTilesMatchesBand = true;
    }

    private void ClearTileCollections()
    {
      this.BaseTiles.Clear();
      this.tileManagementService.PendingTiles.Clear();
    }

    private AppBandTile CopyPendingTiles(AppBandTile tileFromApp)
    {
      AppBandTile tile = tileFromApp.Copy();
      if (this.tileManagementService.PendingTiles.Contains(tileFromApp))
        tile.ShowTile = true;
      tile.IsCurrentlySupported = this.supportedTileService.IsCurrentlySupported(tile);
      return tile;
    }

    private void RefreshThirdPartyStatus() => this.HasThirdPartyTiles = this.ThirdPartyTiles.Any<AppBandTile>();

    private void SubscribeToTileChanges()
    {
      foreach (MvxNotifyPropertyChanged baseTile in (IEnumerable<AppBandTile>) this.BaseTiles)
        baseTile.PropertyChanged += new PropertyChangedEventHandler(this.TileOnPropertyChanged);
      foreach (MvxNotifyPropertyChanged thirdPartyTile in (IEnumerable<AppBandTile>) this.ThirdPartyTiles)
        thirdPartyTile.PropertyChanged += new PropertyChangedEventHandler(this.ThirdPartyTileOnPropertyChanged);
    }

    private void UnsubscribeFromTileChanges()
    {
      foreach (MvxNotifyPropertyChanged baseTile in (IEnumerable<AppBandTile>) this.BaseTiles)
        baseTile.PropertyChanged -= new PropertyChangedEventHandler(this.TileOnPropertyChanged);
      foreach (MvxNotifyPropertyChanged thirdPartyTile in (IEnumerable<AppBandTile>) this.ThirdPartyTiles)
        thirdPartyTile.PropertyChanged -= new PropertyChangedEventHandler(this.ThirdPartyTileOnPropertyChanged);
    }

    private void TileOnPropertyChanged(
      object sender,
      PropertyChangedEventArgs propertyChangedEventArgs)
    {
      AppBandTile appBandTile = (AppBandTile) sender;
      if (!(propertyChangedEventArgs.PropertyName == "ShowTile") || this.suppressTileChanges)
        return;
      this.ToggleTileCommand.Execute((object) appBandTile);
    }

    private void ThirdPartyTileOnPropertyChanged(
      object sender,
      PropertyChangedEventArgs propertyChangedEventArgs)
    {
      AppBandTile appBandTile = (AppBandTile) sender;
      if (!(propertyChangedEventArgs.PropertyName == "ShowTile") || this.suppressTileChanges)
        return;
      this.ToggleThirdPartyTileCommand.Execute((object) appBandTile);
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Dark;

    public ArgbColor32 Theme
    {
      get
      {
        ArgbColor32 colorBase = this.bandThemeManager.CurrentTheme.ColorSet.ColorBase;
        int color = (int) colorBase.Color;
        int num1 = 3932160;
        int num2 = 15360;
        int num3 = 60;
        return (long) (uint) (color & 16711680) < (long) num1 & (long) (uint) (color & 65280) < (long) num2 & (long) (uint) (color & (int) byte.MaxValue) < (long) num3 ? new ArgbColor32(4287137928U) : colorBase;
      }
    }

    private enum ManageTilesPageState
    {
      Loading,
      Loaded,
      Saving,
    }
  }
}
