// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.BandAdminClientManager
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Admin.Desktop;
using Microsoft.Band.Desktop;
using System;
using System.Threading.Tasks;

namespace Microsoft.Band.Admin
{
  public class BandAdminClientManager : IBandAdminClientManager
  {
    private static readonly BandAdminClientManager instance = new BandAdminClientManager();

    private BandAdminClientManager()
    {
    }

    public static IBandAdminClientManager Instance => (IBandAdminClientManager) BandAdminClientManager.ConcreteInstance;

    internal static BandAdminClientManager ConcreteInstance => BandAdminClientManager.instance;

    public async Task<IBandInfo[]> GetBandsAsync() => await Task.Run<IBandInfo[]>((Func<IBandInfo[]>) (() => this.GetBands()));

    public IBandInfo[] GetBands() => (IBandInfo[]) this.ConcreteGetBands();

    public async Task<ICargoClient> ConnectAsync(IBandInfo bandInfo) => await Task.Run<ICargoClient>((Func<ICargoClient>) (() => this.Connect(bandInfo)));

    public ICargoClient Connect(IBandInfo bandInfo) => (ICargoClient) this.ConcreteConnect(bandInfo);

    public async Task<ICargoClient> ConnectAsync(ServiceInfo serviceInfo) => await Task.Run<ICargoClient>((Func<ICargoClient>) (() => this.Connect(serviceInfo)));

    public ICargoClient Connect(ServiceInfo serviceInfo) => (ICargoClient) this.ConcreteConnect(serviceInfo);

    public async Task<ICargoClient> ConnectAsync(IBandInfo bandInfo, string userId) => await Task.Run<ICargoClient>((Func<ICargoClient>) (() => this.Connect(bandInfo, userId)));

    public ICargoClient Connect(IBandInfo bandInfo, string userId) => (ICargoClient) this.ConcreteConnect(bandInfo, userId);

    public async Task<ICargoClient> ConnectAsync(
      string bandId,
      ServiceInfo serviceInfo)
    {
      return await Task.Run<ICargoClient>((Func<ICargoClient>) (() => this.Connect(bandId, serviceInfo)));
    }

    public ICargoClient Connect(string bandId, ServiceInfo serviceInfo) => (ICargoClient) this.ConcreteConnect(bandId, serviceInfo);

    public async Task<ICargoClient> ConnectAsync(
      IBandInfo bandInfo,
      ServiceInfo serviceInfo)
    {
      return await Task.Run<ICargoClient>((Func<ICargoClient>) (() => this.Connect(bandInfo, serviceInfo)));
    }

    public ICargoClient Connect(IBandInfo bandInfo, ServiceInfo serviceInfo) => (ICargoClient) this.ConcreteConnect(bandInfo, serviceInfo);

    internal async Task<UsbDeviceInfo[]> ConcreteGetBandsAsync() => await Task.Run<UsbDeviceInfo[]>((Func<UsbDeviceInfo[]>) (() => this.ConcreteGetBands()));

    internal UsbDeviceInfo[] ConcreteGetBands() => UsbTransport.GetConnectedDevices();

    internal async Task<CargoClient> ConcreteConnectAsync(IBandInfo bandInfo) => await Task.Run<CargoClient>((Func<CargoClient>) (() => this.ConcreteConnect(bandInfo)));

    internal CargoClient ConcreteConnect(IBandInfo bandInfo)
    {
      UsbTransport usbTransport = (UsbTransport) null;
      CargoClient cargoClient = (CargoClient) null;
      try
      {
        LoggerProvider loggerProvider = new LoggerProvider();
        DesktopProvider desktopProvider = new DesktopProvider();
        usbTransport = UsbTransport.Create(bandInfo, (ILoggerProvider) loggerProvider);
        cargoClient = new CargoClient((IDeviceTransport) usbTransport, (CloudProvider) null, (ILoggerProvider) loggerProvider, (IPlatformProvider) desktopProvider, DesktopApplicationPlatformProvider.Current);
        cargoClient.InitializeCachedProperties();
        loggerProvider.Log(ProviderLogLevel.Info, "Created BandAdminClient(IBandInfo bandinfo)", new object[0]);
      }
      catch
      {
        if (cargoClient != null)
          cargoClient.Dispose();
        else
          usbTransport?.Dispose();
        throw;
      }
      return cargoClient;
    }

    internal async Task<CargoClient> ConcreteConnectAsync(ServiceInfo serviceInfo) => await Task.Run<CargoClient>((Func<CargoClient>) (() => this.ConcreteConnect(serviceInfo)));

    internal CargoClient ConcreteConnect(ServiceInfo serviceInfo)
    {
      LoggerProvider loggerProvider = new LoggerProvider();
      DesktopProvider desktopProvider = new DesktopProvider();
      CloudProvider cloudProvider = new CloudProvider(serviceInfo);
      CargoClient cargoClient = new CargoClient((IDeviceTransport) null, cloudProvider, (ILoggerProvider) loggerProvider, (IPlatformProvider) desktopProvider, DesktopApplicationPlatformProvider.Current);
      cloudProvider.SetUserAgent(desktopProvider.GetDefaultUserAgent((FirmwareVersions) null), false);
      loggerProvider.Log(ProviderLogLevel.Info, "Created Phone BandAdminClient(ServiceInfo serviceInfo)", new object[0]);
      return cargoClient;
    }

    internal async Task<CargoClient> ConcreteConnectAsync(
      IBandInfo bandInfo,
      string userId)
    {
      return await Task.Run<CargoClient>((Func<CargoClient>) (() => this.ConcreteConnect(bandInfo, userId)));
    }

    internal CargoClient ConcreteConnect(IBandInfo bandInfo, string userId)
    {
      UsbTransport usbTransport = (UsbTransport) null;
      CargoClient cargoClient = (CargoClient) null;
      try
      {
        LoggerProvider loggerProvider = new LoggerProvider();
        DesktopProvider desktopProvider = new DesktopProvider();
        usbTransport = UsbTransport.Create(bandInfo, (ILoggerProvider) loggerProvider);
        cargoClient = new CargoClient((IDeviceTransport) usbTransport, (CloudProvider) null, (ILoggerProvider) loggerProvider, (IPlatformProvider) desktopProvider, DesktopApplicationPlatformProvider.Current);
        cargoClient.InitializeCachedProperties();
        StorageProvider storageProvider = StorageProvider.Create(userId, cargoClient.DeviceUniqueId.ToString("N"));
        cargoClient.InitializeStorageProvider((IStorageProvider) storageProvider);
        loggerProvider.Log(ProviderLogLevel.Info, "Created BandAdminClient(IBandInfo bandinfo, string userId)", new object[0]);
      }
      catch
      {
        if (cargoClient != null)
          cargoClient.Dispose();
        else
          usbTransport?.Dispose();
        throw;
      }
      return cargoClient;
    }

    internal async Task<CargoClient> ConcreteConnectAsync(
      string bandId,
      ServiceInfo serviceInfo)
    {
      return await Task.Run<CargoClient>((Func<CargoClient>) (() => this.ConcreteConnect(bandId, serviceInfo)));
    }

    internal CargoClient ConcreteConnect(string bandId, ServiceInfo serviceInfo)
    {
      LoggerProvider loggerProvider = new LoggerProvider();
      DesktopProvider desktopProvider = new DesktopProvider();
      CloudProvider cloudProvider = new CloudProvider(serviceInfo);
      CargoClient cargoClient = new CargoClient((IDeviceTransport) null, cloudProvider, (ILoggerProvider) loggerProvider, (IPlatformProvider) desktopProvider, DesktopApplicationPlatformProvider.Current);
      cargoClient.DeviceUniqueId = Guid.Parse(bandId);
      cargoClient.SerialNumber = (string) null;
      cloudProvider.SetUserAgent(desktopProvider.GetDefaultUserAgent((FirmwareVersions) null), false);
      StorageProvider storageProvider = StorageProvider.Create(serviceInfo.UserId, cargoClient.DeviceUniqueId.ToString("N"));
      cargoClient.InitializeStorageProvider((IStorageProvider) storageProvider);
      loggerProvider.Log(ProviderLogLevel.Info, "Created BandAdminClient(string bandId, ServiceInfo serviceInfo)", new object[0]);
      return cargoClient;
    }

    internal async Task<CargoClient> ConcreteConnectAsync(
      IBandInfo bandInfo,
      ServiceInfo serviceInfo)
    {
      return await Task.Run<CargoClient>((Func<CargoClient>) (() => this.ConcreteConnect(bandInfo, serviceInfo)));
    }

    internal CargoClient ConcreteConnect(IBandInfo bandInfo, ServiceInfo serviceInfo)
    {
      UsbTransport usbTransport = (UsbTransport) null;
      CargoClient cargoClient = (CargoClient) null;
      try
      {
        LoggerProvider loggerProvider = new LoggerProvider();
        DesktopProvider desktopProvider = new DesktopProvider();
        usbTransport = UsbTransport.Create(bandInfo, (ILoggerProvider) loggerProvider);
        CloudProvider cloudProvider = new CloudProvider(serviceInfo);
        cargoClient = new CargoClient((IDeviceTransport) usbTransport, cloudProvider, (ILoggerProvider) loggerProvider, (IPlatformProvider) desktopProvider, DesktopApplicationPlatformProvider.Current);
        cargoClient.InitializeCachedProperties();
        cloudProvider.SetUserAgent(desktopProvider.GetDefaultUserAgent(cargoClient.FirmwareVersions), false);
        StorageProvider storageProvider = StorageProvider.Create(serviceInfo.UserId, cargoClient.DeviceUniqueId.ToString("N"));
        cargoClient.InitializeStorageProvider((IStorageProvider) storageProvider);
        loggerProvider.Log(ProviderLogLevel.Info, "Created BandAdminClient(IBandInfo bandinfo, ServiceInfo serviceInfo)", new object[0]);
      }
      catch
      {
        if (cargoClient != null)
          cargoClient.Dispose();
        else
          usbTransport?.Dispose();
        throw;
      }
      return cargoClient;
    }
  }
}
