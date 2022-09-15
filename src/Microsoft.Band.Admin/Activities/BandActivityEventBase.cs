// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Activities.BandActivityEventBase
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin.Activities
{
  internal abstract class BandActivityEventBase
  {
    private static readonly int TileFriendlyNameSize = 60;
    private static readonly int ReservedBlockSize = 8;
    private static readonly int serializedByteCountMinusContext = 20 + BandActivityEventBase.TileFriendlyNameSize + CargoFileTime.GetSerializedByteCount() + BandActivityEventBase.ReservedBlockSize;
    private static readonly int serializedByteCount = 4 + BandActivityEventBase.serializedByteCountMinusContext;

    public Guid TileId { get; private set; }

    public DateTimeOffset Timestamp { get; private set; }

    public static int GetSerializedByteCount() => BandActivityEventBase.serializedByteCount;

    internal static BandActivityEventBase DeserializeFromBand(
      ICargoReader reader)
    {
      BandActivityEventBase emptyActivityEvent = BandActivityEventBase.GetEmptyActivityEvent(reader);
      if (emptyActivityEvent != null)
      {
        reader.ReadExactAndDiscard(4);
        emptyActivityEvent.TileId = reader.ReadGuid();
        reader.ReadExactAndDiscard(BandActivityEventBase.TileFriendlyNameSize);
        emptyActivityEvent.Timestamp = CargoFileTime.DeserializeFromBandAsDateTimeOffset(reader);
        reader.ReadExactAndDiscard(BandActivityEventBase.ReservedBlockSize);
      }
      else
        reader.ReadExactAndDiscard(BandActivityEventBase.serializedByteCountMinusContext);
      return emptyActivityEvent;
    }

    private static BandActivityEventBase GetEmptyActivityEvent(
      ICargoReader reader)
    {
      switch ((ActivityEventContext) reader.ReadUInt32())
      {
        case ActivityEventContext.ActivityStart:
          return (BandActivityEventBase) new BandActivityStartedEvent();
        case ActivityEventContext.ActivityEnd:
          return (BandActivityEventBase) new BandActivityEndedEvent();
        default:
          return (BandActivityEventBase) null;
      }
    }
  }
}
