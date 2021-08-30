// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Tiles.BandTileButtonPressedEvent
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Tiles
{
  internal class BandTileButtonPressedEvent : 
    BandTileEventBase,
    IBandTileButtonPressedEvent,
    IBandTileEvent
  {
    private const string PageIdName = "PageId";
    private const string ElementIdName = "ElementId";

    internal BandTileButtonPressedEvent(DateTimeOffset timestamp)
      : base(timestamp)
    {
    }

    public Guid PageId { get; private set; }

    public ushort ElementId { get; private set; }

    internal override void Dispatch(BandClient client) => client.DispatchTileButtonPressedEvent(this);

    protected override void DeserializeDataFromBand(ICargoReader reader)
    {
      this.TileId = reader.ReadGuid();
      this.PageId = reader.ReadGuid();
      this.ElementId = reader.ReadUInt16();
    }

    public BandTileButtonPressedEvent(IDictionary<string, object> valueSet)
      : base(valueSet)
    {
      this.PageId = (Guid) valueSet[nameof (PageId)];
      this.ElementId = (ushort) valueSet[nameof (ElementId)];
    }

    public override void EncodeToDictionary(IDictionary<string, object> valueSet)
    {
      valueSet["Type"] = (object) "TileButtonPressedEvent";
      valueSet["PageId"] = (object) this.PageId;
      valueSet["ElementId"] = (object) this.ElementId;
      base.EncodeToDictionary(valueSet);
    }
  }
}
