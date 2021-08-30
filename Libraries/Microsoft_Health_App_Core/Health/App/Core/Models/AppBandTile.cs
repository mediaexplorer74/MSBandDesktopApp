// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.AppBandTile
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class AppBandTile : HealthObservableObject, IEquatable<AppBandTile>
  {
    private ArgbColor32 tileGlyphColor;
    private ArgbColor32 tileOverlayGlyphColor;
    private bool isCurrentlySupported;
    private bool canBeModified;
    private bool hasSettings;
    private bool isDefaultTile;
    private bool isThirdParty;
    private bool isDirty;
    private bool isNotificationEnabled;
    private bool requiresCortana;
    private bool requiresLocationServices;
    private bool requiresSms;
    private Microsoft.Band.Tiles.BandIcon thirdPartyIcon;
    private IReadOnlyList<string> settingsPageTaxonomy;
    private bool showSetting;
    private bool showTile;
    private bool isWebTile;
    private bool isWebTileNotificationEnabled;

    public AppBandTile()
    {
      this.CanBeModified = true;
      this.ShowInSettings = true;
      this.IsCurrentlySupported = true;
      this.HasSettings = false;
      this.TileGlyphColor = new ArgbColor32(uint.MaxValue);
      this.TileOverlayGlyphColor = new ArgbColor32(uint.MaxValue);
      this.BandTileBandClassData = (IDictionary<BandClass, Microsoft.Health.App.Core.Models.BandTileBandClassData>) new Dictionary<BandClass, Microsoft.Health.App.Core.Models.BandTileBandClassData>()
      {
        {
          BandClass.Cargo,
          new Microsoft.Health.App.Core.Models.BandTileBandClassData()
        },
        {
          BandClass.Envoy,
          new Microsoft.Health.App.Core.Models.BandTileBandClassData()
        }
      };
    }

    public string Title { get; set; }

    public string EnablementText { get; set; }

    public AppBandIcon BandIcon { get; set; }

    public string TileGlyph { get; set; }

    public ArgbColor32 TileGlyphColor
    {
      get => !this.IsCurrentlySupported ? CoreColors.ManageTiles.Disabled : this.tileGlyphColor;
      set => this.SetProperty<ArgbColor32>(ref this.tileGlyphColor, value, nameof (TileGlyphColor));
    }

    public string TileOverlayGlyph { get; set; }

    public ArgbColor32 TileOverlayGlyphColor
    {
      get => !this.IsCurrentlySupported ? CoreColors.ManageTiles.Disabled : this.tileOverlayGlyphColor;
      set => this.SetProperty<ArgbColor32>(ref this.tileOverlayGlyphColor, value, nameof (TileOverlayGlyphColor));
    }

    public ArgbColor32 TextColor => !this.IsCurrentlySupported ? CoreColors.ManageTiles.Disabled : CoreColors.ManageTiles.Enabled;

    public ArgbColor32 EditGlyphColor => !this.ShowTile || !this.IsCurrentlySupported ? CoreColors.ManageTiles.Disabled : CoreColors.ManageTiles.Edit;

    public Type TileViewModelType { get; set; }

    public Type SettingViewModelType { get; set; }

    public Guid TileId { get; set; }

    public AdminTileSettings Settings { get; set; }

    public AdminTileSettings DefaultOnSettings { get; set; }

    public AdminTileSettings DefaultOffSettings { get; set; }

    public IList<string> Layouts { get; set; }

    public IDictionary<BandClass, Microsoft.Health.App.Core.Models.BandTileBandClassData> BandTileBandClassData { get; set; }

    public TileColorPaletteOverride PaletteOverride { get; set; }

    public string UniqueId
    {
      get
      {
        if (this.TileId != Guid.Empty)
          return this.TileId.ToString();
        return (object) this.TileViewModelType != null ? this.TileViewModelType.Name : throw new InvalidOperationException("Either TileId or TileViewModelType needs to be set");
      }
    }

    public bool ShowTile
    {
      get => this.showTile;
      set
      {
        if (!this.SetProperty<bool>(ref this.showTile, value, nameof (ShowTile)))
          return;
        this.RaisePropertyChanged("EditGlyphColor");
      }
    }

    public bool ShowInSettings
    {
      get => this.showSetting;
      set => this.SetProperty<bool>(ref this.showSetting, value, nameof (ShowInSettings));
    }

    public bool HasSettings
    {
      get => this.hasSettings;
      set => this.SetProperty<bool>(ref this.hasSettings, value, nameof (HasSettings));
    }

    public bool IsNotificationEnabled
    {
      get => this.isNotificationEnabled;
      set => this.SetProperty<bool>(ref this.isNotificationEnabled, value, nameof (IsNotificationEnabled));
    }

    public bool IsCurrentlySupported
    {
      get => this.isCurrentlySupported;
      set
      {
        if (!this.SetProperty<bool>(ref this.isCurrentlySupported, value, nameof (IsCurrentlySupported)))
          return;
        this.RaisePropertyChanged("CanBeModified");
        this.RaisePropertyChanged("TileGlyphColor");
        this.RaisePropertyChanged("TileOverlayGlyphColor");
        this.RaisePropertyChanged("TextColor");
        this.RaisePropertyChanged("EditGlyphColor");
      }
    }

    public bool CanBeModified
    {
      get => this.canBeModified && this.isCurrentlySupported;
      set => this.SetProperty<bool>(ref this.canBeModified, value, nameof (CanBeModified));
    }

    public bool IsDefaultTile
    {
      get => this.isDefaultTile;
      set => this.SetProperty<bool>(ref this.isDefaultTile, value, nameof (IsDefaultTile));
    }

    public bool RequiresLocationServices
    {
      get => this.requiresLocationServices;
      set => this.SetProperty<bool>(ref this.requiresLocationServices, value, nameof (RequiresLocationServices));
    }

    public bool RequiresCortana
    {
      get => this.requiresCortana;
      set => this.SetProperty<bool>(ref this.requiresCortana, value, nameof (RequiresCortana));
    }

    public bool RequiresSms
    {
      get => this.requiresSms;
      set => this.SetProperty<bool>(ref this.requiresSms, value, nameof (RequiresSms));
    }

    public IReadOnlyList<string> SettingsPageTaxonomy
    {
      get => this.settingsPageTaxonomy;
      set => this.SetProperty<IReadOnlyList<string>>(ref this.settingsPageTaxonomy, value, nameof (SettingsPageTaxonomy));
    }

    public bool IsThirdParty
    {
      get => this.isThirdParty;
      set => this.SetProperty<bool>(ref this.isThirdParty, value, nameof (IsThirdParty));
    }

    public Microsoft.Band.Tiles.BandIcon ThirdPartyIcon
    {
      get => this.thirdPartyIcon;
      set => this.SetProperty<Microsoft.Band.Tiles.BandIcon>(ref this.thirdPartyIcon, value, nameof (ThirdPartyIcon));
    }

    public Guid ThirdPartyOwnerId { get; set; }

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.SetProperty<bool>(ref this.isDirty, value, nameof (IsDirty));
    }

    public bool IsWebTile
    {
      get => this.isWebTile;
      set => this.SetProperty<bool>(ref this.isWebTile, value, nameof (IsWebTile));
    }

    public bool IsWebTileNotificationEnabled
    {
      get => this.isWebTileNotificationEnabled;
      set => this.SetProperty<bool>(ref this.isWebTileNotificationEnabled, value, nameof (IsWebTileNotificationEnabled));
    }

    public bool Equals(AppBandTile tile) => tile != (AppBandTile) null && this.TileId.Equals(tile.TileId);

    public override int GetHashCode() => new
    {
      A = this.TileId
    }.GetHashCode();

    public override bool Equals(object obj) => this.Equals(obj as AppBandTile);

    public override string ToString() => this.Title ?? string.Empty;

    public AppBandTile Copy() => (AppBandTile) this.MemberwiseClone();

    public static bool operator ==(AppBandTile tileX, AppBandTile tileY) => object.Equals((object) tileX, (object) tileY);

    public static bool operator !=(AppBandTile tileX, AppBandTile tileY) => !(tileX == tileY);
  }
}
