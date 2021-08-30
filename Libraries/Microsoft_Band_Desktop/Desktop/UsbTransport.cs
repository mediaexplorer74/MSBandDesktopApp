// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.UsbTransport
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.Band.Desktop
{
  internal sealed class UsbTransport : UsbTransportBase
  {
    private UsbDeviceInfo UsbDevice;
    private ILoggerProvider loggerProvider;

    private UsbTransport(UsbDeviceInfo UsbDeviceInfo, ILoggerProvider loggerProvider)
    {
      this.UsbDevice = UsbDeviceInfo;
      this.loggerProvider = loggerProvider;
      this.UsbDevice.Connected = false;
    }

    internal static UsbDeviceInfo[] GetConnectedDevices() => UsbTransport.EnumerateConnectedDevices().ToArray<UsbDeviceInfo>();

    private static IEnumerable<UsbDeviceInfo> EnumerateConnectedDevices()
    {
      Guid[] guidArray = BandConstants.UsbInterfaceGuids;
      for (int index = 0; index < guidArray.Length; ++index)
      {
        foreach (UsbDeviceInfo enumerateConnectedDevice in UsbTransport.EnumerateConnectedDevices(guidArray[index]))
          yield return enumerateConnectedDevice;
      }
      guidArray = (Guid[]) null;
    }

    private static IEnumerable<UsbDeviceInfo> EnumerateConnectedDevices(
      Guid interfaceGuid)
    {
      using (SafeDeviceInfoListHandle classDevicesHandle = new SafeDeviceInfoListHandle(SetupApiImports.NativeMethods.SetupDiGetClassDevs(ref interfaceGuid, IntPtr.Zero, IntPtr.Zero, DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE)))
      {
        if (classDevicesHandle.IsInvalid)
          throw new Win32Exception(DesktopResources.SetupDiGetClassDevsFailed);
        SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA()
        {
          cbSize = (uint) Marshal.SizeOf(typeof (SP_DEVICE_INTERFACE_DATA))
        };
        SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA()
        {
          cbSize = (uint) Marshal.SizeOf(typeof (SP_DEVINFO_DATA))
        };
        SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = new SP_DEVICE_INTERFACE_DETAIL_DATA()
        {
          cbSize = SetupApiImports.SP_DEVICE_INTERFACE_DETAIL_DATA_cbSize
        };
        for (uint interfaceIndex = 0; SetupApiImports.NativeMethods.SetupDiEnumDeviceInterfaces(classDevicesHandle.Handle, IntPtr.Zero, ref interfaceGuid, interfaceIndex, ref deviceInterfaceData); ++interfaceIndex)
        {
          if (!SetupApiImports.NativeMethods.SetupDiGetDeviceInterfaceDetail(classDevicesHandle.Handle, ref deviceInterfaceData, ref deviceInterfaceDetailData, (uint) Marshal.SizeOf(typeof (SP_DEVICE_INTERFACE_DETAIL_DATA)), IntPtr.Zero, ref deviceInfoData))
            throw new Win32Exception(DesktopResources.SetupDiGetDeviceInterfaceDetailFailed);
          yield return new UsbDeviceInfo(deviceInterfaceDetailData.DevicePath, interfaceGuid);
        }
        if (Marshal.GetLastWin32Error() != 259)
          throw new Win32Exception(DesktopResources.SetupDiEnumDeviceInterfacesFailed);
      }
    }

    internal static Task<UsbTransport> CreateAsync(
      IBandInfo deviceInfo,
      ILoggerProvider loggerProvider)
    {
      return Task.Run<UsbTransport>((Func<UsbTransport>) (() => UsbTransport.Create(deviceInfo, loggerProvider)));
    }

    internal static UsbTransport Create(
      IBandInfo deviceInfo,
      ILoggerProvider loggerProvider)
    {
      if (deviceInfo == null)
        throw new ArgumentNullException(nameof (deviceInfo));
      UsbTransport usbTransport = deviceInfo is UsbDeviceInfo ? new UsbTransport((UsbDeviceInfo) deviceInfo, loggerProvider) : throw new ArgumentException("BandInfo must be for a USB connection.", nameof (deviceInfo));
      usbTransport.Connect((ushort) 1);
      return usbTransport;
    }

    private static bool QueryDeviceEndpoints(SafeUsbHandle WinUsbHandle, ref PIPE_ID pipeid)
    {
      USB_INTERFACE_DESCRIPTOR UsbAltInterfaceDescriptor;
      bool flag = WinUsbImports.NativeMethods.WinUsb_QueryInterfaceSettings(WinUsbHandle.Handle, (byte) 0, out UsbAltInterfaceDescriptor);
      if (flag)
      {
        for (int PipeIndex = 0; PipeIndex < (int) UsbAltInterfaceDescriptor.bNumEndpoints; ++PipeIndex)
        {
          WINUSB_PIPE_INFORMATION PipeInformation;
          flag = WinUsbImports.NativeMethods.WinUsb_QueryPipe(WinUsbHandle.Handle, (byte) 0, PipeIndex, out PipeInformation);
          if (flag && PipeInformation.PipeType == USBD_PIPE_TYPE.UsbdPipeTypeBulk)
          {
            if (WinUsbImports.USB_ENDPOINT_DIRECTION_IN((uint) PipeInformation.PipeId))
              pipeid.PipeInId = PipeInformation.PipeId;
            if (WinUsbImports.USB_ENDPOINT_DIRECTION_OUT((uint) PipeInformation.PipeId))
              pipeid.PipeOutId = PipeInformation.PipeId;
          }
        }
      }
      return flag;
    }

    public override BandConnectionType ConnectionType => BandConnectionType.Usb;

    public override bool IsConnected => this.UsbDevice.Connected;

    public override void Connect(ushort maxConnectAttempts = 1)
    {
      this.CheckIfDisposed();
      if (this.IsConnected)
        return;
      try
      {
        SafeFileHandle safeFileHandle = new SafeFileHandle(Kernel32Imports.NativeMethods.CreateFile(this.UsbDevice.DeviceData.DevicePath, EFileAccess.GenericRead | EFileAccess.GenericWrite, EFileShare.Read | EFileShare.Write, IntPtr.Zero, ECreationDisposition.OpenExisting, EFileAttributes.Normal | EFileAttributes.Overlapped, IntPtr.Zero));
        if (safeFileHandle.IsInvalid)
        {
          safeFileHandle.Dispose();
          throw new Win32Exception(DesktopResources.CreateFileFailed);
        }
        IntPtr InterfaceHandle;
        if (!WinUsbImports.NativeMethods.WinUsb_Initialize(safeFileHandle.Handle, out InterfaceHandle))
        {
          safeFileHandle.Dispose();
          throw new Win32Exception(DesktopResources.WinUsbInitializedFailed);
        }
        this.UsbDevice.DeviceData.DeviceHandle = safeFileHandle;
        this.UsbDevice.DeviceData.WinusbHandle = new SafeUsbHandle(InterfaceHandle);
        this.UsbDevice.Connected = UsbTransport.QueryDeviceEndpoints(this.UsbDevice.DeviceData.WinusbHandle, ref this.UsbDevice.PipeID);
      }
      catch (Exception ex)
      {
        if (this.UsbDevice.DeviceData.WinusbHandle != null)
        {
          this.UsbDevice.DeviceData.WinusbHandle.Dispose();
          this.UsbDevice.DeviceData.WinusbHandle = (SafeUsbHandle) null;
        }
        if (this.UsbDevice.DeviceData.DeviceHandle != null)
        {
          this.UsbDevice.DeviceData.DeviceHandle.Dispose();
          this.UsbDevice.DeviceData.DeviceHandle = (SafeFileHandle) null;
        }
        throw new BandIOException(BandResources.ConnectionAttemptFailed, ex);
      }
      this.cargoStream = (ICargoStream) new WinUsbStream(this.UsbDevice);
      this.cargoReader = new CargoStreamReader(this.cargoStream, BufferServer.Pool_8192);
      this.cargoWriter = new CargoStreamWriter(this.cargoStream, this.loggerProvider, BufferServer.Pool_8192);
    }

    public override void Disconnect()
    {
      if (this.IsConnected)
      {
        this.UsbDevice.Connected = false;
        this.cargoReader.Dispose();
        this.cargoWriter.Dispose();
        this.cargoStream.Dispose();
        this.cargoReader = (CargoStreamReader) null;
        this.cargoWriter = (CargoStreamWriter) null;
        this.cargoStream = (ICargoStream) null;
      }
      this.RaiseDisconnectedEvent(new TransportDisconnectedEventArgs(TransportDisconnectedReason.DisconnectCall));
    }

    protected override void CheckIfDisposed()
    {
      if (this.isDisposed)
        throw new ObjectDisposedException(nameof (UsbTransport));
    }

    protected override void CheckIfDisconnected()
    {
      if (!this.IsConnected)
        throw new InvalidOperationException("Transport not connected");
    }
  }
}
