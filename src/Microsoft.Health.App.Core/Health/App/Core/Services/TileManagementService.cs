// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileManagementService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class TileManagementService : ITileManagementService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileManagementService.cs");
    private readonly IConfig config;
    private readonly IBandThemeManager bandThemeManager;
    private readonly IConfigProvider configProvider;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IPackageResourceService packageResourceService;
    private readonly ITileNotificationService tileNotificationService;
    private readonly ISupportedTileService supportedTileService;
    private readonly IKnownTileCollection knownTiles;
    private readonly IWebTileService webTileService;
    private readonly Dictionary<Type, PendingTileSettings> pendingSettingsDictionary = new Dictionary<Type, PendingTileSettings>();
    private SemaphoreSlim tileUpdateConfigurationSemaphore = new SemaphoreSlim(1, 1);
    private IList<AppBandTile> enabledTiles;
    private IList<string> pendingNotifications;
    private IList<AppBandTile> pendingTiles;

    public TileManagementService(
      IConfig config,
      IBandThemeManager bandThemeManager,
      IConfigProvider configProvider,
      IBandConnectionFactory cargoConnectionFactory,
      IPackageResourceService packageResourceService,
      ITileNotificationService tileNotificationService,
      ISupportedTileService supportedTileService,
      IKnownTileCollection knownTileCollection,
      IWebTileService webTileService)
    {
      this.config = config;
      this.bandThemeManager = bandThemeManager;
      this.configProvider = configProvider;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.packageResourceService = packageResourceService;
      this.tileNotificationService = tileNotificationService;
      this.supportedTileService = supportedTileService;
      this.knownTiles = knownTileCollection;
      this.webTileService = webTileService;
    }

    public IKnownTileCollection KnownTiles => this.knownTiles;

    public IWebTile GetWebTileById(Guid tileId) => this.webTileService.GetWebTileManager.GetWebTile(tileId);

    public IList<AppBandTile> EnabledTiles
    {
      get
      {
        if (this.config.EnabledTiles == null || !this.config.EnabledTiles.Any<string>() || string.IsNullOrWhiteSpace(this.config.EnabledTiles[0]))
          this.config.EnabledTiles = TileManagementService.GetIds((IEnumerable<AppBandTile>) this.KnownTiles);
        if (this.enabledTiles == null)
          this.SetEnabledTiles((ICollection<AppBandTile>) this.GetTiles(this.config.EnabledTiles));
        return this.enabledTiles;
      }
      set => this.SetEnabledTiles((ICollection<AppBandTile>) value);
    }

    public bool IsTileEnabled(string tileGuid)
    {
      foreach (AppBandTile enabledTile in (IEnumerable<AppBandTile>) this.EnabledTiles)
      {
        if (enabledTile.TileId.ToString() == tileGuid)
          return true;
      }
      return false;
    }

    public bool HaveTilesChanged { get; set; }

    public bool HaveTilesBeenUpdated { get; set; }

    public bool HaveTilesBeenReordered { get; set; }

    public bool HaveSettingsChanged => this.pendingSettingsDictionary.Values.Any<PendingTileSettings>((Func<PendingTileSettings, bool>) (tileSettings => tileSettings.IsChanged));

    public IList<string> PendingNotifications
    {
      get => this.pendingNotifications ?? (this.pendingNotifications = (IList<string>) new List<string>());
      set => this.pendingNotifications = value;
    }

    public IList<AppBandTile> PendingTiles
    {
      get => this.pendingTiles ?? (this.pendingTiles = (IList<AppBandTile>) new List<AppBandTile>());
      set => this.pendingTiles = value;
    }

    public void RevertPendingSettings() => this.pendingSettingsDictionary.Clear();

    public async Task ApplyPendingSettingsAsync()
    {
      foreach (PendingTileSettings settings in this.pendingSettingsDictionary.Values)
      {
        if (settings.IsChanged)
        {
          await settings.ApplyChangesAsync().ConfigureAwait(false);
          settings.IsChanged = false;
        }
      }
    }

    public async Task<T> GetPendingSettingsAsync<T>() where T : PendingTileSettings
    {
      PendingTileSettings pendingTileSettings;
      if (this.pendingSettingsDictionary.TryGetValue(typeof (T), out pendingTileSettings))
        return (T) pendingTileSettings;
      T typedSettings = ServiceLocator.Current.GetInstance<T>();
      await typedSettings.LoadSettingsAsync(CancellationToken.None).ConfigureAwait(false);
      this.pendingSettingsDictionary.Add(typeof (T), (PendingTileSettings) typedSettings);
      return typedSettings;
    }

    public async Task<IEnumerable<AppBandTile>> SetTilesAsync(
      ICollection<AppBandTile> selectedTiles,
      ICollection<AdminBandTile> originalTiles,
      BandClass bandClass)
    {
      List<AdminBandTile> validBandDefaults = await this.GetValidBandDefaultsAsync();
      StartStrip startStrip;
      try
      {
        await this.tileUpdateConfigurationSemaphore.WaitAsync();
        startStrip = await this.CreateStartStripAsync(selectedTiles, originalTiles, validBandDefaults, bandClass);
      }
      catch (Exception ex)
      {
        TileManagementService.Logger.Error(ex, "Error Getting Defaults While Resetting");
        throw;
      }
      finally
      {
        this.tileUpdateConfigurationSemaphore.Release();
      }
      BandTheme colorPalette;
      IDictionary<Guid, BandTheme> customPalettes;
      try
      {
        AppBandTheme themeFromBandAsync = await this.bandThemeManager.GetCurrentThemeFromBandAsync(CancellationToken.None);
        colorPalette = this.bandThemeManager.CurrentTheme.ColorSet.AsTileColorPalette();
        customPalettes = this.GetCustomPalettes(colorPalette, startStrip.Select<AdminBandTile, Guid>((Func<AdminBandTile, Guid>) (s => s.TileId)));
      }
      catch (Exception ex)
      {
        TileManagementService.Logger.Error(ex, "Error getting theme color palettes");
        throw;
      }
      try
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
          await cargoConnection.PersonalizeBandAsync(uint.MaxValue, CancellationToken.None, startStrip, color: colorPalette, customColors: customPalettes);
      }
      catch (Exception ex)
      {
        TileManagementService.Logger.Error(ex, "Error Setting Start Strip While Resetting");
        throw;
      }
      return (IEnumerable<AppBandTile>) selectedTiles;
    }

    private async Task<StartStrip> CreateStartStripAsync(
      ICollection<AppBandTile> selectedTiles,
      ICollection<AdminBandTile> originalTiles,
      List<AdminBandTile> validBandDefaults,
      BandClass bandClass)
    {
      List<AdminBandTile> addTiles = new List<AdminBandTile>();
      List<Guid> removeTiles = new List<Guid>();
      foreach (AppBandTile appBandTile in this.KnownTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty)))
      {
        AppBandTile tile = appBandTile;
        if (!selectedTiles.Contains(tile))
        {
          await this.ConfigureTileUpdateAsync(tile, false).ConfigureAwait(false);
          removeTiles.Add(tile.TileId);
        }
        else
        {
          AdminBandTile validTile = validBandDefaults.FirstOrDefault<AdminBandTile>((Func<AdminBandTile, bool>) (item => item.TileId == tile.TileId));
          AppBandTile selectedTile = selectedTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (enabledTile => enabledTile.TileId == tile.TileId));
          if (selectedTile == (AppBandTile) null)
            throw new InvalidOperationException("Could not find tile " + tile.Title + " in selected tiles list.");
          if (validTile == null)
          {
            List<AdminBandTile> adminBandTileList = addTiles;
            AdminBandTile cargoTileAsync = await this.ConvertToCargoTileAsync(selectedTile, bandClass, selectedTile.Settings, CancellationToken.None);
            adminBandTileList.Add(cargoTileAsync);
            adminBandTileList = (List<AdminBandTile>) null;
          }
          else
            validTile.SetSettings(selectedTile.TileId, selectedTile.Settings);
          await this.ConfigureTileUpdateAsync(tile, true).ConfigureAwait(false);
          validTile = (AdminBandTile) null;
          selectedTile = (AppBandTile) null;
        }
      }
      addTiles.AddRange(selectedTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (s => s.IsThirdParty)).Select<AppBandTile, AdminBandTile>((Func<AppBandTile, AdminBandTile>) (tile => originalTiles.FirstOrDefault<AdminBandTile>((Func<AdminBandTile, bool>) (p => p.TileId == tile.TileId)))));
      foreach (Guid guid in removeTiles)
      {
        Guid id = guid;
        validBandDefaults.Remove(validBandDefaults.Where<AdminBandTile>((Func<AdminBandTile, bool>) (item => item.TileId == id)).FirstOrDefault<AdminBandTile>());
      }
      validBandDefaults.AddRange((IEnumerable<AdminBandTile>) addTiles);
      return new StartStrip((IEnumerable<AdminBandTile>) TileManagementService.SortForRearrange((ICollection<AdminBandTile>) validBandDefaults, (ICollection<AppBandTile>) selectedTiles.ToList<AppBandTile>()));
    }

    private async Task<List<AdminBandTile>> GetValidBandDefaultsAsync()
    {
      List<AdminBandTile> validBandDefaults = new List<AdminBandTile>();
      try
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
        {
          IList<AdminBandTile> defaultTilesAsync = await cargoConnection.GetDefaultTilesAsync(CancellationToken.None);
          List<Guid> tileIds = this.KnownTiles.Select<AppBandTile, Guid>((Func<AppBandTile, Guid>) (tile => tile.TileId)).ToList<Guid>();
          if (tileIds.Any<Guid>())
            validBandDefaults = defaultTilesAsync.Where<AdminBandTile>((Func<AdminBandTile, bool>) (cs => tileIds.Contains(cs.TileId))).ToList<AdminBandTile>();
        }
      }
      catch (Exception ex)
      {
        TileManagementService.Logger.Error(ex, "Error Getting Defaults While Resetting");
        throw;
      }
      return validBandDefaults;
    }

    public Task SaveCurrentThemeToBandAsync(CancellationToken cancellationToken) => this.SaveTilesWithCurrentThemeToBandAsync((StartStrip) null, cancellationToken);

    public async Task SaveTilesWithCurrentThemeToBandAsync(
      StartStrip startStrip,
      CancellationToken cancellationToken)
    {
      BandTheme colorPalette = this.bandThemeManager.CurrentTheme.ColorSet.AsTileColorPalette();
      IDictionary<Guid, BandTheme> customPalettes = this.GetCustomPalettes(colorPalette, startStrip != null ? startStrip.Select<AdminBandTile, Guid>((Func<AdminBandTile, Guid>) (s => s.TileId)) : this.EnabledTiles.Select<AppBandTile, Guid>((Func<AppBandTile, Guid>) (s => s.TileId)));
      TileManagementService.Logger.Debug((object) "<START> setting band personalization settings");
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        IBandConnection bandConnection = cargoConnection;
        uint id = this.bandThemeManager.CurrentTheme.Id;
        CancellationToken cancellationToken1 = cancellationToken;
        StartStrip startStrip1 = startStrip;
        BandImage image = await this.packageResourceService.LoadWallpaperAsync(this.bandThemeManager.CurrentTheme, cancellationToken);
        await bandConnection.PersonalizeBandAsync(id, cancellationToken1, startStrip1, image, colorPalette, customPalettes);
        bandConnection = (IBandConnection) null;
        cancellationToken1 = new CancellationToken();
        startStrip1 = (StartStrip) null;
      }
      TileManagementService.Logger.Debug((object) "<END> setting band personalization settings");
      this.configProvider.Set<uint>("RecoveredDeviceTheme3", this.bandThemeManager.CurrentTheme.Id);
    }

    private static IList<AdminBandTile> SortForRearrange(
      ICollection<AdminBandTile> cargoTiles,
      ICollection<AppBandTile> bandTiles)
    {
      List<AdminBandTile> adminBandTileList = new List<AdminBandTile>();
      if (cargoTiles.Any<AdminBandTile>() && bandTiles.Any<AppBandTile>())
      {
        IEnumerable<Guid> guids = bandTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId != Guid.Empty)).Select<AppBandTile, Guid>((Func<AppBandTile, Guid>) (tile => tile.TileId));
        Dictionary<Guid, AdminBandTile> dictionary = cargoTiles.ToDictionary<AdminBandTile, Guid>((Func<AdminBandTile, Guid>) (arg => arg.TileId));
        foreach (Guid key in guids)
        {
          AdminBandTile adminBandTile;
          if (dictionary.TryGetValue(key, out adminBandTile))
            adminBandTileList.Add(adminBandTile);
        }
      }
      return (IList<AdminBandTile>) adminBandTileList;
    }

    private static IList<string> GetIds(IEnumerable<AppBandTile> bandTiles)
    {
      List<string> stringList = new List<string>();
      if (bandTiles != null && bandTiles.Any<AppBandTile>())
        stringList.AddRange(bandTiles.Select<AppBandTile, string>((Func<AppBandTile, string>) (tile => tile.UniqueId)));
      return (IList<string>) stringList;
    }

    private IList<AppBandTile> GetTiles(IList<string> ids)
    {
      List<AppBandTile> appBandTileList = new List<AppBandTile>();
      if (ids != null && ids.Any<string>())
      {
        foreach (string id1 in (IEnumerable<string>) ids)
        {
          string id = id1;
          AppBandTile appBandTile = this.KnownTiles.Where<AppBandTile>((Func<AppBandTile, bool>) (tp => tp.UniqueId == id)).FirstOrDefault<AppBandTile>();
          if (appBandTile != (AppBandTile) null)
            appBandTileList.Add(appBandTile.Copy());
        }
      }
      return (IList<AppBandTile>) appBandTileList;
    }

    private async Task<AdminBandTile> ConvertToCargoTileAsync(
      AppBandTile tile,
      BandClass bandClass,
      AdminTileSettings settings,
      CancellationToken token)
    {
      TileManagementService.Logger.Debug((object) string.Format("Creating {0} tile", new object[1]
      {
        (object) tile.Title
      }));
      List<BandIcon> bandIconList1 = new List<BandIcon>();
      List<BandIcon> bandIconList2 = bandIconList1;
      BandIcon bandIcon1 = await this.packageResourceService.LoadIconAsync(tile.BandIcon, bandClass, token);
      bandIconList2.Add(bandIcon1);
      List<BandIcon> icons = bandIconList1;
      bandIconList2 = (List<BandIcon>) null;
      bandIconList1 = (List<BandIcon>) null;
      if (tile.BandTileBandClassData[bandClass].ExtraIcons != null)
      {
        foreach (AppBandIcon extraIcon in (IEnumerable<AppBandIcon>) tile.BandTileBandClassData[bandClass].ExtraIcons)
        {
          bandIconList1 = icons;
          BandIcon bandIcon2 = await this.packageResourceService.LoadIconAsync(extraIcon, bandClass, token);
          bandIconList1.Add(bandIcon2);
          bandIconList1 = (List<BandIcon>) null;
        }
      }
      List<TileLayout> layoutsAsync = await this.GetLayoutsAsync(tile, bandClass);
      return new AdminBandTile(tile.TileId, tile.Title, settings, (IList<BandIcon>) icons, 0U, (IList<TileLayout>) layoutsAsync);
    }

    public Task EnsureTileUpdatesEnabledAsync()
    {
      List<AppBandTile> enabled = new List<AppBandTile>((IEnumerable<AppBandTile>) this.EnabledTiles);
      return this.tileUpdateConfigurationSemaphore.RunSynchronizedAsync((Func<Task>) (async () =>
      {
        foreach (AppBandTile tile in enabled)
          await this.ConfigureTileUpdateAsync(tile, true).ConfigureAwait(false);
      }));
    }

    public Task ConfigureTileUpdatesAsync(IEnumerable<AppBandTile> tilesToUpdate) => this.tileUpdateConfigurationSemaphore.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      foreach (AppBandTile tile in tilesToUpdate)
        await this.ConfigureTileUpdateAsync(tile, tile.ShowTile).ConfigureAwait(false);
    }));

    public async Task ConfigureTileUpdateAsync(AppBandTile tile, bool enable)
    {
      string s = tile.TileId.ToString();
      // ISSUE: reference to a compiler-generated method
      switch (PrivateImplementationDetails.ComputeStringHash(s))
      {
        case 186078193:
          if (!(s == "5992928a-bd79-4bb5-9678-f08246d03e68"))
            return;
          this.config.IsFinanceEnabled = enable;
          return;
        case 579993706:
          if (!(s == "4076b009-0455-4af7-a705-6d4acd45a556"))
            return;
          break;
        case 931107327:
          if (!(s == "64a29f65-70bb-4f32-99a2-0f250a05d427"))
            return;
          this.config.IsStarbucksEnabled = enable;
          return;
        case 951360430:
          if (!(s == "69a39b4e-084b-4b53-9a1b-581826df9e36"))
            return;
          this.config.IsWeatherEnabled = enable;
          return;
        case 1454291383:
          if (!(s == "d7fb5ff5-906a-4f2c-8269-dde6a75138c4"))
            return;
          break;
        case 1930533773:
          if (!(s == "22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"))
            return;
          break;
        case 2370078499:
          if (!(s == "ec149021-ce45-40e9-aeee-08f86e4746a7"))
            return;
          this.config.IsCalendarEnabled = enable;
          return;
        case 2530683590:
          if (!(s == "2e76a806-f509-4110-9c03-43dd2359d2ad"))
            return;
          break;
        case 2566451132:
          if (!(s == "b4edbc35-027b-4d10-a797-1099cd2ad98a"))
            return;
          break;
        case 2615271638:
          if (!(s == "a708f02a-03cd-4da0-bb33-be904e6a2924"))
            return;
          this.config.IsExerciseEnabled = enable;
          return;
        case 2627865013:
          if (!(s == "d36a92ea-3e85-4aed-a726-2898a6f2769b"))
            return;
          break;
        case 3389985662:
          if (!(s == "fd06b486-bbda-4da5-9014-124936386237"))
            return;
          break;
        case 3769397465:
          if (!(s == "76b08699-2f2e-9041-96c2-1f4bfc7eab10"))
            return;
          break;
        case 4116454580:
          if (!(s == "823ba55a-7c98-4261-ad5e-929031289c6e"))
            return;
          break;
        default:
          return;
      }
      await this.tileNotificationService.ConfigureTileNotificationAsync(tile, enable).ConfigureAwait(false);
    }

    public async Task<IList<AdminBandTile>> PrepareDefaultsAsync(
      IList<AdminBandTile> bandDefaults,
      BandClass bandClass,
      CancellationToken token)
    {
      List<TileManagementService.TileInfoHolder> defaultTiles = bandDefaults.Select<AdminBandTile, TileManagementService.TileInfoHolder>((Func<AdminBandTile, TileManagementService.TileInfoHolder>) (adminBandTile =>
      {
        AppBandTile appBandTile = this.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tile => tile.TileId == adminBandTile.TileId));
        return new TileManagementService.TileInfoHolder()
        {
          AdminTile = adminBandTile,
          Tile = appBandTile,
          Settings = appBandTile != (AppBandTile) null ? appBandTile.DefaultOnSettings : AdminTileSettings.EnableNotification
        };
      })).Where<TileManagementService.TileInfoHolder>((Func<TileManagementService.TileInfoHolder, bool>) (tile => tile.Tile != (AppBandTile) null && tile.Tile.IsDefaultTile)).ToList<TileManagementService.TileInfoHolder>();
      foreach (TileManagementService.TileInfoHolder tileInfoHolder in defaultTiles)
        tileInfoHolder.AdminTile.SetSettings(tileInfoHolder.AdminTile.TileId, tileInfoHolder.Settings);
      if (this.supportedTileService.IsCurrentlySupported(this.KnownTiles.MailTile))
      {
        AdminBandTile cargoTileAsync = await this.ConvertToCargoTileAsync(this.KnownTiles.MailTile, bandClass, this.KnownTiles.MailTile.DefaultOnSettings, token);
        defaultTiles.Add(new TileManagementService.TileInfoHolder()
        {
          AdminTile = cargoTileAsync,
          Tile = this.KnownTiles.MailTile,
          Settings = this.KnownTiles.MailTile.DefaultOnSettings
        });
      }
      if (this.supportedTileService.IsCurrentlySupported(this.KnownTiles.CortanaTile))
      {
        AdminBandTile cargoTileAsync = await this.ConvertToCargoTileAsync(this.KnownTiles.CortanaTile, bandClass, this.KnownTiles.CortanaTile.DefaultOnSettings, token);
        defaultTiles.Add(new TileManagementService.TileInfoHolder()
        {
          AdminTile = cargoTileAsync,
          Tile = this.KnownTiles.CortanaTile,
          Settings = this.KnownTiles.CortanaTile.DefaultOnSettings
        });
      }
      List<TileManagementService.TileInfoHolder> filteredTiles = new List<TileManagementService.TileInfoHolder>();
      foreach (TileManagementService.TileInfoHolder defaultTile in defaultTiles)
      {
        bool flag = this.supportedTileService.IsCurrentlySupported(defaultTile.Tile);
        if (flag)
          flag = await this.supportedTileService.CheckPermissionsForTileAsync(defaultTile.Tile);
        if (flag)
          filteredTiles.Add(defaultTile);
      }
      List<AdminBandTile> orderedTiles = filteredTiles.OrderBy<TileManagementService.TileInfoHolder, int>((Func<TileManagementService.TileInfoHolder, int>) (defaultTile => defaultTile.Tile.BandTileBandClassData[bandClass].DefaultOrder)).Select<TileManagementService.TileInfoHolder, AdminBandTile>((Func<TileManagementService.TileInfoHolder, AdminBandTile>) (tile => tile.AdminTile)).ToList<AdminBandTile>();
      this.EnabledTiles = (IList<AppBandTile>) new List<AppBandTile>(orderedTiles.Select<AdminBandTile, AppBandTile>((Func<AdminBandTile, AppBandTile>) (tile => this.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (tileFromApp => tileFromApp.TileId == tile.TileId)))));
      await this.tileUpdateConfigurationSemaphore.RunSynchronizedAsync((Func<Task>) (async () =>
      {
        foreach (AppBandTile enabledTile in (IEnumerable<AppBandTile>) this.EnabledTiles)
          await this.ConfigureTileUpdateAsync(enabledTile, true).ConfigureAwait(false);
      }), token);
      return (IList<AdminBandTile>) orderedTiles;
    }

    public IDictionary<Guid, BandTheme> GetCustomPalettes(
      BandTheme themePalette,
      IEnumerable<Guid> tileIds = null)
    {
      if (tileIds == null)
        tileIds = this.KnownTiles.Select<AppBandTile, Guid>((Func<AppBandTile, Guid>) (s => s.TileId));
      Dictionary<Guid, BandTheme> dictionary1 = new Dictionary<Guid, BandTheme>();
      foreach (Guid tileId1 in tileIds)
      {
        Guid tileId = tileId1;
        AppBandTile appBandTile = this.KnownTiles.FirstOrDefault<AppBandTile>((Func<AppBandTile, bool>) (s => s.TileId == tileId));
        if (appBandTile != (AppBandTile) null)
        {
          TileColorPaletteOverride paletteOverride = appBandTile.PaletteOverride;
          if (paletteOverride != null)
          {
            Dictionary<Guid, BandTheme> dictionary2 = dictionary1;
            Guid key = tileId;
            BandTheme bandTheme = new BandTheme();
            BandColor? nullable = paletteOverride.Base;
            bandTheme.Base = nullable ?? themePalette.Base;
            nullable = paletteOverride.HighContrast;
            bandTheme.HighContrast = nullable ?? themePalette.HighContrast;
            nullable = paletteOverride.Highlight;
            bandTheme.Highlight = nullable ?? themePalette.Highlight;
            nullable = paletteOverride.Lowlight;
            bandTheme.Lowlight = nullable ?? themePalette.Lowlight;
            nullable = paletteOverride.Muted;
            bandTheme.Muted = nullable ?? themePalette.Muted;
            nullable = paletteOverride.SecondaryText;
            bandTheme.SecondaryText = nullable ?? themePalette.SecondaryText;
            dictionary2.Add(key, bandTheme);
          }
        }
      }
      return (IDictionary<Guid, BandTheme>) dictionary1;
    }

    private async Task<List<TileLayout>> GetLayoutsAsync(
      AppBandTile tile,
      BandClass bandClass)
    {
      if (tile.Layouts == null || tile.Layouts.Count == 0)
        return new List<TileLayout>();
      List<TileLayout> layouts = new List<TileLayout>();
      foreach (string layout in (IEnumerable<string>) tile.Layouts)
      {
        TileLayout tileLayout = await this.packageResourceService.LoadLayoutAsync(layout, bandClass);
        layouts.Add(tileLayout);
      }
      return layouts;
    }

    private void SetEnabledTiles(ICollection<AppBandTile> enabledBandTiles)
    {
      if (enabledBandTiles == null)
        return;
      List<AppBandTile> appBandTileList = new List<AppBandTile>()
      {
        this.KnownTiles.StepsTile,
        this.KnownTiles.CaloriesTile
      };
      enabledBandTiles.Remove(this.KnownTiles.StepsTile);
      enabledBandTiles.Remove(this.KnownTiles.CaloriesTile);
      foreach (AppBandTile enabledBandTile in (IEnumerable<AppBandTile>) enabledBandTiles)
      {
        if (enabledBandTile != (AppBandTile) null)
          appBandTileList.Add(enabledBandTile.Copy());
      }
      this.enabledTiles = (IList<AppBandTile>) appBandTileList;
      this.config.EnabledTiles = TileManagementService.GetIds((IEnumerable<AppBandTile>) this.enabledTiles);
      this.RefreshCustomTilesEnabled();
    }

    private void RefreshCustomTilesEnabled()
    {
      this.config.IsCalendarEnabled = this.enabledTiles.Contains(this.KnownTiles.CalendarTile);
      this.config.IsFinanceEnabled = this.enabledTiles.Contains(this.KnownTiles.StocksTile);
      this.config.IsWeatherEnabled = this.enabledTiles.Contains(this.KnownTiles.WeatherTile);
      this.config.IsStarbucksEnabled = this.enabledTiles.Contains(this.KnownTiles.StarbucksTile);
    }

    public async Task<int> GetRemainingCapacityAsync(CancellationToken cancellationToken)
    {
      int num;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        int capacity = (int) await cargoConnection.GetMaxTileCountAsync();
        StartStrip withoutImagesAsync = await cargoConnection.GetStartStripWithoutImagesAsync(CancellationToken.None);
        num = capacity - withoutImagesAsync.Count;
      }
      return num;
    }

    public async Task<BandClass> GetBandClassAsync(CancellationToken cancellationToken)
    {
      BandClass bandClass;
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        bandClass = (await cargoConnection.GetAppBandConstantsAsync(cancellationToken)).BandClass;
      return bandClass;
    }

    public async Task<bool> AddTileToBandAsync(
      AdminBandTile tile,
      CancellationToken cancellationToken)
    {
      TileManagementService.Logger.Debug((object) "<START> adding web tile");
      try
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        {
          await this.tileUpdateConfigurationSemaphore.WaitAsync(cancellationToken);
          uint capacity = await cargoConnection.GetMaxTileCountAsync();
          StartStrip withoutImagesAsync = await cargoConnection.GetStartStripWithoutImagesAsync(cancellationToken);
          if ((long) withoutImagesAsync.Count >= (long) capacity)
          {
            TileManagementService.Logger.Error((object) "Band tile capacity reached");
            return false;
          }
          withoutImagesAsync.Add(tile);
          await cargoConnection.SetStartStripAsync(withoutImagesAsync, cancellationToken);
        }
      }
      catch (Exception ex)
      {
        TileManagementService.Logger.Error(ex, "Error adding tile to Band");
        throw;
      }
      finally
      {
        this.tileUpdateConfigurationSemaphore.Release();
      }
      TileManagementService.Logger.Debug((object) "<END> adding web tile");
      return true;
    }

    private class TileInfoHolder
    {
      public AdminBandTile AdminTile { get; set; }

      public AppBandTile Tile { get; set; }

      public AdminTileSettings Settings { get; set; }
    }
  }

    internal class PrivateImplementationDetails
    {
        internal static uint ComputeStringHash(string s)
        {
            uint num = new uint();
            if (s != null)
            {
                num = 0x811c9dc5;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 0x1000193;
                }
            }
            return num;
        }
    }
}
