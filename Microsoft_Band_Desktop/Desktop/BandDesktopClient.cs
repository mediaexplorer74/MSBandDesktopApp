// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.BandDesktopClient
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Threading;

namespace Microsoft.Band.Desktop
{
  internal class BandDesktopClient : BandClient
  {
    public EventHandler Disconnected;
    private readonly UsbDeviceInfo usbBand;

    protected UsbDeviceInfo UsbBand => this.usbBand;

    internal BandDesktopClient(
      UsbDeviceInfo deviceInfo,
      UsbTransport deviceTransport,
      ILoggerProvider loggerProvider,
      IApplicationPlatformProvider applicationPlatformProvider)
      : base((IDeviceTransport) deviceTransport, loggerProvider, applicationPlatformProvider)
    {
      this.usbBand = deviceInfo;
    }

    protected override void StreamBandData(ManualResetEvent started, CancellationToken stop) => throw new NotImplementedException();

    protected override void OnDisconnected(TransportDisconnectedEventArgs args)
    {
      if (args.Reason != TransportDisconnectedReason.TransportIssue)
        return;
      EventHandler disconnected = this.Disconnected;
      if (disconnected == null)
        return;
      disconnected((object) this, (EventArgs) args);
    }
  }
}
