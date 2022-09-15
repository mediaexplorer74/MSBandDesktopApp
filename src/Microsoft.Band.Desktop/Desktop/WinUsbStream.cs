// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Desktop.WinUsbStream
// Assembly: Microsoft.Band.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 4E5547A6-750A-4477-BF88-BDD5622B3C30
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Desktop.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Band.Desktop
{
  internal class WinUsbStream : ICargoStream, IDisposable
  {
    private UsbDeviceInfo usbDevice;
    private bool unStallPending;
    private int readTimeout;
    private int writeTimeout;
    private ManualResetEvent overlappedEvent;
    private CancellationToken cancel;
    private NativeOverlapped overlapped;
    private WaitHandle[] readWaitHandles;

    internal WinUsbStream(UsbDeviceInfo usbDevice)
    {
      this.usbDevice = usbDevice != null ? usbDevice : throw new ArgumentNullException(nameof (usbDevice));
      this.overlappedEvent = new ManualResetEvent(false);
      this.readWaitHandles = new WaitHandle[2]
      {
        (WaitHandle) this.overlappedEvent,
        this.cancel.WaitHandle
      };
      this.overlapped = new NativeOverlapped()
      {
        EventHandle = this.overlappedEvent.SafeWaitHandle.DangerousGetHandle()
      };
      this.unStallPending = true;
    }

    public int ReadTimeout
    {
      get => this.readTimeout;
      set => this.readTimeout = value >= 1 ? value : throw new ArgumentOutOfRangeException();
    }

    public int WriteTimeout
    {
      get => this.writeTimeout;
      set => this.writeTimeout = value >= 1 ? value : throw new ArgumentOutOfRangeException();
    }

    public CancellationToken Cancel
    {
      get => this.cancel;
      set
      {
        this.cancel = value;
        this.readWaitHandles[1] = value.WaitHandle;
      }
    }

    public int Read(byte[] buffer, int offset, int count)
    {
      this.CheckIsDisposedOrNotConnected();
      this.Cancel.ThrowIfCancellationRequested();
      this.UnstallIfNeeded();
      uint num1 = 0;
      using (WinUsbStream.BufferAndOverlappedPin andOverlappedPin = new WinUsbStream.BufferAndOverlappedPin(buffer, this.overlapped, offset))
      {
        this.overlappedEvent.Reset();
        bool flag = WinUsbImports.NativeMethods.WinUsb_ReadPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeInId, andOverlappedPin.BufferPtr, (uint) count, ref num1, andOverlappedPin.OverlappedPtr);
        if (!flag && Marshal.GetLastWin32Error() == 997)
        {
          int num2 = WaitHandle.WaitAny(this.readWaitHandles, this.ReadTimeout) == 258 ? 1 : 0;
          this.Cancel.ThrowIfCancellationRequested();
          if (num2 != 0)
          {
            WinUsbImports.NativeMethods.WinUsb_AbortPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeInId);
            this.unStallPending = true;
            throw new TimeoutException();
          }
          flag = Kernel32Imports.NativeMethods.GetOverlappedResult(this.usbDevice.DeviceData.WinusbHandle.Handle, andOverlappedPin.OverlappedPtr, ref num1, false);
        }
        if (!flag)
        {
          WinUsbImports.NativeMethods.WinUsb_AbortPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeInId);
          this.unStallPending = true;
          throw new Win32Exception(DesktopResources.WinUsbReadPipeFailed);
        }
      }
      return (int) num1;
    }

    public void Write(byte[] buffer, int offset, int count)
    {
      this.CheckIsDisposedOrNotConnected();
      this.UnstallIfNeeded();
      uint num1 = 0;
      using (WinUsbStream.BufferAndOverlappedPin andOverlappedPin = new WinUsbStream.BufferAndOverlappedPin(buffer, this.overlapped, offset))
      {
        this.overlappedEvent.Reset();
        while ((long) num1 < (long) count)
        {
          uint num2 = 0;
          bool flag = WinUsbImports.NativeMethods.WinUsb_WritePipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeOutId, andOverlappedPin.BufferPtr, (uint) count - num1, ref num2, andOverlappedPin.OverlappedPtr);
          if (!flag && Marshal.GetLastWin32Error() == 997)
          {
            if (!this.overlappedEvent.WaitOne(this.WriteTimeout))
            {
              WinUsbImports.NativeMethods.WinUsb_AbortPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeOutId);
              this.unStallPending = true;
              throw new TimeoutException();
            }
            flag = Kernel32Imports.NativeMethods.GetOverlappedResult(this.usbDevice.DeviceData.WinusbHandle.Handle, andOverlappedPin.OverlappedPtr, ref num2, false);
          }
          if (!flag)
          {
            this.unStallPending = true;
            throw new Win32Exception(DesktopResources.WinUsbWritePipeFailed);
          }
          num1 += num2;
          if ((long) num1 < (long) count)
            andOverlappedPin.IncrementBufferPtr((int) num1);
        }
      }
    }

    private void CheckIsDisposedOrNotConnected()
    {
      if (this.usbDevice == null)
        throw new ObjectDisposedException(nameof (WinUsbStream));
      if (!this.usbDevice.Connected)
        throw new IOException(DesktopResources.NotConnectedError);
    }

    private void UnstallIfNeeded()
    {
      if (!this.unStallPending)
        return;
      this.unStallPending = false;
      WinUsbImports.NativeMethods.WinUsb_ResetPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeInId);
      WinUsbImports.NativeMethods.WinUsb_ResetPipe(this.usbDevice.DeviceData.WinusbHandle.Handle, this.usbDevice.PipeID.PipeOutId);
    }

    public void Flush() => this.CheckIsDisposedOrNotConnected();

    public void Dispose()
    {
      if (this.usbDevice == null)
        return;
      this.UnstallIfNeeded();
      this.overlappedEvent.Dispose();
      this.usbDevice.DeviceData.WinusbHandle.Dispose();
      this.usbDevice.DeviceData.DeviceHandle.Dispose();
      this.usbDevice.Connected = false;
      this.usbDevice = (UsbDeviceInfo) null;
      this.overlappedEvent = (ManualResetEvent) null;
    }

    private class BufferAndOverlappedPin : IDisposable
    {
      private DisposableGCHandle bufferPin;
      private DisposableGCHandle overlappedPin;

      internal BufferAndOverlappedPin(
        byte[] buffer,
        NativeOverlapped overlapped,
        int initialOffset)
      {
        this.bufferPin = DisposableGCHandle.Alloc((object) buffer, GCHandleType.Pinned);
        this.overlappedPin = DisposableGCHandle.Alloc((object) overlapped, GCHandleType.Pinned);
        this.BufferPtr = this.bufferPin.AddrOfPinnedObject();
        this.OverlappedPtr = this.overlappedPin.AddrOfPinnedObject();
        if (initialOffset == 0)
          return;
        this.IncrementBufferPtr(initialOffset);
      }

      internal IntPtr BufferPtr { get; private set; }

      internal IntPtr OverlappedPtr { get; private set; }

      internal void IncrementBufferPtr(int offset) => this.BufferPtr = IntPtr.Add(this.BufferPtr, offset);

      public void Dispose()
      {
        if (this.overlappedPin != null)
        {
          this.overlappedPin.Dispose();
          this.overlappedPin = (DisposableGCHandle) null;
        }
        if (this.bufferPin == null)
          return;
        this.bufferPin.Dispose();
        this.bufferPin = (DisposableGCHandle) null;
      }
    }

    private enum WaitHandleIndices
    {
      ReadWaitHandle,
      CancellationHandle,
    }
  }
}
