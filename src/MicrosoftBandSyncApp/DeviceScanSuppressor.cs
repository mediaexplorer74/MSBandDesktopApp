// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DeviceScanSuppressor
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  internal class DeviceScanSuppressor : IDisposable
  {
    private KDeviceManager manager;

    public DeviceScanSuppressor(KDeviceManager manager)
    {
      this.manager = manager;
      manager.SuppressDeviceScan = true;
    }

    public void Dispose() => this.manager.SuppressDeviceScan = false;
  }
}
