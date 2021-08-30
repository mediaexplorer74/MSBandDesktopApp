// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileDataExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Tiles;

namespace Microsoft.Band.Admin
{
  internal static class TileDataExtensions
  {
    internal static AdminBandTile ToAdminBandTile(this TileData data)
    {
      string name = "(DEFAULT NAME)";
      if (data.FriendlyName != null && data.FriendlyNameLength > (ushort) 0)
        name = data.FriendlyName;
      return new AdminBandTile(data.AppID, name, (AdminTileSettings) data.SettingsMask, data.Icon)
      {
        OwnerId = data.OwnerId
      };
    }
  }
}
