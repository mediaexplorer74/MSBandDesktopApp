// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CargoTimeZoneInfo
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public class CargoTimeZoneInfo
  {
    private string name;
    private static readonly int serializedByteCount = 64 + CargoSystemTime.GetSerializedByteCount() + CargoSystemTime.GetSerializedByteCount();

    public string Name
    {
      get => this.name;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (Name));
        if (value.Length > 30)
        {
          Logger.Log(LogLevel.Warning, string.Format(BandResources.GenericLengthExceeded, new object[1]
          {
            (object) nameof (Name)
          }));
          this.name = value.Substring(0, 30);
        }
        else
          this.name = value;
      }
    }

    public short ZoneOffsetMinutes { get; set; }

    public short DaylightOffsetMinutes { get; set; }

    public CargoSystemTime StandardDate { get; set; }

    public CargoSystemTime DaylightDate { get; set; }

    internal static int GetSerializedByteCount() => CargoTimeZoneInfo.serializedByteCount;

    internal static CargoTimeZoneInfo DeserializeFromBand(ICargoReader reader) => new CargoTimeZoneInfo()
    {
      name = reader.ReadString(30),
      ZoneOffsetMinutes = reader.ReadInt16(),
      DaylightOffsetMinutes = reader.ReadInt16(),
      StandardDate = CargoSystemTime.DeserializeFromBand(reader),
      DaylightDate = CargoSystemTime.DeserializeFromBand(reader)
    };

    internal void SerializeToBand(ICargoWriter writer)
    {
      writer.WriteStringWithPadding(this.Name ?? "", 30);
      writer.WriteInt16(this.ZoneOffsetMinutes);
      writer.WriteInt16(this.DaylightOffsetMinutes);
      this.StandardDate.SerializeToBand(writer);
      this.DaylightDate.SerializeToBand(writer);
    }
  }
}
