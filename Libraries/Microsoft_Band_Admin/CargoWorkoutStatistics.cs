// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CargoWorkoutStatistics
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public sealed class CargoWorkoutStatistics
  {
    private static readonly int serializedByteCount = CargoFileTime.GetSerializedByteCount() + 2 + 4 + 4 + 4 + 4 + CargoFileTime.GetSerializedByteCount() + 4;

    private CargoWorkoutStatistics()
    {
    }

    public DateTime Timestamp { get; private set; }

    public ushort Version { get; private set; }

    public uint Duration { get; private set; }

    public uint Calories { get; private set; }

    public uint AverageHeartrate { get; private set; }

    public uint MaximumHeartrate { get; private set; }

    public DateTime EndTime { get; private set; }

    public FeelingType Feeling { get; private set; }

    internal static int GetSerializedByteCount() => CargoWorkoutStatistics.serializedByteCount;

    internal static CargoWorkoutStatistics DeserializeFromBand(
      ICargoReader reader)
    {
      return new CargoWorkoutStatistics()
      {
        Timestamp = CargoFileTime.DeserializeFromBandAsDateTime(reader),
        Version = reader.ReadUInt16(),
        Duration = reader.ReadUInt32(),
        Calories = reader.ReadUInt32(),
        AverageHeartrate = reader.ReadUInt32(),
        MaximumHeartrate = reader.ReadUInt32(),
        EndTime = CargoFileTime.DeserializeFromBandAsDateTime(reader),
        Feeling = (FeelingType) reader.ReadUInt32()
      };
    }
  }
}
