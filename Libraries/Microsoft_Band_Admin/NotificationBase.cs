// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.NotificationBase
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using Google.Protobuf;
using System;

namespace Microsoft.Band.Admin
{
  public abstract class NotificationBase
  {
    public NotificationBase(Guid tileId) => this.TileId = tileId;

    public Guid TileId { get; private set; }

    internal virtual int GetSerializedByteCount() => 16;

    internal virtual void SerializeToBand(ICargoWriter writer) => writer.WriteGuid(this.TileId);

    internal abstract int GetSerializedProtobufByteCount();

    internal abstract void SerializeProtobufToBand(CodedOutputStream output);
  }
}
