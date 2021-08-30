// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PreferencesViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Social;
using Microsoft.Health.App.Core.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.HomeShell)]
  [PageTaxonomy(new string[] {"Settings", "User", "Preferences"})]
  public class PreferencesViewModel : PageViewModelBase, IHomeShellViewModel
  {
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\PreferencesViewModel.cs");
    private readonly Lazy<IList<LabeledItem<DistanceUnitType>>> lazyDistanceUnitTypesList = new Lazy<IList<LabeledItem<DistanceUnitType>>>(new Func<IList<LabeledItem<DistanceUnitType>>>(LabeledItem<DistanceUnitType>.FromEnumValues));
    private readonly Lazy<IList<LabeledItem<TemperatureUnitType>>> lazyTemperatureUnitTypesList = new Lazy<IList<LabeledItem<TemperatureUnitType>>>(new Func<IList<LabeledItem<TemperatureUnitType>>>(LabeledItem<TemperatureUnitType>.FromEnumValues));
    private readonly Lazy<IList<LabeledItem<MassUnitType>>> lazyMassUnitTypesList = new Lazy<IList<LabeledItem<MassUnitType>>>(new Func<IList<LabeledItem<MassUnitType>>>(LabeledItem<MassUnitType>.FromEnumValues));
    private readonly Lazy<IList<LabeledItem<SyncFrequency>>> lazySyncFrequenciesList = new Lazy<IList<LabeledItem<SyncFrequency>>>(new Func<IList<LabeledItem<SyncFrequency>>>(LabeledItem<SyncFrequency>.FromEnumValues));
    private readonly Lazy<HealthCommand> cancelCommand;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly Lazy<HealthCommand> confirmCommand;
    private readonly IMessageSender messageSender;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly IUserProfileService userProfileService;
    private readonly IShakeFeedbackService shakeFeedbackService;
    private readonly ILocaleSettingsProvider localeService;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IConfig config;
    protected readonly ISmoothNavService SmoothNavService;
    private readonly ISocialEngagementService socialEngagementService;
    private bool displaySaveChanges;
    private bool isLoaded;
    private LabeledItem<DistanceUnitType> selectedDistanceUnitType;
    private LabeledItem<MassUnitType> selectedMassUnitType;
    private LabeledItem<TemperatureUnitType> selectedTemperatureUnitType;
    private LabeledItem<SyncFrequency> selectedSyncFrequency;
    private ILocaleSettings selectedLocaleSettings;
    private bool isShakeFeedbackEnabled;
    private bool showGettingProfileProgress;
    private bool showPreferencesPanel;
    private bool showSavingProfileProgress;
    private bool isSocialTileEnabled;
    private IList<ILocaleSettings> localeSettingsList;
    private bool isUsingEnvoy;

    public PreferencesViewModel(
      IErrorHandlingService errorHandlingService,
      ISmoothNavService smoothNavService,
      IUserProfileService userProfileService,
      IShakeFeedbackService shakeFeedbackService,
      INetworkService networkService,
      ILocaleSettingsProvider localeService,
      IBandHardwareService bandHardwareService,
      IMessageSender messageSender,
      IDynamicConfigurationService dynamicConfigurationService,
      IConfig config,
      ISocialEngagementService socialEngagementService)
      : base(networkService)
    {
      this.errorHandlingService = errorHandlingService;
      this.SmoothNavService = smoothNavService;
      this.userProfileService = userProfileService;
      this.shakeFeedbackService = shakeFeedbackService;
      this.localeService = localeService;
      this.bandHardwareService = bandHardwareService;
      this.messageSender = messageSender;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.config = config;
      this.socialEngagementService = socialEngagementService;
      this.isSocialTileEnabled = this.socialEngagementService.IsSocialEnabled;
      this.confirmCommand = new Lazy<HealthCommand>((Func<HealthCommand>) (() => new HealthCommand(new Action(this.SavePreferences))));
      this.cancelCommand = new Lazy<HealthCommand>((Func<HealthCommand>) (() => new HealthCommand((Action) (() =>
      {
        this.DisplaySaveChanges = false;
        this.IsLoaded = false;
        this.Refresh();
      }))));
      this.isUsingEnvoy = false;
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Dark;

    public bool ShowGettingProfileProgress
    {
      get => this.showGettingProfileProgress;
      set
      {
        this.SetProperty<bool>(ref this.showGettingProfileProgress, value, nameof (ShowGettingProfileProgress));
        this.UpdateShowPreferencesPanel();
      }
    }

    public bool ShowSavingProfileProgress
    {
      get => this.showSavingProfileProgress;
      set
      {
        this.SetProperty<bool>(ref this.showSavingProfileProgress, value, nameof (ShowSavingProfileProgress));
        this.UpdateShowPreferencesPanel();
      }
    }

    public bool ShowPreferencesPanel
    {
      get => this.showPreferencesPanel;
      set => this.SetProperty<bool>(ref this.showPreferencesPanel, value, nameof (ShowPreferencesPanel));
    }

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand.Value;

    public ICommand CancelCommand => (ICommand) this.cancelCommand.Value;

    public IList<LabeledItem<DistanceUnitType>> DistanceUnitTypes => this.lazyDistanceUnitTypesList.Value;

    public IList<LabeledItem<MassUnitType>> MassUnitTypes => this.lazyMassUnitTypesList.Value;

    public IList<LabeledItem<TemperatureUnitType>> TemperatureUnitTypes => this.lazyTemperatureUnitTypesList.Value;

    public IList<LabeledItem<SyncFrequency>> SyncFrequencies => this.lazySyncFrequenciesList.Value;

    public IList<ILocaleSettings> LocaleSettings
    {
      get => this.localeSettingsList;
      set => this.SetProperty<IList<ILocaleSettings>>(ref this.localeSettingsList, value, nameof (LocaleSettings));
    }

    public ILocaleSettings SelectedLocaleSettings
    {
      get => this.selectedLocaleSettings;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<ILocaleSettings>(ref this.selectedLocaleSettings, value, nameof (SelectedLocaleSettings));
      }
    }

    public LabeledItem<MassUnitType> SelectedMassUnitType
    {
      get => this.selectedMassUnitType;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<LabeledItem<MassUnitType>>(ref this.selectedMassUnitType, value, nameof (SelectedMassUnitType));
      }
    }

    public LabeledItem<TemperatureUnitType> SelectedTemperatureUnitType
    {
      get => this.selectedTemperatureUnitType;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<LabeledItem<TemperatureUnitType>>(ref this.selectedTemperatureUnitType, value, nameof (SelectedTemperatureUnitType));
      }
    }

    public LabeledItem<DistanceUnitType> SelectedDistanceUnitType
    {
      get => this.selectedDistanceUnitType;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<LabeledItem<DistanceUnitType>>(ref this.selectedDistanceUnitType, value, nameof (SelectedDistanceUnitType));
      }
    }

    public LabeledItem<SyncFrequency> SelectedSyncFrequency
    {
      get => this.selectedSyncFrequency;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<LabeledItem<SyncFrequency>>(ref this.selectedSyncFrequency, value, nameof (SelectedSyncFrequency));
      }
    }

    public bool IsShakeFeedbackEnabled
    {
      get => this.isShakeFeedbackEnabled;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<bool>(ref this.isShakeFeedbackEnabled, value, nameof (IsShakeFeedbackEnabled));
      }
    }

    public bool IsSocialTileEnabled
    {
      get => this.isSocialTileEnabled;
      set
      {
        if (this.IsLoaded && !this.DisplaySaveChanges)
          this.DisplaySaveChanges = true;
        this.SetProperty<bool>(ref this.isSocialTileEnabled, value, nameof (IsSocialTileEnabled));
      }
    }

    public bool IsUsingEnvoy
    {
      get => this.isUsingEnvoy;
      set => this.SetProperty<bool>(ref this.isUsingEnvoy, value, nameof (IsUsingEnvoy));
    }

    public bool IsLoaded
    {
      get => this.isLoaded;
      set => this.SetProperty<bool>(ref this.isLoaded, value, nameof (IsLoaded));
    }

    public bool DisplaySaveChanges
    {
      get => this.displaySaveChanges;
      set
      {
        this.SetProperty<bool>(ref this.displaySaveChanges, value, nameof (DisplaySaveChanges));
        this.UpdateAppBar();
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      PreferencesViewModel.Logger.Debug((object) "<START> loading preferences page");
      try
      {
        this.ShowGettingProfileProgress = true;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          if (this.LocaleSettings == null)
            this.LocaleSettings = await this.localeService.LoadLocaleSettingsAsync(cancellationTokenSource.Token);
          if (this.userProfileService.IsRegisteredBandPaired)
          {
            await this.bandHardwareService.ClearDeviceTypeAsync(cancellationTokenSource.Token);
            try
            {
              if (await this.bandHardwareService.GetDeviceTypeAsync(cancellationTokenSource.Token) == BandClass.Envoy)
                this.IsUsingEnvoy = true;
            }
            catch
            {
              PreferencesViewModel.Logger.Debug((object) "unable to verify if paired device is envoy");
            }
          }
        }
        this.SetCurrentValuesFromUserProfile();
        this.IsShakeFeedbackEnabled = this.shakeFeedbackService.IsEnabled;
        this.IsSocialTileEnabled = this.socialEngagementService.IsSocialEnabled;
        this.ShowGettingProfileProgress = false;
        PreferencesViewModel.Logger.Debug((object) "<END> loading preferences page");
      }
      catch (Exception ex)
      {
        PreferencesViewModel.Logger.Error(ex, "<FAILED> loading preferences page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.SmoothNavService.GoBack();
      }
      this.IsLoaded = true;
      await base.LoadDataAsync(parameters);
    }

    protected override void OnBackNavigation()
    {
      this.UpdateAppBar();
      base.OnBackNavigation();
    }

    private void UpdateShowPreferencesPanel() => this.ShowPreferencesPanel = !this.ShowSavingProfileProgress && !this.ShowGettingProfileProgress;

    private void SetCurrentValuesFromUserProfile()
    {
      PreferencesViewModel.Logger.Debug((object) "<START> loading current preferences selections");
      this.UpdateSelectedPreferences();
      PreferencesViewModel.Logger.Debug((object) string.Format("<END> loading current preferences selections (mass={0},temp={1},dist={2})", new object[3]
      {
        (object) this.SelectedMassUnitType,
        (object) this.SelectedTemperatureUnitType,
        (object) this.SelectedDistanceUnitType
      }));
    }

    private void UpdateSelectedPreferences()
    {
      this.SelectedMassUnitType = LabeledItem<MassUnitType>.Find(this.userProfileService.MassUnitType, (IEnumerable<LabeledItem<MassUnitType>>) this.MassUnitTypes);
      this.SelectedTemperatureUnitType = LabeledItem<TemperatureUnitType>.Find(this.userProfileService.TemperatureUnitType, (IEnumerable<LabeledItem<TemperatureUnitType>>) this.TemperatureUnitTypes);
      this.SelectedDistanceUnitType = LabeledItem<DistanceUnitType>.Find(this.userProfileService.DistanceUnitType, (IEnumerable<LabeledItem<DistanceUnitType>>) this.DistanceUnitTypes);
      if (this.IsUsingEnvoy)
        this.SelectedLocaleSettings = this.LocaleSettings.Where<ILocaleSettings>((Func<ILocaleSettings, bool>) (p => p.Locale == this.userProfileService.CurrentUserProfile.LocaleSettings.LocaleId)).FirstOrDefault<ILocaleSettings>();
      this.SelectedSyncFrequency = LabeledItem<SyncFrequency>.Find(this.config.BackgroundSyncFrequency, (IEnumerable<LabeledItem<SyncFrequency>>) this.SyncFrequencies);
    }

    protected virtual async void SavePreferences()
    {
      PreferencesViewModel.Logger.Debug((object) string.Format("<START> saving current preferences selections (mass={0},temp={1},dist={2})", new object[3]
      {
        (object) this.SelectedMassUnitType,
        (object) this.SelectedTemperatureUnitType,
        (object) this.SelectedDistanceUnitType
      }));
      try
      {
        this.DisplaySaveChanges = false;
        this.ShowSavingProfileProgress = true;
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
        {
          BandUserProfile userProfileAsync = await this.userProfileService.GetCloudUserProfileAsync(cancellationTokenSource.Token);
          if (userProfileAsync.LocaleSettings == null)
            userProfileAsync.LocaleSettings = LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults);
          userProfileAsync.LocaleSettings.MassUnits = this.selectedMassUnitType.Value;
          userProfileAsync.LocaleSettings.TemperatureUnits = this.selectedTemperatureUnitType.Value;
          userProfileAsync.LocaleSettings.DistanceLongUnits = this.selectedDistanceUnitType.Value;
          userProfileAsync.LocaleSettings.DistanceShortUnits = this.selectedDistanceUnitType.Value;
          if (this.IsUsingEnvoy)
            userProfileAsync.ApplyLocaleSettings(this.SelectedLocaleSettings);
          await this.userProfileService.SaveUserProfileAsync(userProfileAsync, cancellationTokenSource.Token);
        }
        if (this.IsSocialTileEnabled != this.socialEngagementService.IsSocialEnabled)
          await this.socialEngagementService.ToggleSocialAsync(SocialRemoveType.Preferences);
        this.messageSender.Send<InvalidateAllViewModelsOfTypeInBackCacheMessage>(new InvalidateAllViewModelsOfTypeInBackCacheMessage(typeof (TilesViewModel)));
        if (this.shakeFeedbackService.IsEnabled != this.IsShakeFeedbackEnabled)
          ApplicationTelemetry.LogFeedbackShakeSetting(this.IsShakeFeedbackEnabled);
        this.shakeFeedbackService.IsEnabled = this.IsShakeFeedbackEnabled;
        this.config.BackgroundSyncFrequency = this.SelectedSyncFrequency.Value;
        this.ShowSavingProfileProgress = false;
        PreferencesViewModel.Logger.Debug((object) "<END> saving current preferences selections");
      }
      catch (Exception ex)
      {
        PreferencesViewModel.Logger.Error(ex, "<FAILED> saving current preferences selections");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.DisplaySaveChanges = true;
        this.ShowSavingProfileProgress = false;
      }
    }

    private void UpdateAppBar()
    {
      if (this.DisplaySaveChanges)
        this.ShowAppBar();
      else
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Save, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });
  }
}
