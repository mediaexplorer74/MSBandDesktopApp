// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.AppMainWindow
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace DesktopSyncApp
{
    // sealed ?
  public sealed partial class AppMainWindow : 
    Window,
    INotifyPropertyChanged,
    IDisposable,
    IComponentConnector
  {
    private ViewModel model;
    private TrayIcon trayIcon;
    private string baseTitle;
    private DispatcherTimer reSyncTimer;
    private DesktopSyncApp.App app;
    private MainWindowControl mainWindowPage;
    private LoginControl loginPage;
    private DisconnectedControl disconnectedPage;
    private CantPairDeviceControl cantPairDevicePage;
    private PairDeviceControl pairDevicePage;
    private ProfileControl profilePage;
    private SyncControl syncPage;
    private FirmwareUpdateControl firmwareUpdatePage;
    private FirmwareUpdateProgressControl firmwareUpdateProgressPage;
    private SettingsControl settingsPage;
    private AboutControl aboutPage;
    private PairDeviceConfirmControl pairDeviceConfirmPage;
    private ForgetDeviceConfirmControl forgetDeviceConfirmPage;
    private LaunchSoftwareUpdateControl launchSoftwareUpdatePage;
    private ThemeColorPickerControl themeColorPickerPage;
    private TileManagementControl tileManagementPage;
    private ErrorMessageControl errorMessagePage;
    private UpsellPageControl upsellPage;
    private DispatcherTimer firmwareCheckTimer;
    private AutoResetEvent loginLogoutCompleteEvent;
    private DispatcherTimer deviceScanDelayTimer;
    internal AppMainWindow MainWindow;
    internal AnimatedPageControl MainWindowPageManager;
    internal AnimatedPageControl SuperModalPageManager;
    
    //private bool _contentLoaded;

    public void WM_DEVICECHANGE()
    {
      this.deviceScanDelayTimer.Stop();
      if (this.model.DeviceManager.CurrentDevice == null)
        this.deviceScanDelayTimer.Start();
      else
        this.model.DeviceManager.ScanKDevices();
    }

    private void deviceScanDelayTimer_Tick(object sender, EventArgs e)
    {
      this.deviceScanDelayTimer.Stop();
      this.model.DeviceManager.ScanKDevices();
    }

    private async void DeviceManager_DevicesChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice != null || this.model.LoginLogoutStatus != LoginLogoutStatus.LoggedIn || !this.model.LoginInfo.UserProfile.HasCompletedOOBE)
        return;
      await this.ConnectToCorrectDevice(false);
    }

    public async Task ConnectToCorrectDevice(bool pairDevice)
    {
      if (this.model.DeviceManager.Count == 1)
        await this.ConnectToDevice(this.model.DeviceManager.GetSingleDevice(), pairDevice);
      else
        this.model.UserDeviceStatus = this.model.DeviceManager.Count <= 1 ? UserDeviceStatus.None : UserDeviceStatus.Multiple;
    }

    private void DeviceManager_CurrentDeviceChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice != null)
        return;
      this.model.UserDeviceStatus = UserDeviceStatus.None;
    }

    public async Task ConnectToDevice(KDeviceInfo deviceInfo, bool pairDevice)
    {
      this.model.DeviceManager.LastDeviceConnectError = (ErrorInfo) null;
      using (new DisposableAction((Action) (() => this.model.AquiringDevice = true), (Action) (() => this.model.AquiringDevice = false)))
      {
        try
        {
          await this.model.RefreshUserProfile(false);
          await this.model.DeviceManager.UpdateDeviceInfo(deviceInfo, this.model.LoginInfo, false, false);
          if ((int)deviceInfo.AppType != 2 && (int)deviceInfo.AppType != 3)
            this.model.UserDeviceStatus = UserDeviceStatus.None;
          else if ((int)deviceInfo.AppType == 2)
          {
            if (deviceInfo.FirmwareCheckStatus != null && deviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable)
            {
              int num = await this.model.DeviceManager.OpenDevice(deviceInfo, this.model.LoginInfo) ? 1 : 0;
              this.model.UserDeviceStatus = UserDeviceStatus.RegisteredRequiresFW;
            }
            else
              this.model.UserDeviceStatus = UserDeviceStatus.None;
          }
          else if (pairDevice || deviceInfo.DeviceProfileStatus.UserLinkStatus == 1 && (deviceInfo.DeviceProfileStatus.DeviceLinkStatus == 1 || deviceInfo.DeviceProfileStatus.DeviceLinkStatus == null && !deviceInfo.IsOOBEComplete))
          {
            int num = await this.model.DeviceManager.OpenDevice(deviceInfo, this.model.LoginInfo) ? 1 : 0;
            this.model.LoginInfo.UserProfile.PairedDeviceIDUpdated();
            if (deviceInfo.FirmwareCheckStatus != null && deviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable)
            {
              this.model.UserDeviceStatus = UserDeviceStatus.RegisteredRequiresFW;
            }
            else
            {
              this.model.UserDeviceStatus = UserDeviceStatus.Registered;
              this.model.DeviceManager.CurrentDevice.FirmwareStatusChanged += new PropertyChangedEventHandler(this.CurrentDevice_FirmwareStatusChanged);
              this.SetFirmwareUpdateTimer();
              this.DoDeviceConnectedTasks(!deviceInfo.IsOOBEComplete);
            }
          }
          else
            this.model.UserDeviceStatus = deviceInfo.DeviceProfileStatus.UserLinkStatus != null ? (!deviceInfo.IsOOBEComplete ? UserDeviceStatus.CantRegisterUnregister : UserDeviceStatus.CantRegisterUnregisterReset) : (!deviceInfo.IsOOBEComplete ? UserDeviceStatus.CanRegister : UserDeviceStatus.CantRegisterReset);
        }
        catch (Exception ex)
        {
          this.model.LogDeviceConnectError(new ErrorInfo(nameof (ConnectToDevice), Strings.Message_DeviceConnectErrorOccurred, ex));
          this.model.UserDeviceStatus = UserDeviceStatus.None;
        }
      }
    }

    private void CurrentDevice_FirmwareStatusChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.model.DeviceManager.CurrentDevice.FirmwareStatus != FirmwareStatus.ReadyToUpdate)
        return;
      this.model.UserDeviceStatus = UserDeviceStatus.RegisteredRequiresFW;
    }

    public async Task BatteryStreamSubscribe()
    {
      try
      {
        await this.model.DeviceManager.CurrentDevice.CargoClient.SensorSubscribeAsync((SensorType) 38);
      }
      catch (Exception ex)
      {
        this.model.LogError(new ErrorInfo("BatteryStreamSubscribeContinuation", "Sensor log sync task failed", ex));
      }
    }

    public async Task SyncDeviceToCloud()
    {
      if (this.model.DeviceManager.CurrentDevice.Syncing)
        return;
      this.model.DeviceManager.CurrentDevice.LastSyncError = (ErrorInfo) null;
      using (new DisposableAction((Action) (() =>
      {
        this.model.DeviceManager.CurrentDevice.SyncProgress = new DeviceSyncProgress();
        this.model.DeviceManager.CurrentDevice.Syncing = true;
      }), (Action) (() =>
      {
        if (this.model.DeviceManager.CurrentDevice == null)
          return;
        this.model.DeviceManager.CurrentDevice.Syncing = false;
      })))
      {
        try
        {
          if (!this.model.DeviceManager.CurrentDevice.DeviceInfo.IsOOBEComplete)
          {
            KDeviceInfo kdeviceInfo = this.model.DeviceManager.CurrentDevice.DeviceInfo;
            int num = await this.model.DeviceManager.CurrentDevice.CargoClient.GetDeviceOobeCompletedAsync() ? 1 : 0;
            kdeviceInfo.IsOOBEComplete = num != 0;
            kdeviceInfo = (KDeviceInfo) null;
          }
        }
        catch
        {
        }
        await this.SyncDeviceToCloudSafe();
      }
    }

    public async Task SyncDeviceToCloudSafe()
    {
      this.reSyncTimer.Stop();
      try
      {
        await this.model.RefreshUserProfile(false);
        KDeviceInfo kdeviceInfo = this.model.DeviceManager.CurrentDevice.DeviceInfo;
        DeviceProfileStatus profileLinkStatusAsync = await this.model.DeviceManager.CurrentDevice.CargoClient.GetDeviceAndProfileLinkStatusAsync(this.model.LoginInfo.UserProfile.Source);
        kdeviceInfo.DeviceProfileStatus = profileLinkStatusAsync;
        kdeviceInfo = (KDeviceInfo) null;
        if (this.model.DeviceManager.CurrentDevice.DeviceInfo.DeviceProfileStatus.UserLinkStatus == 1)
        {
          if (this.model.DeviceManager.CurrentDevice.DeviceInfo.DeviceProfileStatus.DeviceLinkStatus == 1)
            goto label_7;
        }
        this.model.DeviceManager.CloseDevice();
        this.ConnectToCorrectDevice(false).GetAwaiter();
        return;
      }
      catch
      {
      }
label_7:
      if (!this.model.DeviceManager.CurrentDevice.DeviceInfo.IsOOBEComplete)
      {
        try
        {
          await this.model.DeviceManager.CurrentDevice.CargoClient.NavigateToScreenAsync((CargoScreen) 22);
        }
        catch
        {
        }
      }
      this.model.DeviceManager.CurrentDevice.SyncCancel = new CancellationTokenSource();
      bool finished = false;
      try
      {
        SyncResult cloudAsync = await this.model.DeviceManager.CurrentDevice.CargoClient.ObsoleteSyncDeviceToCloudAsync(this.model.DeviceManager.CurrentDevice.SyncCancel.Token, (IProgress<SyncProgress>) this.model.DeviceManager.CurrentDevice.SyncProgress, false);
        finished = true;
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        this.model.LogSyncError(new ErrorInfo("SensorLogSyncContinuation", "Sensor log sync task failed", ex));
      }
      if (this.model.DeviceManager.CurrentDevice == null)
        return;
      if (finished)
      {
        this.model.DeviceManager.CurrentDevice.LastLogSyncTime = new DateTime?(DateTime.UtcNow);
        this.model.DynamicSettings.LastLogSync = this.model.DeviceManager.CurrentDevice.LastLogSyncTime;
        Telemetry.LogEvent("Utilities/Sync", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
      }
      if (this.model.DynamicSettings.AutoSync)
        this.reSyncTimer.Start();
      await this.model.DeviceManager.CurrentDevice.GetInfoFromDevice();
    }

    public void CancelSensorLogSync()
    {
      this.model.DeviceManager.CurrentDevice.SyncCancel.Cancel();
      this.model.DeviceManager.CurrentDevice.SyncCancel.Dispose();
      this.model.DeviceManager.CurrentDevice.SyncCancel = (CancellationTokenSource) null;
    }

    public async void DoDeviceConnectedTasks(bool forceSync)
    {
      await this.model.DeviceManager.CurrentDevice.GetInfoFromDevice();
      await this.BatteryStreamSubscribe();
      if (!(this.model.DynamicSettings.AutoSync | forceSync))
        return;
      await this.SyncDeviceToCloud();
    }

    public async Task CheckForFirmwareUpdate()
    {
      if (this.model.DeviceManager.CurrentDevice.CheckingFirmware)
        return;
      this.model.DeviceManager.CurrentDevice.LastFWCheckError = (ErrorInfo) null;
      this.model.DeviceManager.CurrentDevice.FirmwareStatus |= FirmwareStatus.Checking;
      this.model.DeviceManager.CurrentDevice.CheckingFirmware = true;
      try
      {
        await this.model.DeviceManager.UpdateDeviceInfo(this.model.DeviceManager.CurrentDevice.DeviceInfo, this.model.LoginInfo, true, true);
      }
      catch (Exception ex)
      {
        this.model.DeviceManager.CurrentDevice.FirmwareStatus ^= FirmwareStatus.Checking;
        this.model.DeviceManager.CurrentDevice.CheckingFirmware = false;
        this.model.LogFWCheckError(new ErrorInfo("GetCloudFirmwareStatusContinuation", "Firmware version info task failed", ex));
        this.SetFirmwareUpdateTimer();
        return;
      }
      if (!this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable)
      {
        this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.UpToDate;
        this.model.DeviceManager.CurrentDevice.CheckingFirmware = false;
        this.SetFirmwareUpdateTimer();
      }
      else
      {
        this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.Downloading;
        ITimedTelemetryEvent stateTimedEvent = (ITimedTelemetryEvent) null;
        try
        {
          stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateDownload(false);
          await this.model.DeviceManager.CurrentDevice.CargoClient.DownloadFirmwareUpdateAsync(this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info);
          stateTimedEvent.End();
          ((IDisposable) stateTimedEvent).Dispose();
        }
        catch (Exception ex)
        {
          this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.Unknown;
          this.model.DeviceManager.CurrentDevice.CheckingFirmware = false;
          this.model.LogFWCheckError(new ErrorInfo("GetCloudFirmwareStatusContinuation", "Firmware download task failed", ex));
          this.SetFirmwareUpdateTimer();
          stateTimedEvent.Cancel();
          return;
        }
        this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.ReadyToUpdate;
        this.model.DeviceManager.CurrentDevice.CheckingFirmware = false;
        this.model.UserDeviceStatus = UserDeviceStatus.RegisteredRequiresFW;
      }
    }

    public async Task PushFirmwareUpdate()
    {
      IFirmwareUpdateInfo info = this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info;
      bool success = true;
      this.model.UserDeviceStatus = UserDeviceStatus.RegisteredFWUpdating;
      this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.Updating;
      this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus = (FirmwareCheckStatus) null;
      this.model.DeviceManager.CurrentDevice.FirmwareUpdateProgress = new FirmwareUpdateProgress();
      bool TwoUpFix = this.model.DeviceManager.CurrentDevice.DeviceInfo.AppType == 2;
      try
      {
        using (new DeviceScanSuppressor(this.model.DeviceManager))
        {
          int num = await this.model.DeviceManager.CurrentDevice.CargoClient.UpdateFirmwareAsync(info, CancellationToken.None, (IProgress<FirmwareUpdateProgress>) this.model.DeviceManager.CurrentDevice.FirmwareUpdateProgress) ? 1 : 0;
        }
        Telemetry.LogEvent("Utilities/FirmwareUpdate/Send to band", (IDictionary<string, string>) null, (IDictionary<string, double>) null);
      }
      catch (Exception ex)
      {
        success = false;
        this.model.LogError(new ErrorInfo("PushLatestFirmwareContinuation", "Firmware update task failed", ex));
        this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.Unknown;
      }
      if (success)
      {
        try
        {
          await this.model.DeviceManager.UpdateDeviceInfo(this.model.DeviceManager.CurrentDevice.DeviceInfo, this.model.LoginInfo, true, true);
        }
        catch (Exception ex)
        {
          success = false;
          this.model.LogError(new ErrorInfo("PushLatestFirmwareContinuation", "Firmware update task failed", ex));
        }
      }
      DeviceProfileStatus deviceProfileStatus = (DeviceProfileStatus) null;
      if (success && !this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable && this.model.DeviceManager.CurrentDevice.DeviceInfo.AppType == 3)
      {
        this.model.DeviceManager.CurrentDevice.FirmwareStatus = FirmwareStatus.UpToDate;
        deviceProfileStatus = await this.model.DeviceManager.CurrentDevice.CargoClient.GetDeviceAndProfileLinkStatusAsync(this.model.LoginInfo.UserProfile.Source);
      }
      if (success && deviceProfileStatus != null && deviceProfileStatus.DeviceLinkStatus == 1 && deviceProfileStatus.UserLinkStatus == 1)
      {
        this.model.DeviceManager.CurrentDevice.CargoClient.UserAgent = KDeviceManager.GetUserAgent(this.model.DeviceManager.CurrentDevice.CargoClient);
        this.model.UserDeviceStatus = UserDeviceStatus.Registered;
        this.SetFirmwareUpdateTimer();
        this.DoDeviceConnectedTasks(true);
      }
      else
      {
        this.model.DeviceManager.CloseDevice();
        this.model.UserDeviceStatus = UserDeviceStatus.None;
        this.model.DeviceManager.ScanKDevices();
      }
      if (!success)
        return;
      Telemetry.LogEvent("Utilities/FirmwareUpdate/Apply Update", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "2UP",
          TwoUpFix.ToString()
        }
      }, (IDictionary<string, double>) null);
    }

    public string GetSelectedSecurityEnvironment() => "prodkds.dns-cargo.com";

    public async void AuthenticateSafe(LoginContext loginContext)
    {
      if (this.model.LoginLogoutStatus != LoginLogoutStatus.LoggedOut)
        return;
      if (this.model.SecurityInfo == null)
        this.model.SecurityInfo = new SecurityInfo(this.GetSelectedSecurityEnvironment());
      try
      {
        if (!await this.Authenticate(loginContext))
          this.model.LoginLogoutStatus = LoginLogoutStatus.LoggedOut;
      }
      catch (Exception ex)
      {
        this.model.LogLoginError(new ErrorInfo("Login", "Login failed", ex));
        this.model.LoginLogoutStatus = LoginLogoutStatus.LoggedOut;
      }
      if (this.model.LoginLogoutStatus == LoginLogoutStatus.LoggedOut)
      {
        this.model.TelemetryListener.SetOdsUserId(Guid.Empty);
        if (this.model.SecurityInfo == null)
          return;
        this.model.SecurityInfo = (SecurityInfo) null;
        this.DeletePersistedLoginInformation();
      }
      else
      {
        this.model.TelemetryListener.SetOdsUserId(this.model.LoginInfo.UserProfile.UserID);
        if (this.model.LoginInfo.UserProfile.HasCompletedOOBE)
        {
          await this.ConnectToCorrectDevice(false);
        }
        else
        {
          this.ShowProfile();
          this.model.BeginEditUserProfile.Execute((object) null);
        }
      }
    }

    public async Task<bool> Authenticate(LoginContext loginContext)
    {
      this.model.LoginContext = loginContext;
      this.model.LastLoginError = (ErrorInfo) null;
      DateTime utcNow = DateTime.UtcNow;
      if (this.model.SecurityInfo.AccessToken != null && this.model.SecurityInfo.RefreshToken != null && this.model.SecurityInfo.KATToken != null)
      {
        DateTime? nullable = this.model.SecurityInfo.RefreshTokenExpires;
        DateTime dateTime1 = utcNow.AddHours(1.0);
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime1 ? 1 : 0) : 0) == 0)
        {
          nullable = this.model.SecurityInfo.AccessTokenExpires;
          DateTime dateTime2 = utcNow.AddHours(1.0);
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < dateTime2 ? 1 : 0) : 0) != 0)
          {
            this.model.LoginLogoutStatus = LoginLogoutStatus.GettingTokens;
            await this.GetTokenRefresh();
            this.SaveLoginInformation();
            goto label_9;
          }
          else
          {
            this.model.LoginLogoutStatus = LoginLogoutStatus.GettingTokens;
            goto label_9;
          }
        }
      }
      if (loginContext != LoginContext.UserInitiated || !this.Login())
        return false;
      this.model.LoginLogoutStatus = LoginLogoutStatus.GettingTokens;
      this.SaveLoginInformation();
label_9:
      await this.GetServiceInfo();
      this.SaveLoginInformation();
      this.model.LoginLogoutStatus = LoginLogoutStatus.LoggedIn;
      if (this.model.HideAfterLogin)
      {
        this.model.HideAfterLogin = false;
        if (this.model.LoginInfo.UserProfile.HasCompletedOOBE)
          this.model.AppVisibility = Visibility.Collapsed;
      }
      await this.model.DynamicGlobalizationConfig.UpdateDynamicConfiguration(this.model.LoginInfo.ServiceInfo);
      return true;
    }

    private bool Login()
    {
      if (this.model.SecurityInfo == null)
        this.model.SecurityInfo = new SecurityInfo(this.GetSelectedSecurityEnvironment());
      return new LoginDialog((Window) this, this.model).ShowLogin(this.model.SecurityInfo);
    }

    private async Task GetTokenRefresh()
    {
      byte[] json = (byte[]) null;
      using (WebClient client = new WebClient())
      {
        Uri tokenRefreshUri = this.model.SecurityInfo.CreateTokenRefreshUri();
        client.Headers.Add("User-Agent", Globals.DefaultUserAgent);
        json = await client.DownloadDataTaskAsync(tokenRefreshUri);
        WebUtil.VerifyContentTypeIsJson(client.ResponseHeaders);
      }
      this.model.SecurityInfo.Update(TokenRefreshResponse.DeserializeJsonObject(json));
    }

    public async Task GetServiceInfo()
    {
      string str = (string) null;
      byte[] json = (byte[]) null;
      bool userIsNew = false;
      using (WebClient client = new WebClient())
      {
        Uri uri = new Uri(string.Format("https://{0}/api/v1/user", (object) this.model.SecurityInfo.SecurityEnvironment));
        client.Headers.Add("User-Agent", Globals.DefaultUserAgent);
        client.Headers.Add("Authorization", this.model.SecurityInfo.AccessToken);
        try
        {
          json = await client.DownloadDataTaskAsync(uri);
        }
        catch (WebException ex)
        {
          if (ex.Response != null && ((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.NotFound)
            userIsNew = true;
          else
            throw;
        }
        if (userIsNew)
        {
          byte[] data = Encoding.UTF8.GetBytes("{}");
          client.Headers.Add("Content-Type", "application/json");
          json = await Task.Run<byte[]>((Func<byte[]>) (() => client.UploadData(uri, "POST", data)));
        }
        WebUtil.VerifyContentTypeIsJson(client.ResponseHeaders);
        str = client.ResponseHeaders["Authorization"];
      }
      if (!str.StartsWith("WRAP access_token=\"") || !str.EndsWith("\""))
        throw new Exception("HTTP Authorization header invalid");
      UserInfo userInfo;
      try
      {
        userInfo = UserInfo.DeserializeJson(json);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to deserialize Json response from KDS security server", ex);
      }
      this.model.SecurityInfo.Update(userInfo.EndPoint, str.Substring(19, str.Length - 19 - 1));
      LoginInfo loginInfo1 = new LoginInfo()
      {
        ServiceInfo = new ServiceInfo()
        {
          AccessToken = this.model.SecurityInfo.KATToken,
          DiscoveryServiceAccessToken = this.model.SecurityInfo.AccessToken,
          DiscoveryServiceAddress = this.model.SecurityInfo.SecurityEnvironment,
          PodAddress = userInfo.EndPoint,
          FileUpdateServiceAddress = userInfo.FUSEndPoint,
          UserId = userInfo.UserID,
          UserAgent = Globals.DefaultUserAgent
        }
      };
      using (ICargoClient cargoClient = await BandAdminClientManager.Instance.ConnectAsync(loginInfo1.ServiceInfo))
      {
        LoginInfo loginInfo2 = loginInfo1;
        IUserProfile userProfileAsync = await cargoClient.GetUserProfileAsync();
        ViewModel model = this.model;
        loginInfo2.UserProfile = new UserProfileSurrogate(userProfileAsync, model);
        loginInfo2 = (LoginInfo) null;
        loginInfo1.UserProfileEdit = new UserProfileEdit(loginInfo1.UserProfile, false, this.model.DynamicGlobalizationConfig.CurrentDynamicGlobalizationConfig.Oobe);
      }
      this.model.LoginInfo = loginInfo1;
    }

    public bool LogOut()
    {
      this.model.LastLoginError = (ErrorInfo) null;
      try
      {
        if (!new LoginDialog((Window) this, this.model).ShowLogout(this.model.SecurityInfo))
          return false;
        if (this.model.DeviceManager.CurrentDevice != null)
        {
          if (this.model.DeviceManager.CurrentDevice.SyncCancel != null)
            this.CancelSensorLogSync();
        }
      }
      catch (Exception ex)
      {
        this.model.LogLoginError(new ErrorInfo("Logout", "Logout failed", ex));
        return false;
      }
      this.model.LoginInfo = (LoginInfo) null;
      this.model.SecurityInfo = (SecurityInfo) null;
      this.model.DeviceManager.CloseDevice();
      this.model.LoginLogoutStatus = LoginLogoutStatus.LoggedOut;
      this.model.TelemetryListener.SetOdsUserId(Guid.Empty);
      this.DeletePersistedLoginInformation();
      return true;
    }

    public void LoadLoginInformation()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Globals.RegistrySoftwareAppRootPath))
          this.model.SecurityInfo = SecurityInfo.DeserializeJsonObject(Convert.FromBase64String(registryKey.GetValue("SecurityInfo") as string));
      }
      catch
      {
      }
    }

    public void SaveLoginInformation()
    {
      try
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(Globals.RegistrySoftwareAppRootPath))
          subKey.SetValue("SecurityInfo", (object) Convert.ToBase64String(this.model.SecurityInfo.SerializeJsonObject(), Base64FormattingOptions.InsertLineBreaks));
      }
      catch
      {
      }
    }

    public void DeletePersistedLoginInformation()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Globals.RegistrySoftwareAppRootPath, true))
          registryKey.DeleteValue("SecurityInfo", false);
      }
      catch (Exception ex)
      {
        this.model.LogError(new ErrorInfo(nameof (DeletePersistedLoginInformation), "Failed to delete login information from registry", ex));
      }
    }

    private void Heartbeat10Min_Beat(object sender, EventArgs e) => this.CheckTokens().ConfigureAwait(false);

    private async Task CheckTokens()
    {
      if (this.model.LoginLogoutStatus != LoginLogoutStatus.LoggedIn)
        return;
      DateTime? katTokenExpires = this.model.SecurityInfo.KATTokenExpires;
      DateTime dateTime = DateTime.UtcNow.AddHours(1.0);
      if ((katTokenExpires.HasValue ? (katTokenExpires.GetValueOrDefault() < dateTime ? 1 : 0) : 0) == 0)
        return;
      try
      {
        await this.GetTokenRefresh();
        this.SaveLoginInformation();
        await this.GetServiceInfo();
        this.SaveLoginInformation();
      }
      catch
      {
      }
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      (PresentationSource.FromVisual((Visual) this) as HwndSource).AddHook(new HwndSourceHook(this.WndProc));
    }

    private IntPtr WndProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      switch (msg)
      {
        case 30:
          this.WM_TIMECHANGE();
          break;
        case 537:
          this.WM_DEVICECHANGE();
          break;
      }
      return IntPtr.Zero;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public AppMainWindow(DesktopSyncApp.App app, ViewModel model)
    {
      this.app = app;
      this.model = model;
      this.DataContext = (object) model;
      CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
      CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;
      this.mainWindowPage = new MainWindowControl(this);
      this.loginPage = new LoginControl(this);
      this.disconnectedPage = new DisconnectedControl(this);
      this.cantPairDevicePage = new CantPairDeviceControl(this);
      this.pairDevicePage = new PairDeviceControl(this);
      this.syncPage = new SyncControl(this);
      this.profilePage = new ProfileControl(this);
      this.firmwareUpdatePage = new FirmwareUpdateControl(this);
      this.firmwareUpdateProgressPage = new FirmwareUpdateProgressControl(this);
      this.settingsPage = new SettingsControl(this);
      this.aboutPage = new AboutControl(this);
      this.pairDeviceConfirmPage = new PairDeviceConfirmControl(this);
      this.forgetDeviceConfirmPage = new ForgetDeviceConfirmControl(this);
      this.launchSoftwareUpdatePage = new LaunchSoftwareUpdateControl(this);
      this.themeColorPickerPage = new ThemeColorPickerControl(this);
      this.tileManagementPage = new TileManagementControl(this);
      this.errorMessagePage = new ErrorMessageControl(this);
      this.upsellPage = new UpsellPageControl(this);
      model.MinimizeAppCommand.OnExecute += new ExecuteHandler(this.MinimizeAppCommandExecute);
      model.HideAppCommand.OnExecute += new ExecuteHandler(this.HideAppCommandExecute);
      model.ShowAppCommand.OnExecute += new ExecuteHandler(this.ShowAppCommandExecute);
      model.ShowSettingsCommand.OnExecute += new ExecuteHandler(this.ShowSettingsCommandExecute);
      model.ShowAboutCommand.OnExecute += new ExecuteHandler(this.ShowAboutCommandExecute);
      model.ShowProfileCommand.OnExecute += new ExecuteHandler(this.ShowProfileCommandExecute);
      model.ShowSyncCommand.OnExecute += new ExecuteHandler(this.ShowSyncCommandExecute);
      model.ShowTileManagementCommand.OnExecute += new ExecuteHandler(this.ShowTileManagementCommandExecute);
      model.ShowBandCustomizationCommand.OnExecute += new ExecuteHandler(this.ShowBandCustomizationCommandExecute);
      model.CloseAppCommand.OnExecute += new ExecuteHandler(this.CloseAppCommandExecute);
      model.LogoutCommand.OnExecute += new ExecuteHandler(this.LogoutCommand_OnExecute);
      model.PairDeviceConfirmCommand.OnExecute += new ExecuteHandler(this.PairDeviceConfirmCommand_OnExecute);
      model.PairDeviceCommand.OnExecute += new ExecuteHandler(this.PairDeviceCommand_OnExecute);
      model.ForgetDeviceConfirmCommand.OnExecute += new ExecuteHandler(this.ForgetDeviceConfirmCommand_OnExecute);
      model.ForgetDeviceCommand.OnExecute += new ExecuteHandler(this.ForgetDeviceCommand_OnExecute);
      model.CheckForFirmwareUpdate.OnExecute += new ExecuteHandler(this.CheckForFirmwareUpdate);
      model.PushFirmwareToDevice.OnExecute += new ExecuteHandler(this.PushFirmwareUpdate);
      this.InitializeComponent();
      this.baseTitle = this.Title;
      this.SetStartupScreenPosition();
      this.trayIcon = new TrayIcon(this);
      model.DynamicSettings.AutoSyncChanged += new PropertyValueChangedEventHandler(this.AutoSyncChanged);
      this.reSyncTimer = new DispatcherTimer()
      {
        Interval = TimeSpan.FromHours(1.0)
      };
      this.reSyncTimer.Tick += new EventHandler(this.reSyncTimer_Tick);
      this.firmwareCheckTimer = new DispatcherTimer();
      this.firmwareCheckTimer.Tick += new EventHandler(this.firmwareCheckTimer_Tick);
      model.HeartBeat10Min.Beat += new HeartBeatHandler(this.Heartbeat10Min_Beat);
      this.deviceScanDelayTimer = new DispatcherTimer()
      {
        Interval = TimeSpan.FromMilliseconds(250.0)
      };
      this.deviceScanDelayTimer.Tick += new EventHandler(this.deviceScanDelayTimer_Tick);
      this.loginLogoutCompleteEvent = new AutoResetEvent(false);
      model.DeviceManager.DevicesChanged += new PropertyChangedEventHandler(this.DeviceManager_DevicesChanged);
      model.DeviceManager.CurrentDeviceChanged += new PropertyChangedEventHandler(this.DeviceManager_CurrentDeviceChanged);
      model.UserDeviceStatusChanged += new PropertyValueChangedEventHandler(this.model_UserDeviceStatusChanged);
      model.LoginLogoutStatusChanged += new PropertyValueChangedEventHandler(this.model_LoginLogoutStatusChanged);
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
      this.mainWindowPage.SubPageManager.ShowPage((SyncAppPageControl) this.loginPage);
      model.OOBECompleted += new EventHandler(this.model_OOBECompleted);
    }

    private async void model_OOBECompleted(object sender, EventArgs e)
    {
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
      await this.ConnectToCorrectDevice(false);
    }

    private void LogoutCommand_OnExecute(object parameter, EventArgs e)
    {
      if (!this.LogOut())
        return;
      this.MainWindowPageManager.HideModalPage();
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
    }

    private void PairDeviceConfirmCommand_OnExecute(object parameter, EventArgs e)
    {
      this.model.LoginInfo.UserProfile.LastDevicePairingError = (ErrorInfo) null;
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.pairDeviceConfirmPage);
    }

    private async void PairDeviceCommand_OnExecute(object parameter, EventArgs e)
    {
      this.model.LoginInfo.UserProfile.UpdatingDevicePairing = true;
      try
      {
        await this.ConnectToCorrectDevice(true);
      }
      finally
      {
        this.MainWindowPageManager.HideModalPage();
        this.model.LoginInfo.UserProfile.UpdatingDevicePairing = false;
      }
    }

    private void ForgetDeviceConfirmCommand_OnExecute(object parameter, EventArgs e)
    {
      this.model.LoginInfo.UserProfile.LastDevicePairingError = (ErrorInfo) null;
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.forgetDeviceConfirmPage);
    }

    private async void ForgetDeviceCommand_OnExecute(object parameter, EventArgs args)
    {
      this.model.LoginInfo.UserProfile.UpdatingDevicePairing = true;
      try
      {
        await this.model.DeviceManager.UnpairDevice(this.model.LoginInfo);
      }
      catch (Exception ex)
      {
        this.model.LogDevicePairingError(new ErrorInfo("ForgetDeviceContinuation", Strings.Message_DeviceUnpairingErrorOccurred, ex));
        return;
      }
      finally
      {
        this.model.LoginInfo.UserProfile.UpdatingDevicePairing = false;
      }
      this.model.LoginInfo.UserProfile.PairedDeviceIDUpdated();
      this.MainWindowPageManager.HideModalPage();
      await this.ConnectToCorrectDevice(false);
    }

    public async void CheckForFirmwareUpdate(object parameter, EventArgs e) => await this.CheckForFirmwareUpdate();

    public async void PushFirmwareUpdate(object parameter, EventArgs e) => await this.PushFirmwareUpdate();

    public void Dispose()
    {
      this.trayIcon.Dispose();
      this.loginLogoutCompleteEvent.Dispose();
      this.model.DeviceManager.CurrentDevice.Syncing = false;
    }

    public void SetFirmwareUpdateTimer()
    {
      this.firmwareCheckTimer.Stop();
      if (this.model.DeviceManager.CurrentDevice == null)
        return;
      if (this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus == null || this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable)
      {
        this.firmwareCheckTimer.Interval = new TimeSpan(0, 1, 0);
      }
      else
      {
        DateTime utcNow = DateTime.UtcNow;
        this.firmwareCheckTimer.Interval = !(utcNow < this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.LastChecked) ? (!(utcNow - this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.LastChecked > Globals.FWCheckUpdateSchedule) ? Globals.FWCheckUpdateSchedule - (utcNow - this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.LastChecked) : new TimeSpan(0, 0, 10)) : new TimeSpan(1, 0, 0);
      }
      this.firmwareCheckTimer.Start();
    }

    private async void firmwareCheckTimer_Tick(object sender, EventArgs e)
    {
      this.firmwareCheckTimer.Stop();
      if (this.model.DeviceManager.CurrentDevice == null || this.model.DeviceManager.CurrentDevice.FirmwareStatus == FirmwareStatus.Updating)
        return;
      if (this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus == null || this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable || DateTime.UtcNow - this.model.DeviceManager.CurrentDevice.DeviceInfo.FirmwareCheckStatus.LastChecked > Globals.FWCheckUpdateMinimum)
      {
        if (this.model.DeviceManager.CurrentDevice.Syncing)
          this.SetFirmwareUpdateTimer();
        else
          await this.CheckForFirmwareUpdate();
      }
      else
        this.SetFirmwareUpdateTimer();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.model.DeviceManager.ScanKDevices();
      if (this.IsWindows10())
        this.ShowUpsellPage();
      this.LoadLoginInformation();
      if (this.model.SecurityInfo != null)
        this.AuthenticateSafe(LoginContext.Automatic);
      else
        this.model.HideAfterLogin = false;
    }

    public DesktopSyncApp.App App => this.app;

    public ViewModel Model => this.model;

    public void SaveSettings() => this.model.DynamicSettings.MainWindowPosition = new Point?(new Point(this.Left, this.Top));

    private void SetStartupScreenPosition()
    {
      if (this.model.DynamicSettings.MainWindowPosition.HasValue)
      {
        this.WindowStartupLocation = WindowStartupLocation.Manual;
        this.Left = this.model.DynamicSettings.MainWindowPosition.Value.X;
        this.Top = this.model.DynamicSettings.MainWindowPosition.Value.Y;
      }
      else
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private async void reSyncTimer_Tick(object sender, EventArgs e) => await this.SyncDeviceToCloud();

    private void MainWindow_Closing_HideInstead(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      this.model.SaveInsightsData();
      this.model.AppVisibility = Visibility.Collapsed;
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
      this.SaveSettings();
      this.trayIcon.Dispose();
    }

    public void ShowApp()
    {
      this.model.AppState = WindowState.Normal;
      this.Activate();
    }

    public void ShowProfile()
    {
      Telemetry.LogPageView("Settings/User/Profile");
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.profilePage, direction: PageSlideDirection.Right);
      this.model.RefreshUserProfile(false);
    }

    public void ShowSync() => this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);

    public void ShowSettings()
    {
      Telemetry.LogPageView("Settings/Application");
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.settingsPage);
    }

    public void ShowAbout()
    {
      Telemetry.LogPageView("Settings/Application/About");
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.aboutPage);
      this.WindowState = WindowState.Normal;
      this.Activate();
    }

    public void ShowBandCustomization()
    {
      this.model.ThemeManager.SetBandClass(this.model.DeviceManager.CurrentDevice.DeviceBandClass);
      this.model.ThemeManager.SwitchToTheme(this.model.DeviceManager.CurrentDevice.MeTileId);
      this.model.ThemeManager.CurrentThemeComponent = ThemeComponent.ColorSet;
      this.themeColorPickerPage = new ThemeColorPickerControl(this);
      Telemetry.LogPageView("Settings/Band/Theme Chooser");
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.themeColorPickerPage, direction: PageSlideDirection.Right);
    }

    public void ShowTileManagement()
    {
      try
      {
        this.model.StrapManager.CurrentStartStrip = this.model.DeviceManager.CurrentDevice.CargoClient.GetStartStrip();
        this.model.StrapManager.TileBackground = this.model.DeviceManager.CurrentDevice.Theme.Base;
      }
      catch (Exception ex)
      {
        this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.errorMessagePage, false);
        this.model.LogError(new ErrorInfo(nameof (ShowTileManagement), "Unable to get Device Theme/StartStrip", ex));
        return;
      }
      this.tileManagementPage.SetPages();
      Telemetry.LogPageView("Settings/Band/Manage Tiles");
      this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.tileManagementPage, direction: PageSlideDirection.Right);
    }

    public void ShowUpsellPage()
    {
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.upsellPage);
      this.WindowState = WindowState.Normal;
      this.Activate();
    }

    public void ShowOfferAppUpdate()
    {
      this.MainWindowPageManager.ShowModalPage((SyncAppPageControl) this.launchSoftwareUpdatePage);
      this.WindowState = WindowState.Normal;
      this.Activate();
    }

    private async void AutoSyncChanged(object sender, PropertyValueChangedEventArgs args)
    {
      if ((bool) args.NewValue)
      {
        if (this.model.DeviceManager.CurrentDevice == null || this.model.LoginInfo == null || this.model.DeviceManager.CurrentDevice.Syncing)
          return;
        await this.SyncDeviceToCloud();
      }
      else
        this.reSyncTimer.Stop();
    }

    private async void btSync_Click(object sender, RoutedEventArgs e) => await this.SyncDeviceToCloud();

    private void OnPropertyChanged(string name, PropertyChangedEventHandler eventHandler = null)
    {
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(name);
      if (this.PropertyChanged != null)
        this.PropertyChanged((object) this, e);
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    public void MinimizeAppCommandExecute(object param, EventArgs e) => this.model.AppState = WindowState.Minimized;

    public void HideAppCommandExecute(object param, EventArgs e)
    {
      this.model.TelemetryListener.ResetSessionId();
      this.model.AppVisibility = Visibility.Hidden;
    }

    public void ShowAppCommandExecute(object param, EventArgs e)
    {
      this.model.TelemetryListener.ResetSessionId();
      this.ShowApp();
    }

    public void ShowSettingsCommandExecute(object param, EventArgs e) => this.ShowSettings();

    public void ShowAboutCommandExecute(object param, EventArgs e) => this.ShowAbout();

    public void ShowProfileCommandExecute(object param, EventArgs e) => this.ShowProfile();

    public void ShowSyncCommandExecute(object param, EventArgs e) => this.ShowSync();

    public void ShowBandCustomizationCommandExecute(object param, EventArgs e) => this.ShowBandCustomization();

    public void ShowTileManagementCommandExecute(object param, EventArgs e) => this.ShowTileManagement();

    public void CloseOverlayCommandExecute(object param, EventArgs e) => this.MainWindowPageManager.HideModalPage();

    public void CloseAppCommandExecute(object param, EventArgs e) => this.App.Shutdown();

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.model.LoginLogoutStatus = LoginLogoutStatus.LoggedOut;
      this.reSyncTimer.Stop();
      this.LogOut();
    }

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
    }

    private void model_LoginLogoutStatusChanged(object sender, PropertyValueChangedEventArgs args)
    {
      switch ((LoginLogoutStatus) args.NewValue)
      {
        case LoginLogoutStatus.LoggedOut:
          this.mainWindowPage.SubPageManager.ShowPage((SyncAppPageControl) this.loginPage, direction: PageSlideDirection.Right);
          break;
        case LoginLogoutStatus.LoggedIn:
          this.model_UserDeviceStatusChanged(sender, new PropertyValueChangedEventArgs("UserDeviceStatusChanged", (object) this.model.UserDeviceStatus, (object) this.model.UserDeviceStatus));
          break;
      }
    }

    private void model_UserDeviceStatusChanged(object sender, PropertyValueChangedEventArgs args)
    {
      UserDeviceStatus newValue = (UserDeviceStatus) args.NewValue;
      if (this.model.LoginLogoutStatus != LoginLogoutStatus.LoggedIn)
        return;
      if (newValue != UserDeviceStatus.RegisteredFWUpdating && this.SuperModalPageManager.ModelPage == this.firmwareUpdateProgressPage)
        this.SuperModalPageManager.HideModalPage();
      if (newValue != UserDeviceStatus.RegisteredRequiresFW && newValue != UserDeviceStatus.RegisteredFWUpdating && this.MainWindowPageManager.TopPage == this.firmwareUpdatePage)
        this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
      if (this.MainWindowPageManager.ModelPage == this.pairDeviceConfirmPage || this.MainWindowPageManager.ModelPage == this.errorMessagePage)
      {
        this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage, false);
        this.MainWindowPageManager.HideModalPage();
      }
      else if (this.MainWindowPageManager.TopPage == this.pairDevicePage)
        this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
      switch (newValue)
      {
        case UserDeviceStatus.None:
        case UserDeviceStatus.Multiple:
        case UserDeviceStatus.CantRegisterReset:
        case UserDeviceStatus.CantRegisterUnregister:
        case UserDeviceStatus.CantRegisterUnregisterReset:
          if (newValue == UserDeviceStatus.None)
            this.mainWindowPage.SubPageManager.ShowPage((SyncAppPageControl) this.disconnectedPage, false);
          else
            this.mainWindowPage.SubPageManager.ShowPage((SyncAppPageControl) this.cantPairDevicePage, this.MainWindowPageManager.TopPage == this.mainWindowPage, PageSlideDirection.Right);
          if (this.MainWindowPageManager.TopPage != this.themeColorPickerPage && this.MainWindowPageManager.TopPage != this.tileManagementPage)
            break;
          this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.mainWindowPage);
          break;
        case UserDeviceStatus.CanRegister:
          this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.pairDevicePage);
          break;
        case UserDeviceStatus.Registered:
          this.mainWindowPage.SubPageManager.ShowPage((SyncAppPageControl) this.syncPage, this.MainWindowPageManager.TopPage == this.mainWindowPage);
          break;
        case UserDeviceStatus.RegisteredRequiresFW:
          this.MainWindowPageManager.ShowPage((SyncAppPageControl) this.firmwareUpdatePage);
          break;
        case UserDeviceStatus.RegisteredFWUpdating:
          this.MainWindowPageManager.HideModalPage();
          this.SuperModalPageManager.ShowModalPage((SyncAppPageControl) this.firmwareUpdateProgressPage);
          break;
      }
    }

    public void WM_TIMECHANGE() => CultureInfo.CurrentCulture.ClearCachedData();

    private bool IsWindows10()
    {
      string str1;
      string str2;
      try
      {
        string name = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
        str1 = registryKey.GetValue("CurrentMajorVersionNumber").ToString();
        str2 = registryKey.GetValue("CurrentMinorVersionNumber").ToString();
      }
      catch (Exception ex)
      {
        return false;
      }
      return str1.Equals("10") && str2.Equals("0");
    }

    /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/mainwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MainWindow = (AppMainWindow) target;
          this.MainWindow.Closed += new EventHandler(this.MainWindow_Closed);
          this.MainWindow.Closing += new CancelEventHandler(this.MainWindow_Closing_HideInstead);
          this.MainWindow.KeyDown += new KeyEventHandler(this.MainWindow_KeyDown);
          this.MainWindow.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
          this.MainWindow.MouseDown += new MouseButtonEventHandler(this.MainWindow_MouseDown);
          break;
        case 2:
          this.MainWindowPageManager = (AnimatedPageControl) target;
          break;
        case 3:
          this.SuperModalPageManager = (AnimatedPageControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
    */
    private enum ProfileSyncType
    {
      None,
      Get,
      Save,
    }
  }
}
