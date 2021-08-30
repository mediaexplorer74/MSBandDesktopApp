// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.SP_DEVICE_INTERFACE_DETAIL_DATA
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Desktop
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
  {
    public uint cbSize;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string DevicePath;
  }
}
