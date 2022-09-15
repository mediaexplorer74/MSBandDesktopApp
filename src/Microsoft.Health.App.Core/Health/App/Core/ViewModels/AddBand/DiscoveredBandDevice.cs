// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddBand.DiscoveredBandDevice
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services.Bluetooth;

namespace Microsoft.Health.App.Core.ViewModels.AddBand
{
  public sealed class DiscoveredBandDevice
  {
    public DiscoveredBandDevice(IBandDevice device, BluetoothConnectionState state)
    {
      Assert.ParamIsNotNull((object) device, nameof (device));
      Assert.EnumIsDefined<BluetoothConnectionState>(state, nameof (state));
      this.Device = device;
      this.Name = device.Name;
      this.State = state;
    }

    public IBandDevice Device { get; }

    public string Address => this.Device.Address.ToString();

    public string Name { get; set; }

    public BluetoothConnectionState State { get; set; }
  }
}
