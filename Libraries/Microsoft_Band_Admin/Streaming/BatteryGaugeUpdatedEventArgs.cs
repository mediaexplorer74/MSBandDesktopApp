// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Streaming.BatteryGaugeUpdatedEventArgs
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin.Streaming
{
  public sealed class BatteryGaugeUpdatedEventArgs : EventArgs
  {
    private const int serializedByteCount = 5;

    private BatteryGaugeUpdatedEventArgs()
    {
    }

    public byte PercentCharge { get; private set; }

    public ushort FilteredVoltage { get; private set; }

    public ushort BatteryGaugeAlerts { get; private set; }

    internal static int GetSerializedByteCount() => 5;

    internal static BatteryGaugeUpdatedEventArgs DeserializeFromBand(
      ICargoReader reader)
    {
      return new BatteryGaugeUpdatedEventArgs()
      {
        PercentCharge = reader.ReadByte(),
        FilteredVoltage = reader.ReadUInt16(),
        BatteryGaugeAlerts = reader.ReadUInt16()
      };
    }
  }
}
