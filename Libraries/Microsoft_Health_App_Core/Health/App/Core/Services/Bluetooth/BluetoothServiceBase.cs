// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Bluetooth.BluetoothServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Bluetooth
{
  public abstract class BluetoothServiceBase : IBluetoothService
  {
    public abstract bool CanEnableBluetoothWithoutSystemPrompt { get; }

    public abstract bool SupportsContinuousDiscovery { get; }

    public event EventHandler<BandDeviceStateChangedEventArgs> BluetoothDeviceStateChanged;

    public event EventHandler<BluetoothDiscoveryStateChangedEventArgs> BluetoothDiscoveryStateChanged;

    public abstract Task DisableDiscoveryAsync(CancellationToken cancellationToken);

    public abstract Task EnableBluetoothAsync(CancellationToken cancellationToken);

    public abstract Task EnableDiscoveryAsync(CancellationToken cancellationToken);

    public async Task<IBandInfo[]> GetPairedBandsAsync(
      CancellationToken cancellationToken)
    {
      if (!await this.IsBluetoothEnabledAsync(cancellationToken).ConfigureAwait(false))
        throw new BluetoothOffException("Bluetooth is turned off");
      return await this.GetPairedBandsInternalAsync(cancellationToken);
    }

    public abstract Task<bool> IsBluetoothEnabledAsync(CancellationToken cancellationToken);

    public abstract Task ShutdownAsync(CancellationToken cancellationToken);

    protected abstract Task<IBandInfo[]> GetPairedBandsInternalAsync(
      CancellationToken cancellationToken);

    protected void OnBluetoothDeviceStateChanged(BandDeviceStateChangedEventArgs e)
    {
      Assert.ParamIsNotNull((object) e, nameof (e));
      EventHandler<BandDeviceStateChangedEventArgs> deviceStateChanged = this.BluetoothDeviceStateChanged;
      if (deviceStateChanged == null)
        return;
      deviceStateChanged((object) this, e);
    }

    protected void OnBluetoothDiscoveryStateChanged(BluetoothDiscoveryStateChangedEventArgs e)
    {
      Assert.ParamIsNotNull((object) e, nameof (e));
      EventHandler<BluetoothDiscoveryStateChangedEventArgs> discoveryStateChanged = this.BluetoothDiscoveryStateChanged;
      if (discoveryStateChanged == null)
        return;
      discoveryStateChanged((object) this, e);
    }
  }
}
