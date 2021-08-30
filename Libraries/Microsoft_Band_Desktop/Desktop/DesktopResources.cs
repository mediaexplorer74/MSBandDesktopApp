// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.DesktopResources
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

namespace Microsoft.Band.Desktop
{
  internal class DesktopResources
  {
    internal static string CreateFileFailed => "CreateFile returned invalid usb device handle.";

    internal static string DeviceInfoNotUsb => "BandInfo must be for a USB device.";

    internal static string InvalidWinUsbHandle => "Invalid WinUsb Handle = {0}";

    internal static string NotConnectedError => "The target device is no longer connected.";

    internal static string SetupDiEnumDeviceInterfacesFailed => "SetupDiEnumDeviceInterfaces returned error result";

    internal static string SetupDiGetClassDevsFailed => "SetupDiGetClassDevs returned invalid class device handle.";

    internal static string SetupDiGetDeviceInterfaceDetailFailed => "SetupDiGetDeviceInterfaceDetail returned error result";

    internal static string WinUsbInitializedFailed => "WinUsb_Initialize failed.";

    internal static string WinUsbReadPipeFailed => "WinUsb_ReadPipe failed.";

    internal static string WinUsbWritePipeFailed => "WinUsb_WritePipe failed.";
  }
}
