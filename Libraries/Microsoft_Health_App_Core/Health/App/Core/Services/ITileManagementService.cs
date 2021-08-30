// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ITileManagementService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface ITileManagementService
  {
    IKnownTileCollection KnownTiles { get; }

    IWebTile GetWebTileById(Guid tileId);

    IList<AppBandTile> EnabledTiles { get; set; }

    bool HaveTilesChanged { get; set; }

    bool HaveTilesBeenUpdated { get; set; }

    bool HaveTilesBeenReordered { get; set; }

    bool HaveSettingsChanged { get; }

    bool IsTileEnabled(string tileGuid);

    IList<string> PendingNotifications { get; set; }

    IList<AppBandTile> PendingTiles { get; set; }

    Task<IEnumerable<AppBandTile>> SetTilesAsync(
      ICollection<AppBandTile> selectedTiles,
      ICollection<AdminBandTile> originalTiles,
      BandClass bandClass);

    Task SaveCurrentThemeToBandAsync(CancellationToken cancellationToken);

    Task SaveTilesWithCurrentThemeToBandAsync(
      StartStrip startStrip,
      CancellationToken cancellationToken);

    Task EnsureTileUpdatesEnabledAsync();

    Task ConfigureTileUpdateAsync(AppBandTile tile, bool enable);

    Task<IList<AdminBandTile>> PrepareDefaultsAsync(
      IList<AdminBandTile> bandDefaults,
      BandClass bandClass,
      CancellationToken token);

    IDictionary<Guid, BandTheme> GetCustomPalettes(
      BandTheme theme,
      IEnumerable<Guid> tileIds = null);

    Task ConfigureTileUpdatesAsync(IEnumerable<AppBandTile> tilesToUpdate);

    Task<int> GetRemainingCapacityAsync(CancellationToken cancellationToken);

    Task<BandClass> GetBandClassAsync(CancellationToken cancellationToken);

    Task<bool> AddTileToBandAsync(AdminBandTile tile, CancellationToken cancellationToken);

    void RevertPendingSettings();

    Task ApplyPendingSettingsAsync();

    Task<T> GetPendingSettingsAsync<T>() where T : PendingTileSettings;
  }
}
