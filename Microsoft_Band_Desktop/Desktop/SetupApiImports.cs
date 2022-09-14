// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.SetupApiImports
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Band.Desktop
{
  internal static class SetupApiImports
  {
    public static readonly uint SP_DEVICE_INTERFACE_DETAIL_DATA_cbSize = (uint) (4 + Marshal.SystemDefaultCharSize);

    static SetupApiImports()
    {
      if (IntPtr.Size != 8)
        return;
      SetupApiImports.SP_DEVICE_INTERFACE_DETAIL_DATA_cbSize += 8U - SetupApiImports.SP_DEVICE_INTERFACE_DETAIL_DATA_cbSize;
    }

    public static class NativeMethods
    {
      [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern IntPtr SetupDiGetClassDevs(
        ref Guid ClassGuid,
        IntPtr Enumerator,
        IntPtr hwndParent,
        DiGetClassFlags Flags);

      [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern bool SetupDiEnumDeviceInterfaces(
        IntPtr hDevInfo,
        IntPtr devInfo,
        ref Guid interfaceClassGuid,
        uint memberIndex,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

      [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern bool SetupDiGetDeviceInterfaceDetail(
        IntPtr hDevInfo,
        ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
        ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
        uint deviceInterfaceDetailDataSize,
        IntPtr requiredSize,
        ref SP_DEVINFO_DATA deviceInfoData);

      [DllImport("setupapi.dll", SetLastError = true)]
      public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);
    }
  }
}
