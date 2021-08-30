// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.WinUsbImports
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Band.Desktop
{
  internal class WinUsbImports
  {
    public const uint AUTO_CLEAR_STALL = 2;
    public const byte EnableAutoClearStall = 1;
    public const uint USB_ENDPOINT_DIRECTION_MASK = 128;
    public const uint PIPE_TRANSFER_TIMEOUT = 3;

    public static bool USB_ENDPOINT_DIRECTION_OUT(uint addr) => ((int) addr & 128) == 0;

    public static bool USB_ENDPOINT_DIRECTION_IN(uint addr) => ((int) addr & 128) == 128;

    public static class NativeMethods
    {
      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_Initialize(IntPtr DeviceHandle, out IntPtr InterfaceHandle);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_Free(IntPtr InterfaceHandle);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_QueryInterfaceSettings(
        IntPtr InterfaceHandle,
        byte AlternateSettingNumber,
        out USB_INTERFACE_DESCRIPTOR UsbAltInterfaceDescriptor);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_QueryPipe(
        IntPtr InterfaceHandle,
        byte AlternateSettingNumber,
        int PipeIndex,
        out WINUSB_PIPE_INFORMATION PipeInformation);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_WritePipe(
        IntPtr InterfaceHandle,
        byte PipeID,
        IntPtr Buffer,
        uint BufferLength,
        ref uint LengthTransferred,
        IntPtr Overlapped);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_ReadPipe(
        IntPtr InterfaceHandle,
        byte PipeID,
        IntPtr Buffer,
        uint BufferLength,
        ref uint LengthTransferred,
        IntPtr Overlapped);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_ResetPipe(IntPtr InterfaceHandle, byte PipeID);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_AbortPipe(IntPtr InterfaceHandle, byte PipeID);

      [DllImport("winusb.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool WinUsb_SetPipePolicy(
        IntPtr InterfaceHandle,
        byte PipeID,
        uint PolicyType,
        uint ValueLength,
        IntPtr Value);
    }
  }
}
