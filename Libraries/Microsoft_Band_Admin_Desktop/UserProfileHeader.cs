// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.UserProfileHeader
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  internal sealed class UserProfileHeader
  {
    private static readonly int serializedByteCount = 2 + CargoFileTime.GetSerializedByteCount() + 16;

    public ushort Version { get; set; }

    public DateTimeOffset? LastKDKSyncUpdateOn { get; set; }

    public Guid UserID { get; set; }

    public static int GetSerializedByteCount() => UserProfileHeader.serializedByteCount;

    public static UserProfileHeader DeserializeFromBand(ICargoReader reader)
    {
      UserProfileHeader userProfileHeader = new UserProfileHeader();
      userProfileHeader.Version = reader.ReadUInt16();
      if (userProfileHeader.Version > (ushort) 2)
        throw new SerializationException("Unsupported user profile version");
      userProfileHeader.LastKDKSyncUpdateOn = new DateTimeOffset?(CargoFileTime.DeserializeFromBandAsDateTimeOffset(reader));
      userProfileHeader.UserID = reader.ReadGuid();
      return userProfileHeader;
    }

    public void SerializeToBand(ICargoWriter writer, DynamicAdminBandConstants constants)
    {
      writer.WriteUInt16(constants.BandProfileAppDataVersion);
      if (this.LastKDKSyncUpdateOn.HasValue)
        CargoFileTime.SerializeToBandFromDateTimeOffset(writer, this.LastKDKSyncUpdateOn.Value);
      else
        CargoFileTime.SerializeToBandFromDateTimeOffset(writer, DateTimeOffset.MinValue);
      writer.WriteGuid(this.UserID);
    }
  }
}
