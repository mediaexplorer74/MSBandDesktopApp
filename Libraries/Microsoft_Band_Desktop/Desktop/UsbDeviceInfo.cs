// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.UsbDeviceInfo
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Security;

namespace Microsoft.Band.Desktop
{
  internal sealed class UsbDeviceInfo : IBandInfo
  {
    public DEVICE_DATA DeviceData;
    public PIPE_ID PipeID;

    public string Name { get; private set; }

    public Guid Id { get; private set; }

    public BandConnectionType ConnectionType => BandConnectionType.Usb;

    public bool Connected { get; set; }

    [SecurityCritical]
    public UsbDeviceInfo(string name, Guid id)
    {
      this.Name = name;
      this.Id = id;
      this.Connected = false;
      this.DeviceData = new DEVICE_DATA();
      this.DeviceData.DevicePath = name;
      this.PipeID = new PIPE_ID();
    }
  }
}
