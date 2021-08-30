// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Themes.DesignBandThemeViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Themes;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Themes
{
  public class DesignBandThemeViewModel : BandThemeViewModelBase
  {
    public override BandColorSet DefaultColorSet
    {
      get => new BandColorSet((ushort) 0, "default", new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U));
      protected set
      {
      }
    }

    public override BandColorSet CurrentColorSet
    {
      get => new BandColorSet((ushort) 0, "default", new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U));
      protected set
      {
      }
    }

    public override AppBandTheme CurrentBandTheme
    {
      get => DesignBandThemeViewModel.CreateDefaultTheme();
      protected set
      {
      }
    }

    public override IEnumerable<SelectableItem<BandColorSet, BandColorSet>> SelectableColorSets
    {
      get => (IEnumerable<SelectableItem<BandColorSet, BandColorSet>>) new List<SelectableItem<BandColorSet, BandColorSet>>()
      {
        new SelectableItem<BandColorSet, BandColorSet>((BandColorSet) null, new BandColorSet((ushort) 0, "default", new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U))),
        new SelectableItem<BandColorSet, BandColorSet>((BandColorSet) null, new BandColorSet((ushort) 0, "default", new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U)), true)
      };
      protected set
      {
      }
    }

    public override IEnumerable<SelectableItem<BandBackgroundStyle, ResourceIdentifier>> SelectableBackgroundStyles
    {
      get => (IEnumerable<SelectableItem<BandBackgroundStyle, ResourceIdentifier>>) new List<SelectableItem<BandBackgroundStyle, ResourceIdentifier>>()
      {
        new SelectableItem<BandBackgroundStyle, ResourceIdentifier>((BandBackgroundStyle) null, DesignBandThemeViewModel.CreateDefaultResource())
      };
      protected set
      {
      }
    }

    private static AppBandTheme CreateDefaultTheme() => new AppBandTheme(new BandBackgroundStyle((ushort) 1, "Angle"), new BandColorSet((ushort) 1, "Violet", new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U)), BandClass.Cargo);

    private static ResourceIdentifier CreateDefaultResource() => new ResourceIdentifier();
  }
}
