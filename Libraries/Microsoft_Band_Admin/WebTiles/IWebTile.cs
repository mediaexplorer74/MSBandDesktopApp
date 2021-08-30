// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.IWebTile
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Band.Admin.WebTiles
{
  public interface IWebTile
  {
    Guid TileId { get; set; }

    string Name { get; }

    string Description { get; }

    uint ManifestVersion { get; }

    uint Version { get; }

    string VersionString { get; }

    string Author { get; }

    string Organization { get; }

    string ContactEmail { get; }

    Dictionary<int, string> TileIcons { get; }

    Dictionary<int, string> BadgeIcons { get; }

    Dictionary<string, string> AdditionalIcons { get; }

    BandIcon TileBandIcon { get; }

    BandIcon BadgeBandIcon { get; }

    BandIcon[] AdditionalBandIcons { get; }

    WebTileTheme TileTheme { get; }

    uint RefreshIntervalMinutes { get; }

    IWebTileResource[] Resources { get; }

    WebTilePage[] Pages { get; }

    IImageProvider ImageProvider { get; set; }

    string PackageFolderPath { get; set; }

    Task LoadIconsAsync();

    Task<TileLayout[]> GetLayoutsAsync(BandClass bandClass);

    Task<bool> ResolveContentMappingsAsync();

    string DataFolderPath { get; set; }

    IStorageProvider StorageProvider { get; set; }

    HeaderNameValuePair[] RequestHeaders { get; set; }

    bool HasRefreshIntervalElapsed(DateTimeOffset time);

    void SaveLastSync(DateTimeOffset time);

    bool HasNotifications { get; }

    bool NotificationEnabled { get; set; }

    Task SetNotificationEnabledAsync(bool enabled);

    Task SaveUserSettingsAsync();

    void SaveUserSettings();

    void LoadUserSettings();

    List<PageData> Refresh(
      out bool clearPages,
      out bool sendAsMessage,
      out NotificationDialog notificationDialog);

    void Validate();

    void SetAuthenticationHeader(IWebTileResource resource, string userName, string password);

    Task SetAuthenticationHeaderAsync(
      IWebTileResource resource,
      string userName,
      string password);

    Task<bool> AuthenticateResourceAsync(IWebTileResource resource);

    void SaveResourceAuthentication();

    void LoadResourceAuthentication();

    void DeleteStoredResourceCredentials();
  }
}
