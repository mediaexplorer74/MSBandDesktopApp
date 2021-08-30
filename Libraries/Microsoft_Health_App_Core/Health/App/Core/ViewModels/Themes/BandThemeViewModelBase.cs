// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Themes.BandThemeViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Themes;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Themes
{
  public abstract class BandThemeViewModelBase : HealthViewModelBase
  {
    public abstract BandColorSet DefaultColorSet { get; protected set; }

    public abstract BandColorSet CurrentColorSet { get; protected set; }

    public abstract AppBandTheme CurrentBandTheme { get; protected set; }

    public abstract IEnumerable<SelectableItem<BandColorSet, BandColorSet>> SelectableColorSets { get; protected set; }

    public abstract IEnumerable<SelectableItem<BandBackgroundStyle, ResourceIdentifier>> SelectableBackgroundStyles { get; protected set; }
  }
}
