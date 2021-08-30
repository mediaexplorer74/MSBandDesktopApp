// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Devices.PedometerDevice
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Pedometer;
using Microsoft.Health.App.Core.Services.Sync;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Devices
{
  public sealed class PedometerDevice : IPedometerDevice, IDevice
  {
    private readonly IPedometerSyncManager pedometerSyncManager;
    private readonly IPedometerManager pedometerManager;

    public PedometerDevice(
      IPedometerSyncManager pedometerSyncManager,
      IPedometerManager pedometerManager)
    {
      Assert.ParamIsNotNull((object) pedometerSyncManager, nameof (pedometerSyncManager));
      Assert.ParamIsNotNull((object) pedometerManager, nameof (pedometerManager));
      this.pedometerSyncManager = pedometerSyncManager;
      this.pedometerManager = pedometerManager;
    }

    public Task<bool> CanSetIsEnabledAsync(CancellationToken token) => this.pedometerManager.CanSetIsEnabledAsync(token);

    public async Task SetIsEnabledAsync(bool enable, CancellationToken token)
    {
      if (!await this.CanSetIsEnabledAsync(token).ConfigureAwait(false))
        throw new InvalidOperationException("The pedometer settings cannot be changed on this device.");
      await this.pedometerManager.SetIsEnabledAsync(enable, token).ConfigureAwait(false);
      if (!enable)
        return;
      await this.pedometerSyncManager.SetSyncEnabledTimeAsync(token).ConfigureAwait(false);
    }

    public Task<bool> IsEnabledAsync(CancellationToken token) => this.pedometerManager.IsEnabledAsync(token);

    public Task LaunchSettingsAsync(CancellationToken token) => this.pedometerManager.LaunchSettingsAsync(token);

    public async Task<int> GetVersionAsync(CancellationToken token) => (await this.pedometerManager.GetVersionAsync(token).ConfigureAwait(false)).Value;

    public DeviceType DeviceType => DeviceType.Phone;

    public async Task<DateTimeOffset?> GetLastSyncTimeAsync(
      CancellationToken token)
    {
      return await this.IsEnabledAsync(token) ? new DateTimeOffset?(await this.pedometerSyncManager.GetLastSyncTimeAsync(token)) : new DateTimeOffset?();
    }

    public async Task<bool> CanSyncAsync(CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.pedometerManager.IsAvailableAsync(cancellationToken).ConfigureAwait(false);
      bool flag = await configuredTaskAwaitable;
      if (flag)
      {
        configuredTaskAwaitable = this.IsEnabledAsync(cancellationToken).ConfigureAwait(false);
        flag = await configuredTaskAwaitable;
      }
      return flag;
    }

    public Task SyncDeviceAsync(
      SyncType syncType,
      CancellationToken token,
      bool ignoreIfUnable = false,
      IProgress<DeviceSyncProgress> progress = null)
    {
      using (ApplicationTelemetry.TimePhoneSensorSync())
        return this.pedometerSyncManager.SyncWithDeviceAsync(token, progress);
    }
  }
}
