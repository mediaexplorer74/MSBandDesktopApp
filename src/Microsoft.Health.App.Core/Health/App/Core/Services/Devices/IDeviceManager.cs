// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Devices.IDeviceManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.ForegroundSync;
using Microsoft.Health.App.Core.Services.Sync;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Devices
{
  public interface IDeviceManager
  {
    Task<IGetDevicesResult> GetDevicesAsync(CancellationToken cancellationToken);

    Task SyncDevicesAsync(
      SyncType syncType,
      CancellationToken cancellationToken,
      bool ignoreIfUnable = false,
      IProgress<DeviceSyncProgress> progress = null);

    void CancelDevicesSync();

    event EventHandler<SyncStateChangedEventArgs> SyncStateChanged;

    event EventHandler DevicesChanged;

    bool IsSyncing { get; }

    SyncStateChangedEventArgs LastReportedSyncState { get; }
  }
}
