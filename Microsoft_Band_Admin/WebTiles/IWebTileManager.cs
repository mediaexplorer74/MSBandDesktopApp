// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.IWebTileManager
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Band.Admin.WebTiles
{
  public interface IWebTileManager
  {
    Task<IWebTile> GetWebTilePackageAsync(Uri uri);

    Task<IWebTile> GetWebTilePackageAsync(Stream source, string sourceFileName);

    Task InstallWebTileAsync(IWebTile webTile);

    Task UninstallWebTileAsync(Guid tileId);

    Task<IList<IWebTile>> GetInstalledWebTilesAsync(bool loadTileDisplayIcons);

    Task<AdminBandTile> CreateAdminBandTileAsync(
      IWebTile webTile,
      BandClass bandClass);

    IList<Guid> GetInstalledWebTileIds();

    IWebTile GetWebTile(Guid tileId);

    Task DeleteAllStoredResourceCredentialsAsync();
  }
}
