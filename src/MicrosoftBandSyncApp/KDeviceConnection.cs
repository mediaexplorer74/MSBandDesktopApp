// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.KDeviceConnection
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Admin.Streaming;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DesktopSyncApp
{
  public sealed class KDeviceConnection : IDisposable, INotifyPropertyChanged
  {
    private KDeviceManager manager;
    private ICargoClient cargoClient;
    private byte? batteryPercentCharge;
    private FirmwareStatus firmwareStatus = FirmwareStatus.UpToDate;
    private DeviceSyncProgress syncProgress;
    private FirmwareUpdateProgress firmwareUpdateProgress;
    private DateTime? lastLogSyncTime;
    private bool syncing;
    private CancellationTokenSource syncCancel;
    private bool checkingFirmware;
    private DateTime? lastKnownDeviceTime;
    private Stopwatch sinceLastKnownDeviceTime = new Stopwatch();
    private ViewModel model;
    private string deviceSerialNumber;
    private IUserProfile userProfile;
    private ErrorInfo lastSyncError;
    private ErrorInfo lastFWCheckError;
    private uint meTileId;
    private BandClass deviceBandClass;
    private ImageSource meTileImage;
    private ThemeColorPaletteProxy theme;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyChangedEventHandler FirmwareStatusChanged;

    public event PropertyChangedEventHandler SyncingChanged;

    public event PropertyValueChangedEventHandler BatteryPercentChargeChanged;

    public event PropertyValueChangedEventHandler LastSyncErrorChanged;

    public KDeviceConnection(
      ViewModel model,
      KDeviceManager manager,
      ICargoClient cargoClient,
      KDeviceInfo deviceInfo,
      ServiceInfo serviceInfo)
    {
      this.model = model;
      this.manager = manager;
      this.cargoClient = cargoClient;
      this.DeviceInfo = deviceInfo;
      this.ServiceInfo = serviceInfo;
      cargoClient.BatteryGaugeUpdated += new EventHandler<BatteryGaugeUpdatedEventArgs>(this.cargoClient_BatteryGaugeUpdated);
      model.HeartBeat01Sec.Beat += new HeartBeatHandler(this.heartBeat01Sec_Beat);
      model.HeartBeat05Sec.Beat += new HeartBeatHandler(this.heartBeat05Sec_Beat);
      if (deviceInfo.FirmwareCheckStatus != null)
        this.firmwareStatus = deviceInfo.FirmwareCheckStatus.Info.IsFirmwareUpdateAvailable ? FirmwareStatus.Available : FirmwareStatus.UpToDate;
      else
        this.firmwareStatus = FirmwareStatus.Unknown;
    }

    public ICargoClient CargoClient => this.cargoClient != null ? this.cargoClient : throw new ObjectDisposedException(nameof (KDeviceConnection));

    public KDeviceInfo DeviceInfo { get; private set; }

    public ServiceInfo ServiceInfo { get; private set; }

    public byte? BatteryPercentCharge
    {
      get => this.batteryPercentCharge;
      set
      {
        byte? batteryPercentCharge = this.batteryPercentCharge;
        byte? nullable1 = batteryPercentCharge;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        byte? nullable3 = value;
        int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        if ((nullable2.GetValueOrDefault() == nullable4.GetValueOrDefault() ? (nullable2.HasValue != nullable4.HasValue ? 1 : 0) : 1) == 0)
          return;
        this.batteryPercentCharge = value;
        this.OnPropertyChanged(nameof (BatteryPercentCharge), this.PropertyChanged, this.BatteryPercentChargeChanged, (object) batteryPercentCharge, (object) value);
      }
    }

    public FirmwareStatus FirmwareStatus
    {
      get => this.firmwareStatus;
      set
      {
        if (this.firmwareStatus == value)
          return;
        this.firmwareStatus = value;
        this.OnPropertyChanged(nameof (FirmwareStatus), this.PropertyChanged, this.FirmwareStatusChanged);
      }
    }

    public bool Syncing
    {
      get => this.syncing;
      set
      {
        if (this.syncing == value)
          return;
        this.syncing = value;
        this.OnPropertyChanged(nameof (Syncing), this.PropertyChanged, this.SyncingChanged);
        if (this.syncing)
          return;
        if (this.syncCancel != null)
          this.syncCancel.Dispose();
        this.syncCancel = (CancellationTokenSource) null;
        this.syncProgress = (DeviceSyncProgress) null;
      }
    }

    public CancellationTokenSource SyncCancel
    {
      get => this.syncCancel;
      set
      {
        if (this.syncCancel == value)
          return;
        this.syncCancel = value;
        this.OnPropertyChanged(nameof (SyncCancel), this.PropertyChanged);
      }
    }

    public bool CheckingFirmware
    {
      get => this.checkingFirmware;
      set
      {
        if (this.checkingFirmware == value)
          return;
        this.checkingFirmware = value;
        this.OnPropertyChanged(nameof (CheckingFirmware), this.PropertyChanged);
      }
    }

    public DeviceSyncProgress SyncProgress
    {
      get => this.syncProgress;
      set
      {
        this.syncProgress = value;
        this.OnPropertyChanged(nameof (SyncProgress), this.PropertyChanged);
      }
    }

    public FirmwareUpdateProgress FirmwareUpdateProgress
    {
      get => this.firmwareUpdateProgress;
      set
      {
        if (this.firmwareUpdateProgress == value)
          return;
        this.firmwareUpdateProgress = value;
        this.OnPropertyChanged(nameof (FirmwareUpdateProgress), this.PropertyChanged);
      }
    }

    public DateTime? LastLogSyncTime
    {
      get => this.lastLogSyncTime;
      set
      {
        DateTime? lastLogSyncTime = this.lastLogSyncTime;
        DateTime? nullable = value;
        if ((lastLogSyncTime.HasValue == nullable.HasValue ? (lastLogSyncTime.HasValue ? (lastLogSyncTime.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.lastLogSyncTime = value;
        this.OnPropertyChanged(nameof (LastLogSyncTime), this.PropertyChanged);
        this.OnPropertyChanged("LastLogSyncRelativeTime", this.PropertyChanged);
      }
    }

    public TimeSpan? LastLogSyncRelativeTime
    {
      get
      {
        if (!this.lastLogSyncTime.HasValue)
          return new TimeSpan?();
        DateTime utcNow = DateTime.UtcNow;
        DateTime? lastLogSyncTime = this.lastLogSyncTime;
        return !lastLogSyncTime.HasValue ? new TimeSpan?() : new TimeSpan?(utcNow - lastLogSyncTime.GetValueOrDefault());
      }
    }

    public DateTime? LastKnownDeviceTime
    {
      get => this.lastKnownDeviceTime;
      set
      {
        DateTime? lastKnownDeviceTime = this.lastKnownDeviceTime;
        DateTime? nullable = value;
        if ((lastKnownDeviceTime.HasValue == nullable.HasValue ? (lastKnownDeviceTime.HasValue ? (lastKnownDeviceTime.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1) == 0)
          return;
        this.lastKnownDeviceTime = value;
        if (this.lastKnownDeviceTime.HasValue)
          this.sinceLastKnownDeviceTime.Restart();
        else
          this.sinceLastKnownDeviceTime.Stop();
        this.OnPropertyChanged(nameof (LastKnownDeviceTime), this.PropertyChanged);
        this.OnPropertyChanged("CalculatedCurrentLocalDeviceTime", this.PropertyChanged);
      }
    }

    public DateTime? CalculatedCurrentLocalDeviceTime
    {
      get
      {
        if (!this.lastKnownDeviceTime.HasValue)
          return new DateTime?();
        DateTime? lastKnownDeviceTime = this.lastKnownDeviceTime;
        TimeSpan elapsed = this.sinceLastKnownDeviceTime.Elapsed;
        return !lastKnownDeviceTime.HasValue ? new DateTime?() : new DateTime?(lastKnownDeviceTime.GetValueOrDefault() + elapsed);
      }
    }

    public string DeviceSerialNumber
    {
      get => this.deviceSerialNumber;
      set
      {
        if (!(this.deviceSerialNumber != value))
          return;
        this.deviceSerialNumber = value;
        this.OnPropertyChanged(nameof (DeviceSerialNumber), this.PropertyChanged);
      }
    }

    public IUserProfile UserProfile
    {
      get => this.userProfile;
      set
      {
        if (this.userProfile == value)
          return;
        this.userProfile = value;
        this.OnPropertyChanged(nameof (UserProfile), this.PropertyChanged);
      }
    }

    public uint MeTileId
    {
      get => this.meTileId;
      set
      {
        this.meTileId = value;
        this.OnPropertyChanged(nameof (MeTileId), this.PropertyChanged);
      }
    }

    public BandClass DeviceBandClass
    {
      get => this.deviceBandClass;
      set
      {
        this.deviceBandClass = value;
        this.OnPropertyChanged(nameof (DeviceBandClass), this.PropertyChanged);
        this.OnPropertyChanged("DeviceBandClassIsEnvoy", this.PropertyChanged);
      }
    }

    public bool DeviceBandClassIsEnvoy => this.deviceBandClass == 2;

    public ImageSource MeTileImage
    {
      get => this.meTileImage;
      set
      {
        this.meTileImage = value;
        this.OnPropertyChanged(nameof (MeTileImage), this.PropertyChanged);
      }
    }

    public ThemeColorPaletteProxy Theme
    {
      get => this.theme;
      set
      {
        this.theme = value;
        this.OnPropertyChanged(nameof (Theme), this.PropertyChanged);
      }
    }

    public ErrorInfo LastSyncError
    {
      get => this.lastSyncError;
      set
      {
        ErrorInfo lastSyncError = this.lastSyncError;
        if (lastSyncError == value)
          return;
        this.lastSyncError = value;
        this.OnPropertyChanged(nameof (LastSyncError), this.PropertyChanged, this.LastSyncErrorChanged, (object) lastSyncError, (object) value);
      }
    }

    public ErrorInfo LastFWCheckError
    {
      get => this.lastFWCheckError;
      set
      {
        if (this.lastFWCheckError == value)
          return;
        this.lastFWCheckError = value;
        this.OnPropertyChanged(nameof (LastFWCheckError), this.PropertyChanged);
      }
    }

    private void heartBeat01Sec_Beat(object sender, EventArgs e)
    {
      if (this.lastLogSyncTime.HasValue)
        this.OnPropertyChanged("LastLogSyncRelativeTime", this.PropertyChanged);
      if (!this.lastKnownDeviceTime.HasValue)
        return;
      this.OnPropertyChanged("CalculatedCurrentLocalDeviceTime", this.PropertyChanged);
    }

    private async void heartBeat05Sec_Beat(object sender, EventArgs e)
    {
      try
      {
        this.LastKnownDeviceTime = new DateTime?(await this.cargoClient.GetDeviceLocalTimeAsync());
      }
      catch
      {
      }
    }

    private void cargoClient_BatteryGaugeUpdated(object sender, BatteryGaugeUpdatedEventArgs e) => this.BatteryPercentCharge = new byte?(e.PercentCharge);

    public void GetFirmwareVersions() => this.DeviceInfo.Versions = this.CargoClient.FirmwareVersions;

    public async Task GetInfoFromDevice()
    {
      this.DeviceBandClass = this.cargoClient.ConnectedBandConstants.BandClass;
      this.DeviceSerialNumber = await this.cargoClient.GetProductSerialNumberAsync();
      this.LastKnownDeviceTime = new DateTime?(await this.cargoClient.GetDeviceLocalTimeAsync());
      this.UserProfile = await this.cargoClient.GetUserProfileFromDeviceAsync();
      this.Theme = new ThemeColorPaletteProxy(await this.cargoClient.GetDeviceThemeAsync());
      this.MeTileId = await this.cargoClient.GetMeTileIdAsync();
      if (this.meTileId <= 0U)
        return;
      try
      {
        this.MeTileImage = (ImageSource) BandImageExtensions.ToWriteableBitmap(await this.cargoClient.GetMeTileImageAsync());
      }
      catch (Exception ex)
      {
        this.model.LogError(new ErrorInfo(nameof (GetInfoFromDevice), "Unable to get Device MeTileImage", ex));
      }
    }

    private static Color ColorFromRGB(uint rgb) => Color.FromArgb(byte.MaxValue, (byte) (rgb >> 16 & (uint) byte.MaxValue), (byte) (rgb >> 8 & (uint) byte.MaxValue), (byte) (rgb & (uint) byte.MaxValue));

    public void Dispose()
    {
      if (this.cargoClient == null)
        return;
      this.cargoClient.BatteryGaugeUpdated -= new EventHandler<BatteryGaugeUpdatedEventArgs>(this.cargoClient_BatteryGaugeUpdated);
      this.model.HeartBeat01Sec.Beat -= new HeartBeatHandler(this.heartBeat01Sec_Beat);
      this.model.HeartBeat05Sec.Beat -= new HeartBeatHandler(this.heartBeat05Sec_Beat);
      ((IDisposable) this.cargoClient).Dispose();
      this.cargoClient = (ICargoClient) null;
    }
  }
}
