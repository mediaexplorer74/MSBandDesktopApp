// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.AdminBandTileExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Tiles;

namespace Microsoft.Band.Admin
{
  internal static class AdminBandTileExtensions
  {
    internal static TileData ToTileData(this AdminBandTile tile, uint startStripOrder = 0)
    {
      TileData tileData = new TileData();
      tileData.AppID = tile.Id;
      tileData.StartStripOrder = startStripOrder;
      tileData.ThemeColor = 0U;
      tileData.SettingsMask = (TileSettings) tile.SettingsMask;
      tileData.SetNameAndOwnerId(tile.Name, tile.OwnerId);
      return tileData;
    }
  }
}
