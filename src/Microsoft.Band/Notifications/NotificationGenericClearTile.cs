// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Notifications.NotificationGenericClearTile
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

using Google.Protobuf;
using Microsoft.Band.Protobuf;
using System;

namespace Microsoft.Band.Notifications
{
  internal sealed class NotificationGenericClearTile : NotificationBase
  {
    public NotificationGenericClearTile(Guid tileId)
      : base(tileId)
    {
    }

    internal override int GetSerializedProtobufByteCount() => 0 + 2 + ProtoGuid.GetSerializedProtobufByteCount() + (1 + CodedOutputStream.ComputeEnumSize(1));

    internal override void SerializeProtobufToBand(CodedOutputStream output)
    {
      output.WriteRawTag((byte) 10);
      output.WriteLength(ProtoGuid.GetSerializedProtobufByteCount());
      ProtoGuid.SerializeProtobufToBand(output, this.TileId);
      output.WriteRawTag((byte) 24);
      output.WriteEnum(1);
    }
  }
}
