// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.NavViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.ForegroundSync;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.App.Core.ViewModels.Golf;
using Microsoft.Health.App.Core.ViewModels.SendFeedback;
using Microsoft.Health.App.Core.ViewModels.WhatsNew;
using Microsoft.Health.App.Core.ViewModels.Workouts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public class NavViewModel : HealthViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\NavViewModel.cs");
    private readonly IOobeService oobeService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IMessageSender messageSender;
    private readonly IUserProfileService userProfileService;
    private readonly ISmoothNavService smoothNavService;
    private readonly ILauncherService launcherService;
    private readonly IPagePicker pagePicker;
    private readonly IWhatsNewService whatsNewService;
    private readonly IDeviceManager deviceManager;
    private readonly IDispatchService dispatchService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IRefreshService refreshService;
    private readonly IApplicationLifecycleService applicationLifecycleService;
    private readonly IUserService userService;
    private readonly ISyncEntryService syncEntryService;
    private bool isSyncing;
    private HealthCommand syncCommand;
    private IList<INavListItem> navChoices;
    private INavChoiceViewModel historyChoiceViewModel;
    private INavChoiceViewModel bandChoiceViewModel;
    private INavChoiceViewModel personalizeChoiceViewModel;
    private INavChoiceViewModel manageChoiceViewModel;
    private INavChoiceViewModel addBandChoiceViewModel;
    private INavChoiceViewModel phoneChoiceViewModel;

    public NavViewModel(
      IOobeService oobeService,
      IMessageBoxService messageBoxService,
      ILauncherService launcherService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IUserProfileService userProfileService,
      IPagePicker pagePicker,
      IWhatsNewService whatsNewService,
      IDeviceManager deviceManager,
      IDispatchService dispatchService,
      IErrorHandlingService errorHandlingService,
      IRefreshService refreshService,
      IApplicationLifecycleService applicationLifecycleService,
      IUserService userService,
      ISyncEntryService syncEntryService)
    {
      Assert.ParamIsNotNull((object) userService, nameof (userService));
      NavViewModel.Logger.Debug((object) "Begin NavViewModel()");
      this.oobeService = oobeService;
      this.messageBoxService = messageBoxService;
      this.launcherService = launcherService;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.userProfileService = userProfileService;
      this.pagePicker = pagePicker;
      this.whatsNewService = whatsNewService;
      this.deviceManager = deviceManager;
      this.dispatchService = dispatchService;
      this.errorHandlingService = errorHandlingService;
      this.refreshService = refreshService;
      this.applicationLifecycleService = applicationLifecycleService;
      this.userService = userService;
      this.syncEntryService = syncEntryService;
      this.LastSyncTimes = (IList<DeviceSyncTime>) new ObservableCollection<DeviceSyncTime>();
      this.Setup();
      this.deviceManager.SyncStateChanged += new EventHandler<SyncStateChangedEventArgs>(this.OnSyncStateChanged);
      this.deviceManager.DevicesChanged += new EventHandler(this.OnRefreshRequested);
      this.refreshService.Subscribe((object) this, (Func<CancellationToken, Task>) (cancellationToken =>
      {
        this.dispatchService.TryRunOnUIThreadAsync((Action) (() => this.Refresh())).IgnoreException("RunOnUIThreadAsync.", ".ctor");
        return (Task) Task.FromResult<bool>(true);
      }));
      this.applicationLifecycleService.Resuming += new EventHandler<object>(this.OnResume);
      this.messageSender.Register<BandRegistrationChangedMessage>((object) this, new Action<BandRegistrationChangedMessage>(this.OnBandRegistrationChanged));
      this.messageSender.Register<AnyBandRegisteredMessage>((object) this, new Action<AnyBandRegisteredMessage>(this.OnAnyBandRegistered));
      NavViewModel.Logger.Debug((object) "End NavViewModel()");
    }

    private void OnAnyBandRegistered(AnyBandRegisteredMessage message) => this.UpdateNavChoices(includeActivityHistory: new bool?(message.HasAnyBandBeenRegistered));

    private async void Setup()
    {
      IList<IDevice> devices = (IList<IDevice>) null;
      try
      {
        IList<IDevice> deviceList = devices;
        devices = (IList<IDevice>) (await this.deviceManager.GetDevicesAsync(CancellationToken.None).ConfigureAwait(false)).ToList<IDevice>();
      }
      catch (Exception ex)
      {
        NavViewModel.Logger.Error(ex, "<FAILED> getting devices.");
      }
      this.dispatchService.RunOnUIThreadAsync((Action) (() =>
      {
        this.UpdateNavChoices((IEnumerable<IDevice>) devices);
        this.Refresh((IEnumerable<IDevice>) devices);
      })).IgnoreException("RunOnUIThreadAsync.", nameof (Setup));
    }

    private async void UpdateNavChoices(IEnumerable<IDevice> devices = null, bool? includeActivityHistory = null)
    {
            int a = this.userProfileService.HasAnyBandBeenRegistered ? 1 : 0;
            bool b = (bool)(includeActivityHistory ?? a != 0);
            this.HistoryChoiceViewModel.IsEnabled = b;
               
      this.RaisePropertyChanged("HistoryChoiceViewModel");
      bool localIncludeMyBand = this.userProfileService.IsBandRegistered;
      int num = await this.IsPhoneIncludedAsync(devices, CancellationToken.None) ? 1 : 0;
      this.UpdateDeviceNavChoices(localIncludeMyBand, num != 0);
    }

    private async Task ResetOobeAsync()
    {
      if (!await this.oobeService.ResetOobeStatusAsync())
        return;
      this.messageSender.Send<NavChoiceMessage>(new NavChoiceMessage()
      {
        Enabled = true
      });
      this.smoothNavService.Navigate(typeof (FreshInstallLoadingViewModel));
    }

    private async Task SignOutAsync()
    {
      if (await this.messageBoxService.ShowAsync(AppResources.SignOutConfirmationMessage, AppResources.ApplicationTitle, PortableMessageBoxButton.OKCancel, AppResources.SignOut, AppResources.Cancel) != PortableMessageBoxResult.OK)
        return;
      await this.userService.SignOutAsync();
    }

    private void UpdateDeviceNavChoices(bool localIncludeMyBand, bool includeMyPhone)
    {
      this.BandChoiceViewModel.IsEnabled = localIncludeMyBand;
      this.PersonalizeChoiceViewModel.IsEnabled = localIncludeMyBand;
      this.ManageChoiceViewModel.IsEnabled = localIncludeMyBand;
      this.AddBandChoiceViewModel.Name = localIncludeMyBand ? AppResources.NavigationReplaceMyBand : AppResources.NavigationAddABand;
      this.AddBandChoiceViewModel.GlyphIcon = localIncludeMyBand ? "\uE140" : "\uE042";
      this.PhoneChoiceViewModel.IsEnabled = includeMyPhone;
    }

    private NavChoiceViewModel CreateNavChoice<T>(string name, string icon) => this.CreateNavChoice(name, icon, typeof (T));

    private NavChoiceViewModel CreateNavChoice(
      string name,
      string icon,
      System.Type viewModelType)
    {
      return new NavChoiceViewModel(this.messageBoxService, this.launcherService, this.smoothNavService, this.messageSender)
      {
        Name = name,
        GlyphIcon = icon,
        Page = viewModelType
      };
    }

    private async Task<bool> IsPhoneIncludedAsync(
      IEnumerable<IDevice> devices,
      CancellationToken token)
    {
      bool includeMyPhone = false;
      try
      {
        IEnumerable<IDevice> source = devices;
        if (source == null)
          source = (IEnumerable<IDevice>) await this.deviceManager.GetDevicesAsync(CancellationToken.None);
        includeMyPhone = source.Any<IDevice>((Func<IDevice, bool>) (p => p.DeviceType == DeviceType.Phone));
      }
      catch (Exception ex)
      {
        NavViewModel.Logger.Error((object) "<FAILED> getting available devices.", ex);
      }
      return includeMyPhone;
    }

    public INavChoiceViewModel HistoryChoiceViewModel => this.historyChoiceViewModel ?? (this.historyChoiceViewModel = (INavChoiceViewModel) this.CreateNavChoice<HistoryViewModel>(AppResources.NavigationHistory, "\uE050"));

    public INavChoiceViewModel BandChoiceViewModel => this.bandChoiceViewModel ?? (this.bandChoiceViewModel = (INavChoiceViewModel) this.CreateNavChoice<MyBandViewModel>(AppResources.NavigationMyBand, "\uE175"));

    public INavChoiceViewModel PersonalizeChoiceViewModel => this.personalizeChoiceViewModel ?? (this.personalizeChoiceViewModel = (INavChoiceViewModel) this.CreateNavChoice<PersonalizeBandViewModel>(AppResources.NavigationPersonalize, "\uE043"));

    public INavChoiceViewModel ManageChoiceViewModel => this.manageChoiceViewModel ?? (this.manageChoiceViewModel = (INavChoiceViewModel) this.CreateNavChoice<ManageTilesViewModel>(AppResources.NavigationManageTiles, "\uE182"));

    public INavChoiceViewModel AddBandChoiceViewModel
    {
      get
      {
        INavChoiceViewModel bandChoiceViewModel = this.addBandChoiceViewModel;
        if (bandChoiceViewModel != null)
          return bandChoiceViewModel;
        NavCommandViewModel commandViewModel = new NavCommandViewModel();
        commandViewModel.Name = this.userProfileService.IsRegisteredBandPaired ? AppResources.NavigationReplaceMyBand : AppResources.NavigationAddABand;
        commandViewModel.GlyphIcon = this.userProfileService.IsRegisteredBandPaired ? "\uE140" : "\uE042";
        commandViewModel.ActionCommand = (ICommand) AsyncHealthCommand.Create(new Func<Task>(this.AddBandAsync));
        INavChoiceViewModel navChoiceViewModel = (INavChoiceViewModel) commandViewModel;
        this.addBandChoiceViewModel = (INavChoiceViewModel) commandViewModel;
        return navChoiceViewModel;
      }
    }

    public INavChoiceViewModel PhoneChoiceViewModel => this.phoneChoiceViewModel ?? (this.phoneChoiceViewModel = (INavChoiceViewModel) this.CreateNavChoice(AppResources.NavigationMyPhone, "\uE143", this.pagePicker.NavPhoneChoice));

    public IList<INavListItem> NavChoices
    {
      get
      {
        if (this.navChoices == null)
        {
          List<INavListItem> navListItemList = new List<INavListItem>();
          navListItemList.Add((INavListItem) this.CreateNavChoice<TilesViewModel>(AppResources.NavigationHome, "\uE129"));
          navListItemList.Add((INavListItem) this.HistoryChoiceViewModel);
          navListItemList.Add((INavListItem) this.CreateNavChoice<SettingsProfileViewModel>(AppResources.NavigationProfile, "\uE173"));
          navListItemList.Add((INavListItem) this.CreateNavChoice<WorkoutPlanLandingViewModel>(AppResources.NavigationBrowseWorkouts, "\uE003"));
          navListItemList.Add((INavListItem) new NavCommandViewModel()
          {
            Name = AppResources.NavigationFindGolfCourse,
            GlyphIcon = "\uE159",
            ActionCommand = (ICommand) new HealthCommand((Action) (() =>
            {
              ApplicationTelemetry.LogGolfFindCourse("Menu");
              this.smoothNavService.Navigate(typeof (GolfLandingViewModel));
            }))
          });
          WhatsNewNavChoiceViewModel navChoiceViewModel = new WhatsNewNavChoiceViewModel(this.messageBoxService, this.launcherService, this.smoothNavService, this.messageSender, this.whatsNewService);
          navChoiceViewModel.Name = AppResources.NavigationWhatsNew;
          navChoiceViewModel.GlyphIcon = "\uE071";
          navChoiceViewModel.Page = typeof (WhatsNewViewModel);
          navListItemList.Add((INavListItem) navChoiceViewModel);
          navListItemList.Add((INavListItem) this.CreateNavChoice<ConnectedAppsViewModel>(AppResources.LeftNavConnectedApps, "\uE147"));
          navListItemList.Add((INavListItem) new NavSectionHeaderViewModel(AppResources.NavigationDeviceSettingsLabel));
          navListItemList.Add((INavListItem) this.BandChoiceViewModel);
          navListItemList.Add((INavListItem) this.PersonalizeChoiceViewModel);
          navListItemList.Add((INavListItem) this.ManageChoiceViewModel);
          navListItemList.Add((INavListItem) this.AddBandChoiceViewModel);
          navListItemList.Add((INavListItem) this.PhoneChoiceViewModel);
          navListItemList.Add((INavListItem) new NavSectionHeaderViewModel(AppResources.NavigationSettingsLabel));
          navListItemList.Add((INavListItem) this.CreateNavChoice(AppResources.NavigationPreferences, "\uE044", this.pagePicker.Preferences));
          navListItemList.Add((INavListItem) this.CreateNavChoice<AboutViewModel>(AppResources.About, "\uE172"));
          navListItemList.Add((INavListItem) this.CreateNavChoice<HelpAndFeedbackViewModel>(AppResources.NavigationHelpAndFeedback, "\uE183"));
          this.navChoices = (IList<INavListItem>) navListItemList;
          if (this.userService.SupportsSignOut)
            this.navChoices.Add((INavListItem) new NavCommandViewModel()
            {
              Name = AppResources.SignOut,
              GlyphIcon = "\uE184",
              ActionCommand = (ICommand) AsyncHealthCommand.Create(new Func<Task>(this.SignOutAsync))
            });
          if (EnvironmentUtilities.IsDebugSettingEnabled)
          {
            this.navChoices.Add((INavListItem) new NavSectionHeaderViewModel("Debug"));
            this.navChoices.Add((INavListItem) this.CreateNavChoice("Debug", "\uE046", this.pagePicker.Debug));
            this.navChoices.Add((INavListItem) new NavCommandViewModel()
            {
              Name = "Reset Oobe",
              GlyphIcon = "\uE006",
              ActionCommand = (ICommand) AsyncHealthCommand.Create(new Func<Task>(this.ResetOobeAsync))
            });
          }
        }
        return this.navChoices;
      }
    }

    public IList<DeviceSyncTime> LastSyncTimes { get; set; }

    public ICommand SyncCommand => (ICommand) this.syncCommand ?? (ICommand) (this.syncCommand = new HealthCommand(new Action(this.Sync)));

    private void Sync()
    {
      this.messageSender.Send<DeviceDrawerCloseMessage>(new DeviceDrawerCloseMessage());
      if (this.isSyncing)
        return;
      this.syncEntryService.Sync(SyncType.Manual, TimeSpan.Zero);
    }

    private void OnBandRegistrationChanged(BandRegistrationChangedMessage message) => this.dispatchService.TryRunOnUIThreadAsync((Action) (() =>
    {
      this.UpdateNavChoices();
      this.Refresh();
    })).IgnoreException("RunOnUIThreadAsync.", nameof (OnBandRegistrationChanged));

    private void OnSyncStateChanged(object sender, SyncStateChangedEventArgs eventArgs)
    {
      if (this.isSyncing == eventArgs.IsSyncing)
        return;
      this.isSyncing = eventArgs.IsSyncing;
      this.dispatchService.TryRunOnUIThreadAsync((Action) (() =>
      {
        if (this.isSyncing)
          return;
        this.Refresh();
      }), false).IgnoreException("RunOnUIThreadAsync.", nameof (OnSyncStateChanged));
    }

    private void OnRefreshRequested(object sender, EventArgs eventArgs) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.Refresh()), false).IgnoreException("RunOnUIThreadAsync.", nameof (OnRefreshRequested));

    private void OnResume(object sender, object e) => this.dispatchService.RunOnUIThreadAsync((Action) (() => this.Refresh()), false).IgnoreException("RunOnUIThreadAsync.", nameof (OnResume));

    private async void Refresh(IEnumerable<IDevice> devices = null)
    {
      try
      {
        IEnumerable<IDevice> source = devices;
        if (source == null)
          source = (IEnumerable<IDevice>) await this.deviceManager.GetDevicesAsync(CancellationToken.None);
        List<Task<DeviceSyncTime>> refreshTasks = source.Select<IDevice, Task<DeviceSyncTime>>((Func<IDevice, Task<DeviceSyncTime>>) (device => NavViewModel.GetDeviceSyncTimeAsync(device, CancellationToken.None))).ToList<Task<DeviceSyncTime>>();
        DeviceSyncTime[] deviceSyncTimeArray = await Task.WhenAll<DeviceSyncTime>((IEnumerable<Task<DeviceSyncTime>>) refreshTasks);
        this.LastSyncTimes.Clear();
        foreach (Task<DeviceSyncTime> task in refreshTasks)
        {
          if (task.Result != null)
            this.LastSyncTimes.Add(task.Result);
        }
        refreshTasks = (List<Task<DeviceSyncTime>>) null;
      }
      catch (Exception ex)
      {
        NavViewModel.Logger.Error(ex, "<FAILED> refresh last sync time.");
      }
    }

    private static async Task<DeviceSyncTime> GetDeviceSyncTimeAsync(
      IDevice device,
      CancellationToken token)
    {
      DateTimeOffset? lastSyncTimeAsync = await device.GetLastSyncTimeAsync(token);
      return lastSyncTimeAsync.HasValue ? new DeviceSyncTime(device.DeviceType, lastSyncTimeAsync) : (DeviceSyncTime) null;
    }

    private async Task AddBandAsync()
    {
      try
      {
        bool shouldStartAddBandProcess = false;
        if (this.userProfileService.IsBandRegistered)
        {
          if (await this.messageBoxService.ShowAsync(AppResources.RegisterUnregisterBandConfirmationBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
          {
            this.deviceManager.CancelDevicesSync();
            shouldStartAddBandProcess = true;
          }
        }
        else
          shouldStartAddBandProcess = true;
        if (shouldStartAddBandProcess)
          this.smoothNavService.Navigate(typeof (AddBandStartViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "IsOobe",
              bool.FalseString
            }
          });
      }
      catch (Exception ex)
      {
        NavViewModel.Logger.Error((object) ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    }
  }
}
