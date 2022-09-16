// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ViewModel
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopSyncApp
{
  public partial class ViewModel : INotifyPropertyChanged
  {
    public static Visibility FallbackVisibility;
    private Command minimizeAppCommand;
    private Command hideAppCommand;
    private Command showAppCommand;
    private Command showSettingsCommand;
    private Command showAboutCommand;
    private Command showProfileCommand;
    private Command showSyncCommand;
    private Command showTileManagementCommand;
    private Command showBandCustomizationCommand;
    private Command closeAppCommand;
    private Command logoutConfirmCommand;
    private Command logoutCommand;
    private Command pairDeviceConfirmCommand;
    private Command pairDeviceCommand;
    private Command forgetDeviceConfirmCommand;
    private Command forgetDeviceCommand;
    private Command checkForFirmwareUpdate;
    private Command pushFirmwareToDevice;
    private Command checkOrOfferAppUpdate;
    private Command launchAppUpdate;
    private Command launchTermsOfUseWeb;
    private Command launchDownloadWeb;
    private Command launchPrivacyPolicyWeb;
    private Command launchSupportWeb;
    private Command launchThirdPartyNoticesWeb;
    private Command uninstallApp;
    private Command beginEditUserProfile;
    private Command endEditUserProfile;
    private Command launchDashboardCommand;
    private Dictionary<string, IBandInfo> presentDevices = new Dictionary<string, IBandInfo>();
    private SecurityInfo securityInfo;
    private LoginInfo loginInfo;
    private UserDeviceStatus userDeviceStatus;
    private LoginLogoutStatus loginLogoutStatus;
    private Visibility appVisibility;
    private WindowState appState;
    private LinkedList<ErrorInfo> errorLog = new LinkedList<ErrorInfo>();
    private ErrorInfo lastLoginError;
    private ErrorInfo lastAppUpdateCheckError;
    private bool debugPageEnabled;
    private bool debugEnabled;
    private bool provideBogusAuthTokensToKDK;
    private bool forceFirmwareUpdateCheck;
    private KDeviceManager deviceManager;
    private bool batteryPlugVisible = true;
    private HeartBeatService heartBeat01Sec;
    private HeartBeatService heartBeat05Sec;
    private HeartBeatService heartBeat10Min;
    private DispatcherTimer heartBeatTimer01Sec;
    private DispatcherTimer heartBeatTimer05Sec;
    private DispatcherTimer heartBeatTimer10Min;
    private bool aquiringDevice;
    private bool checkingAppUpdate;
    private AppUpdateCheckStatus appUpdateCheckStatus;
    private AppUpdateStatus appUpdateStatus;
    private DispatcherTimer appUpdateCheckTimer;
    private CancellationTokenSource userProfileGetCancel;
    private ApplicationInsightsTelemetryListener insightsListener;
    private DynamicGlobalizationConfiguration dynamicGlobalizationConfig;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyValueChangedEventHandler UserDeviceStatusChanged;

    public event PropertyValueChangedEventHandler LoginLogoutStatusChanged;

    public event PropertyValueChangedEventHandler AppVisibilityChanged;

    public event EventHandler OOBECompleted;

    public event PropertyValueChangedEventHandler LoginInfoChanged;

    public ViewModel(
      DesktopSyncApp.App app,
      DynamicSettings dynamicSettings,
      CommandLineSettings commandLineSettings)
    {
      ViewModel.FallbackVisibility = Visibility.Hidden;
      this.App = app;
      this.DynamicSettings = dynamicSettings;
      this.CommandLineSettings = commandLineSettings;
      this.deviceManager = new KDeviceManager(this);
      this.ThemeManager = new DeviceThemeManager();
      this.StrapManager = new StartStripManager();
      this.HideAfterLogin = commandLineSettings.Hidden;
      this.heartBeat01Sec = new HeartBeatService();
      this.heartBeat05Sec = new HeartBeatService();
      this.heartBeat10Min = new HeartBeatService();
      this.heartBeatTimer01Sec = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.heartBeatTimer01Sec_Tick), DesktopSyncApp.App.Current.Dispatcher);
      this.heartBeatTimer01Sec.Start();
      this.heartBeatTimer05Sec = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Normal, new EventHandler(this.heartBeatTimer05Sec_Tick), DesktopSyncApp.App.Current.Dispatcher);
      this.heartBeatTimer05Sec.Start();
      this.heartBeatTimer10Min = new DispatcherTimer(new TimeSpan(0, 10, 0), DispatcherPriority.Normal, new EventHandler(this.heartBeatTimer10Min_Tick), DesktopSyncApp.App.Current.Dispatcher);
      this.heartBeatTimer10Min.Start();
      this.heartBeat01Sec.Beat += new HeartBeatHandler(this.heartBeat01Sec_Beat);
      this.appUpdateCheckTimer = new DispatcherTimer();
      this.appUpdateCheckTimer.Interval = TimeSpan.FromSeconds(10.0);
      this.appUpdateCheckTimer.Tick += new EventHandler(this.appUpdateCheckTimer_Tick);
      this.appUpdateCheckTimer.Start();
      this.dynamicGlobalizationConfig = new DynamicGlobalizationConfiguration(RegionInfo.CurrentRegion.TwoLetterISORegionName);
      this.insightsListener = new ApplicationInsightsTelemetryListener(DesktopTelemetry.GetInstrumentationKey());
      Telemetry.AddListener((ITelemetryListener) this.insightsListener);
    }

    public ApplicationInsightsTelemetryListener TelemetryListener => this.insightsListener;

    public DynamicGlobalizationConfiguration DynamicGlobalizationConfig => this.dynamicGlobalizationConfig;

    public void SaveInsightsData()
    {
      if (this.insightsListener == null)
        return;
      this.insightsListener.Flush();
    }

    private void heartBeat01Sec_Beat(object sender, EventArgs e)
    {
      if (this.AppUpdateCheckStatus == null)
        return;
      this.OnPropertyChanged("LastAppUpdateCheckRelativeTime", this.PropertyChanged);
    }

    public bool HideAfterLogin { get; set; }

    public DesktopSyncApp.App App { get; private set; }

    public HeartBeatService HeartBeat01Sec => this.heartBeat01Sec;

    public HeartBeatService HeartBeat05Sec => this.heartBeat05Sec;

    public HeartBeatService HeartBeat10Min => this.heartBeat10Min;

    public DynamicSettings DynamicSettings { get; private set; }

    public CommandLineSettings CommandLineSettings { get; private set; }

    public Visibility AppVisibility
    {
      get => this.appVisibility;
      set
      {
        Visibility appVisibility = this.appVisibility;
        if (appVisibility == value)
          return;
        this.appVisibility = value;
        this.OnPropertyChanged(nameof (AppVisibility), this.PropertyChanged, this.AppVisibilityChanged, (object) appVisibility, (object) value);
      }
    }

    public WindowState AppState
    {
      get => this.appState;
      set
      {
        if (this.appState == value)
          return;
        this.appState = value;
        this.OnPropertyChanged(nameof (AppState), this.PropertyChanged);
      }
    }

    public bool IsAppRestored => this.appVisibility == Visibility.Visible && this.appState == WindowState.Normal;

    public Command MinimizeAppCommand => this.minimizeAppCommand ?? (this.minimizeAppCommand = new Command());

    public Command HideAppCommand => this.hideAppCommand ?? (this.hideAppCommand = new Command());

    public Command ShowAppCommand => this.showAppCommand ?? (this.showAppCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowSettingsCommand => this.showSettingsCommand ?? (this.showSettingsCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowAboutCommand => this.showAboutCommand ?? (this.showAboutCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowProfileCommand => this.showProfileCommand ?? (this.showProfileCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowSyncCommand => this.showSyncCommand ?? (this.showSyncCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowBandCustomizationCommand => this.showBandCustomizationCommand ?? (this.showBandCustomizationCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command ShowTileManagementCommand => this.showTileManagementCommand ?? (this.showTileManagementCommand = new Command(new ExecuteHandler(this.ShowAppCommandExecute)));

    public Command CloseAppCommand => this.closeAppCommand ?? (this.closeAppCommand = new Command());

    public Command LogoutConfirmCommand => this.logoutConfirmCommand ?? (this.logoutConfirmCommand = new Command());

    public Command LogoutCommand => this.logoutCommand ?? (this.logoutCommand = new Command());

    public Command PairDeviceConfirmCommand => this.pairDeviceConfirmCommand ?? (this.pairDeviceConfirmCommand = new Command());

    public Command PairDeviceCommand => this.pairDeviceCommand ?? (this.pairDeviceCommand = new Command());

    public Command ForgetDeviceConfirmCommand => this.forgetDeviceConfirmCommand ?? (this.forgetDeviceConfirmCommand = new Command());

    public Command ForgetDeviceCommand => this.forgetDeviceCommand ?? (this.forgetDeviceCommand = new Command());

    public Command CheckForFirmwareUpdate => this.checkForFirmwareUpdate ?? (this.checkForFirmwareUpdate = new Command());

    public Command PushFirmwareToDevice => this.pushFirmwareToDevice ?? (this.pushFirmwareToDevice = new Command());

    public Command CheckOrOfferAppUpdate => this.checkOrOfferAppUpdate ?? (this.checkOrOfferAppUpdate = new Command(new ExecuteHandler(this.CheckOrOfferAppUpdate_OnExecute)));

    public Command LaunchAppUpdate => this.launchAppUpdate ?? (this.launchAppUpdate = new Command(new ExecuteHandler(this.LaunchAppUpdate_OnExecute)));

    public Command LaunchDownloadWeb => this.launchDownloadWeb ?? (this.launchDownloadWeb = new Command(new ExecuteHandler(this.LaunchDownloadWeb_OnExecute)));

    public Command LaunchTermsOfUseWeb => this.launchTermsOfUseWeb ?? (this.launchTermsOfUseWeb = new Command(new ExecuteHandler(this.LaunchTermsOfUseWeb_OnExecute)));

    public Command LaunchPrivacyPolicyWeb => this.launchPrivacyPolicyWeb ?? (this.launchPrivacyPolicyWeb = new Command(new ExecuteHandler(this.LaunchPrivacyPolicyWeb_OnExecute)));

    public Command LaunchSupportWeb => this.launchSupportWeb ?? (this.launchSupportWeb = new Command(new ExecuteHandler(this.LaunchSupportWeb_OnExecute)));

    public Command LaunchThirdPartyNoticesWeb => this.launchThirdPartyNoticesWeb ?? (this.launchThirdPartyNoticesWeb = new Command(new ExecuteHandler(this.LaunchThirdPartyNoticesWeb_OnExecute)));

    public Command UninstallApp => this.uninstallApp ?? (this.uninstallApp = new Command(new ExecuteHandler(this.UninstallApp_OnExecute)));

    public Command BeginEditUserProfile => this.beginEditUserProfile ?? (this.beginEditUserProfile = new Command(new ExecuteHandler(this.BeginEditUserProfile_OnExecute)));

    public Command EndEditUserProfile => this.endEditUserProfile ?? (this.endEditUserProfile = new Command(new ExecuteHandler(this.EndEditUserProfile_OnExecute)));

    public SecurityInfo SecurityInfo
    {
      get => this.securityInfo;
      set
      {
        if (this.securityInfo == null && value == null)
          return;
        this.securityInfo = value;
        this.OnPropertyChanged(nameof (SecurityInfo), this.PropertyChanged);
      }
    }

    public LoginInfo LoginInfo
    {
      get => this.loginInfo;
      set
      {
        LoginInfo loginInfo = this.LoginInfo;
        if (loginInfo == null && value == null)
          return;
        this.loginInfo = value;
        this.OnPropertyChanged(nameof (LoginInfo), this.PropertyChanged, this.LoginInfoChanged, (object) loginInfo, (object) value);
        this.OnPropertyChanged("DeviceCloudSyncEnabled", this.PropertyChanged);
        this.OnPropertyChanged("DeviceCloudSyncDisabled", this.PropertyChanged);
      }
    }

    public bool DebugPageEnabled
    {
      get => this.debugPageEnabled;
      set
      {
        if (this.debugPageEnabled == value)
          return;
        this.debugPageEnabled = value;
        this.OnPropertyChanged(nameof (DebugPageEnabled), this.PropertyChanged);
        if (this.debugPageEnabled)
          return;
        this.DebugEnabled = false;
      }
    }

    public bool DebugEnabled
    {
      get => this.debugEnabled;
      set
      {
        if (this.debugEnabled == value)
          return;
        this.debugEnabled = value;
        this.OnPropertyChanged(nameof (DebugEnabled), this.PropertyChanged);
      }
    }

    public LoginLogoutStatus LoginLogoutStatus
    {
      get => this.loginLogoutStatus;
      set
      {
        LoginLogoutStatus loginLogoutStatus = this.loginLogoutStatus;
        if (loginLogoutStatus == value)
          return;
        this.loginLogoutStatus = value;
        this.OnPropertyChanged(nameof (LoginLogoutStatus), this.PropertyChanged, this.LoginLogoutStatusChanged, (object) loginLogoutStatus, (object) value);
        this.OnPropertyChanged("LoginBusy", this.PropertyChanged);
      }
    }

    public bool LoginBusy => this.loginLogoutStatus == LoginLogoutStatus.GettingTokens;

    public UserDeviceStatus UserDeviceStatus
    {
      get => this.userDeviceStatus;
      set
      {
        UserDeviceStatus userDeviceStatus = this.userDeviceStatus;
        if (userDeviceStatus == value)
          return;
        this.userDeviceStatus = value;
        this.OnPropertyChanged(nameof (UserDeviceStatus), this.PropertyChanged, this.UserDeviceStatusChanged, (object) userDeviceStatus, (object) value);
      }
    }

    public LoginContext LoginContext { get; set; }

    public ErrorInfo LastLoginError
    {
      get => this.lastLoginError;
      set
      {
        if (this.lastLoginError == value)
          return;
        this.lastLoginError = value;
        this.OnPropertyChanged(nameof (LastLoginError), this.PropertyChanged);
      }
    }

    public LinkedList<ErrorInfo> ErrorLog => this.errorLog;

    public bool ProvideBogusAuthTokensToKDK
    {
      get => this.provideBogusAuthTokensToKDK;
      set
      {
        if (this.provideBogusAuthTokensToKDK == value)
          return;
        this.provideBogusAuthTokensToKDK = value;
        this.OnPropertyChanged(nameof (ProvideBogusAuthTokensToKDK), this.PropertyChanged);
      }
    }

    public bool ForceFirmwareUpdateCheck
    {
      get => this.forceFirmwareUpdateCheck;
      set
      {
        if (this.forceFirmwareUpdateCheck == value)
          return;
        this.forceFirmwareUpdateCheck = value;
        this.OnPropertyChanged(nameof (ForceFirmwareUpdateCheck), this.PropertyChanged);
      }
    }

    public KDeviceManager DeviceManager => this.deviceManager;

    public bool BatteryPlugVisible
    {
      get => this.batteryPlugVisible;
      set
      {
        if (this.batteryPlugVisible == value)
          return;
        this.batteryPlugVisible = value;
        this.OnPropertyChanged(nameof (BatteryPlugVisible), this.PropertyChanged);
      }
    }

    public bool AquiringDevice
    {
      get => this.aquiringDevice;
      set
      {
        if (this.aquiringDevice == value)
          return;
        this.aquiringDevice = value;
        this.OnPropertyChanged(nameof (AquiringDevice), this.PropertyChanged);
      }
    }

    public AppUpdateStatus AppUpdateStatus
    {
      get => this.appUpdateStatus;
      set
      {
        if (this.appUpdateStatus == value)
          return;
        this.appUpdateStatus = value;
        this.OnPropertyChanged(nameof (AppUpdateStatus), this.PropertyChanged);
      }
    }

    public AppUpdateCheckStatus AppUpdateCheckStatus
    {
      get => this.appUpdateCheckStatus;
      set
      {
        if (this.appUpdateCheckStatus == value)
          return;
        this.appUpdateCheckStatus = value;
        this.OnPropertyChanged(nameof (AppUpdateCheckStatus), this.PropertyChanged);
        this.OnPropertyChanged("LastAppUpdateCheckRelativeTime", this.PropertyChanged);
      }
    }

    public bool CheckingAppUpdate
    {
      get => this.checkingAppUpdate;
      set
      {
        if (this.checkingAppUpdate == value)
          return;
        this.checkingAppUpdate = value;
        this.OnPropertyChanged(nameof (CheckingAppUpdate), this.PropertyChanged);
      }
    }

    public TimeSpan? LastAppUpdateCheckRelativeTime => this.appUpdateCheckStatus != null ? new TimeSpan?(DateTime.UtcNow - this.appUpdateCheckStatus.LastChecked) : new TimeSpan?();

    public ErrorInfo LastAppUpdateCheckError
    {
      get => this.lastAppUpdateCheckError;
      set
      {
        ErrorInfo updateCheckError = this.lastAppUpdateCheckError;
        if (this.lastAppUpdateCheckError == value)
          return;
        this.lastAppUpdateCheckError = value;
        this.OnPropertyChanged(nameof (LastAppUpdateCheckError), this.PropertyChanged);
      }
    }

    public string SupportMailto => (string) null;

    public Command SendSupportEmail => (Command) null;

    public string GetSelectedDashboardUrl() => "https://dashboard.microsofthealth.com";

    public Command LaunchDashboardCommand => this.launchDashboardCommand ?? (this.launchDashboardCommand = new Command((ExecuteHandler) ((param, evetnArgs) =>
    {
      Process.Start(new ProcessStartInfo()
      {
        UseShellExecute = true,
        FileName = this.GetSelectedDashboardUrl()
      });
      Telemetry.LogEvent("Visual/Navigation/Dashboard", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
    })));

    public DeviceThemeManager ThemeManager { get; private set; }

    public StartStripManager StrapManager { get; private set; }

    private void heartBeatTimer01Sec_Tick(object sender, EventArgs e) => this.heartBeat01Sec.OnBeat();

    private void heartBeatTimer05Sec_Tick(object sender, EventArgs e) => this.heartBeat05Sec.OnBeat();

    private void heartBeatTimer10Min_Tick(object sender, EventArgs e) => this.heartBeat10Min.OnBeat();

    public void ShowAppCommandExecute(object parameter, EventArgs e) => this.AppVisibility = Visibility.Visible;

    public void CloseAppForceCommandExecute(object parameter, EventArgs e)
    {
      this.SaveInsightsData();
      this.App.Shutdown();
    }

    public void LogError(ErrorInfo error)
    {
      this.ErrorLog.AddLast(error);
      Exception exception = error.Exception;
    }

    public void LogLoginError(ErrorInfo error)
    {
      this.LogError(error);
      this.LastLoginError = error;
    }

    public void LogSyncError(ErrorInfo error)
    {
      this.LogError(error);
      this.DeviceManager.CurrentDevice.LastSyncError = error;
    }

    public void LogFWCheckError(ErrorInfo error)
    {
      this.LogError(error);
      if (this.DeviceManager.CurrentDevice == null)
        return;
      this.DeviceManager.CurrentDevice.LastFWCheckError = error;
    }

    public void LogProfileUpdateError(ErrorInfo error)
    {
      this.LogError(error);
      this.LoginInfo.LastProfileUpdateError = error;
    }

    public void LogDevicePairingError(ErrorInfo error)
    {
      this.LogError(error);
      this.LoginInfo.UserProfile.LastDevicePairingError = error;
    }

    public void LogDeviceConnectError(ErrorInfo error)
    {
      this.LogError(error);
      this.DeviceManager.LastDeviceConnectError = error;
    }

    private async void BeginEditUserProfile_OnExecute(object parameter, EventArgs args)
    {
      await this.RefreshUserProfile(true);
      this.LoginInfo.UserProfileEdit = new UserProfileEdit(this.LoginInfo.UserProfile, false, this.DynamicGlobalizationConfig.CurrentDynamicGlobalizationConfig.Oobe);
      if (!this.loginInfo.UserProfile.HasCompletedOOBE)
        await this.UpdateLoginInfoWithMsaUserProfile();
      this.LoginInfo.UserProfileEdit.Editing = true;
    }

    public async Task UpdateLoginInfoWithMsaUserProfile()
    {
      try
      {
        MsaUserProfile userProfileAsync = await new CloudCaller(this.LoginInfo.ServiceInfo).GetMsaUserProfileAsync(CancellationToken.None);
        if (userProfileAsync == null)
          return;
        DateTime? birthDate = userProfileAsync.BirthDate;
        this.LoginInfo.UserProfileEdit.Gender = userProfileAsync.Gender == 0 ? (Gender) 0 : (Gender) 1;
        this.LoginInfo.UserProfileEdit.ZipCode = userProfileAsync.ZipCode;
        if (string.IsNullOrWhiteSpace(this.LoginInfo.UserProfileEdit.FirstName))
          this.LoginInfo.UserProfileEdit.FirstName = userProfileAsync.FirstName.Length > 25 ? userProfileAsync.FirstName.Substring(0, 25) : userProfileAsync.FirstName;
        if (birthDate.HasValue && birthDate.Value > DateTime.MinValue)
        {
          UserProfileEdit userProfileEdit = this.LoginInfo.UserProfileEdit;
          DateTime dateTime = birthDate.Value;
          int year = dateTime.Year;
          dateTime = birthDate.Value;
          int month = dateTime.Month;
          DateTime? nullable = new DateTime?(new DateTime(year, month, 15));
          userProfileEdit.Birthdate = nullable;
        }
        if (string.IsNullOrWhiteSpace(this.LoginInfo.UserProfileEdit.FirstName) || !string.IsNullOrWhiteSpace(this.LoginInfo.UserProfileEdit.DeviceName))
          return;
        string str = string.Format(LStrings.OobeBandNameString, (object) this.LoginInfo.UserProfileEdit.FirstName);
        if (str.Length > 15)
          return;
        this.LoginInfo.UserProfileEdit.DeviceName = str;
      }
      catch (Exception ex)
      {
        this.LogError(new ErrorInfo(nameof (UpdateLoginInfoWithMsaUserProfile), "Couldn't retrive MsaUserProfile", ex));
      }
    }

    public async Task RefreshUserProfile(bool force)
    {
      if (this.loginInfo.UpdatingUserProfile || !force && !(this.loginInfo.UserProfile.Updated.Elapsed > TimeSpan.FromSeconds(5.0)))
        return;
      this.LoginInfo.LastProfileUpdateError = (ErrorInfo) null;
      using (new DisposableAction((Action) (() => this.loginInfo.UpdatingUserProfile = true), (Action) (() => this.loginInfo.UpdatingUserProfile = false)))
      {
        try
        {
          await this.LoadUserProfile();
        }
        catch (Exception ex)
        {
          this.LogProfileUpdateError(new ErrorInfo("LoadUserProfile", LStrings.Message_ProfileRefreshErrorOccurred, ex));
        }
      }
    }

    private async Task LoadUserProfile()
    {
      this.userProfileGetCancel = new CancellationTokenSource();
      try
      {
        IUserProfile profile = (IUserProfile) null;
        if (this.DeviceManager.CurrentDevice != null)
        {
          try
          {
            profile = await this.DeviceManager.CurrentDevice.CargoClient.GetUserProfileAsync(this.userProfileGetCancel.Token);
          }
          catch (OperationCanceledException ex)
          {
            return;
          }
          if (this.loginInfo.UserProfile == null)
          {
            this.loginInfo.UserProfile = new UserProfileSurrogate(profile, this);
            this.loginInfo.UserProfileEdit = new UserProfileEdit(this.loginInfo.UserProfile, false, this.DynamicGlobalizationConfig.CurrentDynamicGlobalizationConfig.Oobe);
          }
          else
            this.loginInfo.UserProfile.Source = profile;
        }
        else
        {
          using (ICargoClient cargoClient = await BandAdminClientManager.Instance.ConnectAsync(this.loginInfo.ServiceInfo))
          {
            try
            {
              profile = await cargoClient.GetUserProfileAsync(this.userProfileGetCancel.Token);
            }
            catch (OperationCanceledException ex)
            {
              return;
            }
            if (this.loginInfo.UserProfile == null)
            {
              this.loginInfo.UserProfile = new UserProfileSurrogate(profile, this);
              this.loginInfo.UserProfileEdit = new UserProfileEdit(this.loginInfo.UserProfile, false, this.DynamicGlobalizationConfig.CurrentDynamicGlobalizationConfig.Oobe);
            }
            else
              this.loginInfo.UserProfile.Source = profile;
          }
        }
        profile = (IUserProfile) null;
      }
      catch (Exception ex)
      {
        this.LogProfileUpdateError(new ErrorInfo(nameof (LoadUserProfile), LStrings.Message_ProfileRefreshErrorOccurred, ex));
        throw;
      }
    }

    private async void EndEditUserProfile_OnExecute(object parameter, EventArgs args)
    {
      if (this.userProfileGetCancel != null)
        this.userProfileGetCancel.Cancel();
      if (this.LoginInfo == null || this.LoginInfo.UserProfileEdit == null)
        return;
      this.LoginInfo.LastProfileUpdateError = (ErrorInfo) null;
      if ((bool) parameter)
      {
        if (!this.LoginInfo.UserProfileEdit.CanBeSaved)
          return;
        bool oldOobeComplete = this.loginInfo.UserProfile.HasCompletedOOBE;
        string oldBandName = this.loginInfo.UserProfile.DeviceName;
        this.loginInfo.UserProfileEdit.Save();
        this.loginInfo.UserProfile.Source.HasCompletedOOBE = true;
        try
        {
          await this.SaveUserProfile();
        }
        catch
        {
          this.loginInfo.UserProfile.Source.HasCompletedOOBE = oldOobeComplete;
          throw;
        }
        if (!oldOobeComplete)
        {
          this.loginInfo.UserProfile.HasCompletedOOBE = true;
          if (this.OOBECompleted != null)
            this.OOBECompleted((object) this, new EventArgs());
          ApplicationTelemetry.LogOobeComplete(this.loginInfo.UserProfile.Gender == 0, this.loginInfo.UserProfile.Birthdate.Value, this.loginInfo.UserProfile.PairedDeviceID != Guid.Empty, (OobePhoneMotionTrackingData) null);
        }
        else
          Telemetry.LogEvent("ProfileUpdated", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
        if (oldBandName != this.loginInfo.UserProfile.DeviceName)
          ApplicationTelemetry.LogBandRename(!oldOobeComplete);
        oldBandName = (string) null;
      }
      else
        this.loginInfo.UserProfileEdit.Editing = false;
    }

    private async Task SaveUserProfile()
    {
      this.loginInfo.UpdatingUserProfile = true;
      try
      {
        if (this.DeviceManager.CurrentDevice != null)
        {
          await this.DeviceManager.CurrentDevice.CargoClient.SaveUserProfileAsync(this.LoginInfo.UserProfile.Source, new DateTimeOffset?());
        }
        else
        {
          using (ICargoClient cargoClient = await BandAdminClientManager.Instance.ConnectAsync(Guid.Empty.ToString(), this.loginInfo.ServiceInfo))
            await cargoClient.SaveUserProfileAsync(this.LoginInfo.UserProfile.Source, new DateTimeOffset?());
        }
        this.LoginInfo.UserProfile.Saved();
      }
      catch (Exception ex)
      {
        this.LoginInfo.UserProfileEdit.Restore();
        this.LogProfileUpdateError(new ErrorInfo(nameof (SaveUserProfile), LStrings.Message_ProfileSaveErrorOccurred, ex));
        return;
      }
      finally
      {
        this.LoginInfo.UpdatingUserProfile = false;
      }
      this.LoginInfo.UserProfileEdit.Editing = false;
    }

    private async void appUpdateCheckTimer_Tick(object sender, EventArgs e)
    {
      if (!await this.CheckAppUpdate())
        return;
      this.App.MainWindow.ShowOfferAppUpdate();
    }

    private async void CheckOrOfferAppUpdate_OnExecute(object parameter, EventArgs args)
    {
      if (this.AppUpdateStatus != AppUpdateStatus.Available)
      {
        int num = await this.CheckAppUpdate() ? 1 : 0;
      }
      if (this.AppUpdateStatus != AppUpdateStatus.Available)
        return;
      this.App.MainWindow.ShowOfferAppUpdate();
    }

    private async Task<bool> CheckAppUpdate()
    {
      if (this.CheckingAppUpdate)
        return false;
      this.appUpdateCheckTimer.Stop();
      this.CheckingAppUpdate = true;
      this.AppUpdateStatus = AppUpdateStatus.Checking;
      try
      {
        string downloadURL = await DownloadCenter.CheckUpdates(Globals.ApplicationVersion);
        if (Globals.ApplicationVersion == new Version(1, 0, 0, 0))
          downloadURL = "";
        this.AppUpdateStatus = downloadURL != "" ? AppUpdateStatus.Available : AppUpdateStatus.UpToDate;
        this.AppUpdateCheckStatus = new AppUpdateCheckStatus(downloadURL, DateTime.UtcNow);
      }
      catch
      {
        this.AppUpdateStatus = AppUpdateStatus.Unknown;
      }
      finally
      {
        this.CheckingAppUpdate = false;
        this.appUpdateCheckTimer.Interval = Globals.AppCheckUpdateSchedule;
        this.appUpdateCheckTimer.Start();
      }
      return this.AppUpdateStatus == AppUpdateStatus.Available;
    }

    public void LaunchAppUpdate_OnExecute(object parameter, EventArgs args) => Process.Start(new ProcessStartInfo()
    {
      FileName = this.AppUpdateCheckStatus.DownloadURL,
      UseShellExecute = true
    });

    private void LaunchTermsOfUseWeb_OnExecute(object parameter, EventArgs args) => Process.Start(new ProcessStartInfo()
    {
      FileName = "https://go.microsoft.com/fwlink/?LinkID=507589",
      UseShellExecute = true
    });

    private void LaunchPrivacyPolicyWeb_OnExecute(object parameter, EventArgs args) => Process.Start(new ProcessStartInfo()
    {
      FileName = "https://go.microsoft.com/fwlink/?LinkID=521839",
      UseShellExecute = true
    });

    private void LaunchDownloadWeb_OnExecute(object parameter, EventArgs args)
    {
      Telemetry.LogEvent("InstallUWPLink", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
      Process.Start(new ProcessStartInfo()
      {
        FileName = "https://www.microsoft.com/en-us/store/apps/microsoft-health/9wzdncrfjbcx",
        UseShellExecute = true
      });
    }

    private void LaunchSupportWeb_OnExecute(object parameter, EventArgs args) => Process.Start(new ProcessStartInfo()
    {
      FileName = "https://go.microsoft.com/fwlink/?LinkID=506763",
      UseShellExecute = true
    });

    private void LaunchThirdPartyNoticesWeb_OnExecute(object parameter, EventArgs args) => Process.Start(new ProcessStartInfo()
    {
      FileName = "https://go.microsoft.com/fwlink/?LinkID=513024",
      UseShellExecute = true
    });

    private void UninstallApp_OnExecute(object parameter, EventArgs args)
    {
      try
      {
        Telemetry.LogEvent("UninstallSyncClientLink", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
        string mainWindowTitle = Process.GetCurrentProcess().MainWindowTitle;
        string uninstallCommand = this.GetUninstallCommand("Microsoft Band");
        if (string.IsNullOrEmpty(uninstallCommand))
          return;
        new Process()
        {
          StartInfo = {
            FileName = uninstallCommand.Substring(0, uninstallCommand.IndexOf(" ")),
            Arguments = uninstallCommand.Substring(uninstallCommand.IndexOf(" "))
          }
        }.Start();
      }
      catch (Exception ex)
      {
        this.LogError(new ErrorInfo(nameof (UninstallApp_OnExecute), "Unable to uninstall app programmatically", ex));
      }
    }

    private string GetUninstallCommand(string productDisplayName)
    {
      RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
        if (registryKey2 != null)
        {
          string str = (string) registryKey2.GetValue("DisplayName");
          if (str != null && str.Contains(productDisplayName))
            return (string) registryKey2.GetValue("UninstallString");
        }
      }
      return "";
    }
  }
}
