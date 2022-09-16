// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.KDeviceManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopSyncApp
{
  public class KDeviceManager : INotifyPropertyChanged
  {
    private SortedDictionary<Guid, KDeviceInfo> presentDevices;
    private ViewModel model;
    private KDeviceConnection currentDevice;
    private bool scanRunning;
    private bool scanRequested;
    private SemaphoreSlim scanLocker = new SemaphoreSlim(1);
    private ErrorInfo lastDeviceConnectError;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyChangedEventHandler DevicesChanged;

    public event PropertyChangedEventHandler CurrentDeviceChanged;

    public KDeviceManager(ViewModel model)
    {
      this.model = model;
      this.presentDevices = new SortedDictionary<Guid, KDeviceInfo>();
    }

    public KDeviceConnection CurrentDevice
    {
      get => this.currentDevice;
      private set
      {
        this.currentDevice = value;
        if (this.currentDevice != null)
          this.currentDevice.LastLogSyncTime = this.model.DynamicSettings.LastLogSync;
        this.OnPropertyChanged(nameof (CurrentDevice), this.PropertyChanged, this.CurrentDeviceChanged);
      }
    }

    public int Count => this.presentDevices.Count;

    public bool SuppressDeviceScan { get; set; }

    public ErrorInfo LastDeviceConnectError
    {
      get => this.lastDeviceConnectError;
      set
      {
        if (this.lastDeviceConnectError == value)
          return;
        this.lastDeviceConnectError = value;
        this.OnPropertyChanged(nameof (LastDeviceConnectError), this.PropertyChanged);
      }
    }

    public async void ScanKDevices()
    {
      if (this.SuppressDeviceScan || !this.BeginScan())
        return;
      bool devicesChanged = false;
      do
      {
        try
        {
          using (await SemaphoreLocker.LockAsync(this.scanLocker))
          {
            try
            {
              bool flag = devicesChanged;
              int num = await this.ScanKDevicesInternal() ? 1 : 0;
              devicesChanged = flag | num != 0;
            }
            catch (Exception ex)
            {
              this.model.LogError(new ErrorInfo(nameof (ScanKDevices), "ScanKDevicesInternal failed", ex));
            }
          }
        }
        catch
        {
        }
      }
      while (this.ScanAgain());
      if (!devicesChanged)
        return;
      this.OnPropertyChanged("Devices", this.DevicesChanged);
    }

    private bool BeginScan()
    {
      lock (this.scanLocker)
      {
        if (this.scanRunning)
        {
          this.scanRequested = true;
          return false;
        }
        this.scanRunning = true;
        return true;
      }
    }

    private bool ScanAgain()
    {
      lock (this.scanLocker)
      {
        if (!this.scanRequested)
        {
          this.scanRunning = false;
          return false;
        }
        this.scanRequested = false;
        return true;
      }
    }

    private async Task<bool> ScanKDevicesInternal()
    {
      Dictionary<Guid, KDeviceInfo> newDevices = new Dictionary<Guid, KDeviceInfo>();
      IBandInfo[] bandsAsync = await BandAdminClientManager.Instance.GetBandsAsync();
      if (this.CurrentDevice != null && this.CurrentDevice.FirmwareStatus == FirmwareStatus.Updating)
        newDevices[this.CurrentDevice.DeviceInfo.UniqueID] = this.CurrentDevice.DeviceInfo;
      IBandInfo[] ibandInfoArray = bandsAsync;
      for (int index = 0; index < ibandInfoArray.Length; ++index)
      {
        IBandInfo deviceInfo = ibandInfoArray[index];
        if (this.scanRequested)
          return false;
        bool deviceIdentified = false;
        FirmwareVersions versions = (FirmwareVersions) null;
        Guid empty = Guid.Empty;
        int tries = 1;
        StringBuilder failures = new StringBuilder();
        if (this.CurrentDevice != null && this.CurrentDevice.DeviceInfo.DeviceInfo.Name == deviceInfo.Name)
        {
          deviceIdentified = true;
          newDevices[this.CurrentDevice.DeviceInfo.UniqueID] = this.CurrentDevice.DeviceInfo;
        }
        for (; tries <= 3 && !deviceIdentified; ++tries)
        {
          try
          {
            ICargoClient client = (ICargoClient) null;
            try
            {
              client = await this.OpenDevice(deviceInfo);
            }
            catch (Exception ex)
            {
              this.model.LogError(new ErrorInfo(nameof (ScanKDevicesInternal), "Unable to create CargoClient", ex));
              failures.AppendFormat("{0}CargoClient.Create()", failures.Length > 0 ? (object) ", " : (object) "");
              continue;
            }
            using (client)
            {
              try
              {
                versions = client.FirmwareVersions;
              }
              catch (Exception ex)
              {
                this.model.LogError(new ErrorInfo(nameof (ScanKDevicesInternal), "Unable to get device firmware versions", ex));
                failures.AppendFormat("{0}Firmware Version", failures.Length > 0 ? (object) ", " : (object) "");
                throw;
              }
              if ((int)client.DeviceTransportApp == 3)
              {
                try
                {
                  Guid userId = (await client.GetUserProfileFromDeviceAsync()).UserID;
                }
                catch (Exception ex)
                {
                  this.model.LogError(new ErrorInfo(nameof (ScanKDevicesInternal), "Unable to get device user ID", ex));
                  failures.AppendFormat("{0}User ID", failures.Length > 0 ? (object) ", " : (object) "");
                  throw;
                }
              }
              newDevices[client.DeviceUniqueId] = new KDeviceInfo(this.model, client.DeviceUniqueId, deviceInfo, versions, client.DeviceTransportApp, tries, "Failures: " + failures.ToString());
              deviceIdentified = true;
            }
            client = (ICargoClient) null;
          }
          catch (TimeoutException ex)
          {
          }
          catch (Exception ex)
          {
            break;
          }
        }
        versions = (FirmwareVersions) null;
        failures = (StringBuilder) null;
        deviceInfo = (IBandInfo) null;
      }
      ibandInfoArray = (IBandInfo[]) null;
      bool flag1 = false;
      bool flag2 = false;
      if (this.scanRequested)
        return false;
      lock (this.presentDevices)
      {
        foreach (KeyValuePair<Guid, KDeviceInfo> keyValuePair in newDevices)
        {
          KDeviceInfo kdeviceInfo1 = keyValuePair.Value;
          bool flag3 = false;
          KDeviceInfo kdeviceInfo2;

            if (this.presentDevices.TryGetValue(kdeviceInfo1.UniqueID, out kdeviceInfo2))
            {
                //TODO
                //if (kdeviceInfo1.DeviceInfo.Name != kdeviceInfo2.DeviceInfo.Name 
                //                || FirmwareVersion.Inequality(kdeviceInfo1.Versions.UpdaterVersion, kdeviceInfo2.Versions.UpdaterVersion) 
                //                || FirmwareVersion.op_Inequality(kdeviceInfo1.Versions.ApplicationVersion, kdeviceInfo2.Versions.ApplicationVersion) 
                //                || kdeviceInfo1.AppType != kdeviceInfo2.AppType)
                //{
                flag3 = true;
                //}
            }
            else
            {
                flag3 = true;
            }

          if (flag3)
            this.presentDevices[kdeviceInfo1.UniqueID] = kdeviceInfo1;
          flag1 |= flag3;
          if (this.CurrentDevice != null && this.CurrentDevice.DeviceInfo.UniqueID == kdeviceInfo1.UniqueID && this.CurrentDevice.DeviceInfo.DeviceInfo.Name == kdeviceInfo1.DeviceInfo.Name)
            flag2 = true;
        }
        List<Guid> guidList = new List<Guid>();
        foreach (KeyValuePair<Guid, KDeviceInfo> presentDevice in this.presentDevices)
        {
          KDeviceInfo kdeviceInfo = presentDevice.Value;
          if (!newDevices.ContainsKey(kdeviceInfo.UniqueID))
            guidList.Add(kdeviceInfo.UniqueID);
        }
        foreach (Guid key in guidList)
          this.presentDevices.Remove(key);
        flag1 |= guidList.Count > 0;
      }
      if (this.CurrentDevice != null && !flag2)
        this.CloseDevice();
      return flag1 || this.CurrentDevice == null;
    }

    private async Task<ICargoClient> OpenDevice(IBandInfo device) => await BandAdminClientManager.Instance.ConnectAsync(device);

    private async Task<ICargoClient> OpenDevice(
      IBandInfo device,
      ServiceInfo serviceInfo)
    {
      ICargoClient cargoClient = await BandAdminClientManager.Instance.ConnectAsync(device, serviceInfo);
      try
      {
        cargoClient.UserAgent = KDeviceManager.GetUserAgent(cargoClient);
      }
      catch
      {
        ((IDisposable) cargoClient).Dispose();
      }
      return cargoClient;
    }

    public KDeviceInfo GetSingleDevice()
    {
      if (this.Count != 1)
        throw new InvalidOperationException("Count =! 1");
      KDeviceInfo kdeviceInfo = (KDeviceInfo) null;
      using (SortedDictionary<Guid, KDeviceInfo>.Enumerator enumerator = this.presentDevices.GetEnumerator())
      {
        if (enumerator.MoveNext())
          kdeviceInfo = enumerator.Current.Value;
      }
      return kdeviceInfo;
    }

    public async Task UpdateDeviceInfo(
      KDeviceInfo deviceInfo,
      LoginInfo loginInfo,
      bool force,
      bool throwOnFimwareCheckError)
    {
      ICargoClient client = (ICargoClient) null;
      SemaphoreLocker locker = (SemaphoreLocker) null;
      try
      {
        if (this.CurrentDevice != null && this.CurrentDevice.DeviceInfo == deviceInfo)
        {
          client = this.CurrentDevice.CargoClient;
        }
        else
        {
          locker = await SemaphoreLocker.LockAsync(this.scanLocker);
          client = await this.OpenDevice(deviceInfo.DeviceInfo, loginInfo.ServiceInfo);
        }
        KDeviceInfo kdeviceInfo;
        if (!deviceInfo.Loaded | force)
        {
          kdeviceInfo = deviceInfo;
          RunningAppType runningAppAsync = await client.GetRunningAppAsync();
          kdeviceInfo.AppType = runningAppAsync;
          kdeviceInfo = (KDeviceInfo) null;
        }
        deviceInfo.Versions = client.FirmwareVersions;
        if (deviceInfo.FirmwareCheckStatus == null | force || (int)deviceInfo.AppType == 2 && !deviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable)
        {
          IFirmwareUpdateInfo fwInfo = (IFirmwareUpdateInfo) null;
          try
          {
            List<KeyValuePair<string, string>> keyValuePairList = (List<KeyValuePair<string, string>>) null;
            if (this.model.ForceFirmwareUpdateCheck)
            {
              this.model.ForceFirmwareUpdateCheck = false;
              keyValuePairList = new List<KeyValuePair<string, string>>()
              {
                new KeyValuePair<string, string>("Debug.Force", "True")
              };
            }
            fwInfo = await client.GetLatestAvailableFirmwareVersionAsync(keyValuePairList);
          }
          catch (Exception ex)
          {
            if (throwOnFimwareCheckError)
              throw;
            else
              this.model.LogFWCheckError(new ErrorInfo(nameof (UpdateDeviceInfo), "Unable to check for firmware update", ex));
          }
          if (fwInfo != null)
            deviceInfo.FirmwareCheckStatus = new FirmwareCheckStatus(fwInfo, DateTime.UtcNow);
          fwInfo = (IFirmwareUpdateInfo) null;
        }
        if ((int)deviceInfo.AppType == 3)
        {
          if (deviceInfo.CanReportOOBEComplete | force)
          {
            try
            {
              kdeviceInfo = deviceInfo;
              int num = await client.GetDeviceOobeCompletedAsync() ? 1 : 0;
              kdeviceInfo.IsOOBEComplete = num != 0;
              kdeviceInfo = (KDeviceInfo) null;
              deviceInfo.CanReportOOBEComplete = true;
            }
            catch (TimeoutException ex)
            {
              deviceInfo.CanReportOOBEComplete = false;
            }
          }
          kdeviceInfo = deviceInfo;
          DeviceProfileStatus profileLinkStatusAsync = await client.GetDeviceAndProfileLinkStatusAsync(loginInfo.UserProfile.Source);
          kdeviceInfo.DeviceProfileStatus = profileLinkStatusAsync;
          kdeviceInfo = (KDeviceInfo) null;
        }
        else
        {
          deviceInfo.IsOOBEComplete = false;
          deviceInfo.DeviceProfileStatus = new DeviceProfileStatus();
        }
        deviceInfo.Loaded = true;
      }
      finally
      {
        if (locker != null)
        {
          ((IDisposable) client)?.Dispose();
          locker.Dispose();
        }
      }
    }

    public async Task<bool> OpenDevice(KDeviceInfo deviceInfo, LoginInfo loginInfo)
    {
      if (this.CurrentDevice != null)
      {
        if (this.CurrentDevice.DeviceInfo.UniqueID == deviceInfo.UniqueID)
          return true;
        throw new Exception("Device already open");
      }
      using (await SemaphoreLocker.LockAsync(this.scanLocker))
      {
        ICargoClient client = await this.OpenDevice(deviceInfo.DeviceInfo, loginInfo.ServiceInfo);
        try
        {
          if ((int)deviceInfo.AppType == 3 
              && 
             ((int)deviceInfo.DeviceProfileStatus.UserLinkStatus != 1 
             ||
             (int)deviceInfo.DeviceProfileStatus.DeviceLinkStatus != 1))
          {
            this.model.ThemeManager.SetBandClass(client.ConnectedBandConstants.BandClass);
            if ((int)client.ConnectedBandConstants.BandClass == 2)
            {
              await client.NavigateToScreenAsync((CargoScreen) 12);
              await this.SetDefaultTiles(client, loginInfo.ServiceInfo);
              int num1 = await client.UpdateGpsEphemerisDataAsync(CancellationToken.None, true) ? 1 : 0;
              int num2 = await client.UpdateTimeZoneListAsync(CancellationToken.None, true, (IUserProfile) null) ? 1 : 0;
              await client.SetCurrentTimeAndTimeZoneAsync();
              await client.SetOobeStageAsync((OobeStage) 8);
              await client.LinkDeviceToProfileAsync(loginInfo.UserProfile.Source, true);
              await client.FinalizeOobeAsync();
            }
            else
            {
              await client.LinkDeviceToProfileAsync(loginInfo.UserProfile.Source, true);
              await this.SaveThemeToBand(client, (KDeviceConnection) null, this.model.ThemeManager.DefaultColorId, this.model.ThemeManager.DefaultPatternId, true);
            }
            KDeviceInfo kdeviceInfo = deviceInfo;
            DeviceProfileStatus profileLinkStatusAsync = await client.GetDeviceAndProfileLinkStatusAsync(loginInfo.UserProfile.Source);
            kdeviceInfo.DeviceProfileStatus = profileLinkStatusAsync;
            kdeviceInfo = (KDeviceInfo) null;
          }
          this.CurrentDevice = new KDeviceConnection(this.model, this, client, deviceInfo, loginInfo.ServiceInfo);
        }
        catch
        {
          ((IDisposable) client).Dispose();
          throw;
        }
      }
      return true;
    }

    private async Task SaveThemeToBand(
      ICargoClient client,
      KDeviceConnection device,
      int colorId,
      int patternId,
      bool inOobe)
    {
      this.model.ThemeManager.ColorSets.TrySetSelectedItemById(colorId);
      this.model.ThemeManager.Patterns.TrySetSelectedItemById(patternId);
      await this.SaveThemeToBand(client, device, inOobe);
    }

    public async Task SaveThemeToBand(
      ICargoClient client,
      KDeviceConnection device,
      bool inOobe)
    {
      uint newMeTileId = (uint) ((this.model.ThemeManager.ColorSets.SelectedItem.Id & (int) ushort.MaxValue) << 16 | this.model.ThemeManager.Patterns.SelectedItem.Id & (int) ushort.MaxValue);
      BitmapSource source = (BitmapSource) this.model.ThemeManager.Patterns.SelectedItem.Image;
      if (source.Format != PixelFormats.Pbgra32)
        source = (BitmapSource) new FormatConvertedBitmap(source, PixelFormats.Pbgra32, (BitmapPalette) null, 0.0);
      await client.PersonalizeDeviceAsync((StartStrip) null, WriteableBitmapExtensions.ToBandImage(new WriteableBitmap(source)), this.model.ThemeManager.ColorSets.SelectedItem.Colors.Palette, newMeTileId, (IDictionary<Guid, BandTheme>) null);
      if (device != null)
      {
        device.MeTileId = newMeTileId;
        device.MeTileImage = (ImageSource) this.model.ThemeManager.Patterns.SelectedItem.Image;
        device.Theme = this.model.ThemeManager.ColorSets.SelectedItem.Colors;
      }
      ApplicationTelemetry.LogBandThemeChange(this.model.ThemeManager.Patterns.SelectedItem.Name, this.model.ThemeManager.ColorSets.SelectedItem.Name, inOobe);
    }

    private async Task SetDefaultTiles(ICargoClient client, ServiceInfo serviceInfo)
    {
      IList<AdminBandTile> defaults = await client.GetDefaultTilesAsync();
      AdminBandTile adminBandTile = ((IEnumerable<AdminBandTile>) defaults).SingleOrDefault<AdminBandTile>((Func<AdminBandTile, bool>) (s => s.Id == new Guid("59976cf5-15c8-4799-9e31-f34c765a6bd1")));
      AdminBandTile exerciseStrapp = ((IEnumerable<AdminBandTile>) defaults).SingleOrDefault<AdminBandTile>((Func<AdminBandTile, bool>) (s => s.Id == new Guid("a708f02a-03cd-4da0-bb33-be904e6a2924")));
      if (adminBandTile != null)
        ((ICollection<AdminBandTile>) defaults).Remove(adminBandTile);
      if (exerciseStrapp != null)
      {
        try
        {
          IList<ExerciseTag> exerciseTagsAsync = await new CloudCaller(serviceInfo).GetExerciseTagsAsync(CancellationToken.None);
          if (exerciseTagsAsync != null)
          {
            List<ExerciseTag> list1 = ((IEnumerable<ExerciseTag>) exerciseTagsAsync).Where<ExerciseTag>((Func<ExerciseTag, bool>) (tag => tag.IsChecked)).ToList<ExerciseTag>();
            if (list1 != null)
            {
              IList<WorkoutActivity> list2 = (IList<WorkoutActivity>) ((IEnumerable<ExerciseTag>) list1).Select<ExerciseTag, WorkoutActivity>(new Func<ExerciseTag, WorkoutActivity>(this.ConvertToWorkoutActivity)).ToList<WorkoutActivity>();
              if (list2 != null)
                await client.SetWorkoutActivitiesAsync(list2);
            }
          }
        }
        catch (Exception ex)
        {
          this.model.LogError(new ErrorInfo("KDeviceManager.DefaultTiles", "Could not set Exercise Tags", ex));
          ((ICollection<AdminBandTile>) defaults).Remove(exerciseStrapp);
        }
      }
      await client.SetStartStripAsync(new StartStrip((IEnumerable<AdminBandTile>) defaults));
      if (((IEnumerable<AdminBandTile>) defaults).Any<AdminBandTile>((Func<AdminBandTile, bool>) (d => d.Id == new Guid("b4edbc35-027b-4d10-a797-1099cd2ad98a"))))
        await client.SetSmsResponsesAsync(LStrings.DefaultMessageCannedResponses1, LStrings.DefaultMessageCannedResponses2, string.Empty, string.Empty);
      if (!((IEnumerable<AdminBandTile>) defaults).Any<AdminBandTile>((Func<AdminBandTile, bool>) (d => d.Id == new Guid("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"))))
        return;
      await client.SetPhoneCallResponsesAsync
          (
              LStrings.DefaultPhoneCannedResponses1, 
              LStrings.DefaultPhoneCannedResponses2, 
              LStrings.DefaultPhoneCannedResponses3, 
              string.Empty
          );
    }

    private WorkoutActivity ConvertToWorkoutActivity(ExerciseTag exerciseTag) => new WorkoutActivity(exerciseTag.ExerciseTypeId, exerciseTag.Text)
    {
      Flags = exerciseTag.Flags,
      TrackingAlgorithmId = (byte) exerciseTag.Algorithm
    };

    public async Task UnpairDevice(LoginInfo loginInfo)
    {
      if (this.currentDevice != null)
      {
        await this.currentDevice.CargoClient.UnlinkDeviceFromProfileAsync(loginInfo.UserProfile.Source);
        KDeviceInfo kdeviceInfo = this.currentDevice.DeviceInfo;
        DeviceProfileStatus profileLinkStatusAsync = await this.currentDevice.CargoClient.GetDeviceAndProfileLinkStatusAsync(loginInfo.UserProfile.Source);
        kdeviceInfo.DeviceProfileStatus = profileLinkStatusAsync;
        kdeviceInfo = (KDeviceInfo) null;
        this.CurrentDevice.Dispose();
        this.CurrentDevice = (KDeviceConnection) null;
      }
      else
      {
        using (ICargoClient client = BandAdminClientManager.Instance.Connect(loginInfo.ServiceInfo))
          await client.UnlinkDeviceFromProfileAsync(loginInfo.UserProfile.Source);
        this.OnPropertyChanged("Devices", this.DevicesChanged);
      }
    }

    public void CloseDevice()
    {
      if (this.CurrentDevice == null)
        return;
      try
      {
        this.CurrentDevice.Dispose();
      }
      catch
      {
      }
      this.CurrentDevice = (KDeviceConnection) null;
    }

    public static string GetUserAgent(ICargoClient cargoClient)
    {
      Version version = Globals.ApplicationVersion;
      if (version == (Version) null || version == new Version("1.0.0.0"))
        version = new Version(1, 99, Convert.ToInt32(DateTime.UtcNow.ToString("Mdd")), 99);
      return string.Format("KApp/{0} (.NET CLR/{1}; {2}/{3}; {4}) Cargo/{5} (PcbId/{6})", (object) version, (object) Environment.Version, (object) "Windows Desktop", (object) Globals.HostOSVersion, (object) CultureInfo.CurrentCulture.Name, (object) cargoClient.FirmwareVersions.ApplicationVersion, (object) cargoClient.FirmwareVersions.PcbId);
    }
  }
}
