// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.BandClientManager
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using Microsoft.Band.Desktop;
using System;
using System.Threading.Tasks;

namespace Microsoft.Band
{
  public sealed class BandClientManager : IBandClientManager
  {
    private static readonly BandClientManager instance = new BandClientManager();

    private BandClientManager()
    {
    }

    public static IBandClientManager Instance => (IBandClientManager) BandClientManager.instance;

    public Task<IBandInfo[]> GetBandsAsync() => this.GetBandsAsync(false);

    public Task<IBandInfo[]> GetBandsAsync(bool isBackground) => Task.Run<IBandInfo[]>((Func<IBandInfo[]>) (() => (IBandInfo[]) UsbTransport.GetConnectedDevices()));

    public Task<IBandClient> ConnectAsync(IBandInfo bandInfo)
    {
      if (bandInfo == null)
        throw new ArgumentNullException(nameof (bandInfo));
      if (!(bandInfo is UsbDeviceInfo deviceInfo))
        throw new ArgumentException(DesktopResources.DeviceInfoNotUsb);
      UsbTransport deviceTransport = (UsbTransport) null;
      BandDesktopClient bandDesktopClient = (BandDesktopClient) null;
      try
      {
        LoggerProviderStub loggerProviderStub = new LoggerProviderStub();
        deviceTransport = UsbTransport.Create((IBandInfo) deviceInfo, (ILoggerProvider) loggerProviderStub);
        bandDesktopClient = new BandDesktopClient(deviceInfo, deviceTransport, (ILoggerProvider) loggerProviderStub, DesktopApplicationPlatformProvider.Current);
        bandDesktopClient.InitializeCachedProperties();
        bandDesktopClient.CheckFirmwareSdkBit(FirmwareSdkCheckPlatform.Windows, (byte) 0);
        loggerProviderStub.Log(ProviderLogLevel.Info, "Created BandClient(IBandInfo bandinfo)", new object[0]);
      }
      catch
      {
        if (bandDesktopClient != null)
          bandDesktopClient.Dispose();
        else
          deviceTransport?.Dispose();
        throw;
      }
      return Task.FromResult<IBandClient>((IBandClient) bandDesktopClient);
    }
  }
}
