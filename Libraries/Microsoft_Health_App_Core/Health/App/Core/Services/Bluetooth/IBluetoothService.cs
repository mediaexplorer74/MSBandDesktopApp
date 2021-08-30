// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Bluetooth.IBluetoothService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Bluetooth
{
  public interface IBluetoothService
  {
    event EventHandler<BandDeviceStateChangedEventArgs> BluetoothDeviceStateChanged;

    event EventHandler<BluetoothDiscoveryStateChangedEventArgs> BluetoothDiscoveryStateChanged;

    bool SupportsContinuousDiscovery { get; }

    bool CanEnableBluetoothWithoutSystemPrompt { get; }

    Task ShutdownAsync(CancellationToken cancellationToken);

    Task<bool> IsBluetoothEnabledAsync(CancellationToken cancellationToken);

    Task EnableBluetoothAsync(CancellationToken cancellationToken);

    Task EnableDiscoveryAsync(CancellationToken cancellationToken);

    Task DisableDiscoveryAsync(CancellationToken cancellationToken);

    Task<IBandInfo[]> GetPairedBandsAsync(CancellationToken cancellationToken);
  }
}
