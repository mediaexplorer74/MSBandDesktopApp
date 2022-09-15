// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PersonalizeBandControlViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.ViewModels.Settings;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class PersonalizeBandControlViewModel
  {
    private readonly ISmoothNavService navService;
    private readonly IBandThemeManager bandThemeManager;
    private readonly IPackageResourceService packageResourceService;
    private ICommand selectBackgroundCommand;
    private ICommand selectColorCommand;

    public PersonalizeBandControlViewModel(
      ISmoothNavService navService,
      IBandThemeManager bandThemeManager,
      IPackageResourceService packageResourceService)
    {
      this.navService = navService;
      this.bandThemeManager = bandThemeManager;
      this.packageResourceService = packageResourceService;
    }

    public ICommand SelectColorCommand => this.selectColorCommand ?? (this.selectColorCommand = (ICommand) new HealthCommand(new Action(this.SelectColor)));

    public ICommand SelectBackgroundCommand => this.selectBackgroundCommand ?? (this.selectBackgroundCommand = (ICommand) new HealthCommand(new Action(this.SelectBackground)));

    public DateTimeOffset CurrentTime => DateTimeOffset.Now.ToLocalTime();

    public AppBandTheme CurrentBandTheme => this.bandThemeManager.CurrentTheme;

    public ResourceIdentifier CurrentWallpaperResource => this.packageResourceService.GetWallpaperResourceId(this.CurrentBandTheme);

    private void SelectColor() => this.navService.Navigate(typeof (SelectBandColorSetViewModel));

    private void SelectBackground() => this.navService.Navigate(typeof (SelectBandBackgroundStyleViewModel));
  }
}
