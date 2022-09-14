// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Settings.SelectBandBackgroundStyleViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.ViewModels.Themes;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Settings
{
  [PageMetadata(PageContainerType.FullScreen)]
  [PageTaxonomy(new string[] {"Settings", "Personalize", "Wallpaper Chooser"})]
  public class SelectBandBackgroundStyleViewModel : PanelViewModelBase
  {
    private readonly IBandThemeManager bandThemeManager;
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private BandThemeViewModel bandThemeViewModel;
    private HealthCommand<SelectableItem<BandBackgroundStyle, ResourceIdentifier>> selectThemeCommand;

    public SelectBandBackgroundStyleViewModel(
      BandThemeViewModel bandThemeViewModel,
      ISmoothNavService smoothNavService,
      IBandThemeManager bandThemeManager,
      IErrorHandlingService errorHandlingService,
      INetworkService networkService)
      : base(networkService)
    {
      this.bandThemeViewModel = bandThemeViewModel;
      this.smoothNavService = smoothNavService;
      this.bandThemeManager = bandThemeManager;
      this.errorHandlingService = errorHandlingService;
    }

    public ICommand SelectThemeCommand => (ICommand) this.selectThemeCommand ?? (ICommand) (this.selectThemeCommand = new HealthCommand<SelectableItem<BandBackgroundStyle, ResourceIdentifier>>((Action<SelectableItem<BandBackgroundStyle, ResourceIdentifier>>) (selectableItem => this.errorHandlingService.HandleExceptions((Action) (() =>
    {
      this.bandThemeManager.SwitchActiveBackgroundStyle(selectableItem.Item);
      this.smoothNavService.GoBack();
    })))));

    public BandThemeViewModel BandTheme => this.bandThemeViewModel;
  }
}
