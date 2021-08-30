// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.DEVICE_DATA
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Desktop
{
  internal struct DEVICE_DATA
  {
    public SafeUsbHandle WinusbHandle;
    public SafeFileHandle DeviceHandle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string DevicePath;
  }
}
