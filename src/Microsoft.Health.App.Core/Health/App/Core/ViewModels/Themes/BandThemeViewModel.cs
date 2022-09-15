// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Themes.BandThemeViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Themes
{
  public class BandThemeViewModel : BandThemeViewModelBase
  {
    private readonly IBandThemeManager bandThemeManager;
    private readonly IPackageResourceService packageResourceService;

    public BandThemeViewModel(
      IBandThemeManager bandThemeManager,
      IPackageResourceService packageResourceService)
    {
      this.packageResourceService = packageResourceService;
      this.bandThemeManager = bandThemeManager;
      this.bandThemeManager.SetDeviceTypeAsync();
      this.bandThemeManager.PropertyChanged += (PropertyChangedEventHandler) ((sender, args) =>
      {
        if (!(args.PropertyName == "CurrentTheme"))
          return;
        this.RaisePropertyChanged(nameof (CurrentColorSet));
        this.RaisePropertyChanged(nameof (CurrentBandTheme));
      });
    }

    public override sealed BandColorSet DefaultColorSet
    {
      get => this.bandThemeManager.DefaultTheme.ColorSet;
      protected set
      {
      }
    }

    public override sealed BandColorSet CurrentColorSet
    {
      get => this.bandThemeManager.CurrentTheme.ColorSet;
      protected set
      {
      }
    }

    public override sealed AppBandTheme CurrentBandTheme
    {
      get => this.bandThemeManager.CurrentTheme;
      protected set
      {
      }
    }

    public override IEnumerable<SelectableItem<BandColorSet, BandColorSet>> SelectableColorSets
    {
      get => this.bandThemeManager.ColorSets.Select<BandColorSet, SelectableItem<BandColorSet, BandColorSet>>((Func<BandColorSet, SelectableItem<BandColorSet, BandColorSet>>) (item => new SelectableItem<BandColorSet, BandColorSet>(item, item, item == this.bandThemeManager.CurrentTheme.ColorSet)));
      protected set
      {
      }
    }

    public override IEnumerable<SelectableItem<BandBackgroundStyle, ResourceIdentifier>> SelectableBackgroundStyles
    {
      get => this.bandThemeManager.Themes.Where<AppBandTheme>((Func<AppBandTheme, bool>) (item => item.ColorSet == this.bandThemeManager.CurrentTheme.ColorSet)).Select<AppBandTheme, SelectableItem<BandBackgroundStyle, ResourceIdentifier>>((Func<AppBandTheme, SelectableItem<BandBackgroundStyle, ResourceIdentifier>>) (item => new SelectableItem<BandBackgroundStyle, ResourceIdentifier>(item.BackgroundStyle, this.packageResourceService.GetWallpaperResourceId(item), item.BackgroundStyle == this.bandThemeManager.CurrentTheme.BackgroundStyle)));
      protected set
      {
      }
    }
  }
}
