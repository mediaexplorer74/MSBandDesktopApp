// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PersonalizeBandViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.HomeShell)]
  [PageTaxonomy(new string[] {"Settings", "Personalize", "Theme Chooser"})]
  public class PersonalizeBandViewModel : PageViewModelBase, IHomeShellViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\PersonalizeBandViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IBandThemeManager bandThemeManager;
    private readonly PersonalizeBandControlViewModel personalizeBandControlViewModel;
    private readonly ISmoothNavService smoothNavService;
    private readonly ITileManagementService tileManager;
    private HealthCommand cancelCommand;
    private bool isUpdating;
    private bool showProgress;
    private HealthCommand updateThemeCommand;

    public PersonalizeBandViewModel(
      PersonalizeBandControlViewModel personalizeBandControlViewModel,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IBandThemeManager bandThemeManager,
      ITileManagementService tileManager,
      INetworkService networkService)
      : base(networkService)
    {
      this.personalizeBandControlViewModel = personalizeBandControlViewModel;
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.bandThemeManager = bandThemeManager;
      this.tileManager = tileManager;
      this.showProgress = true;
    }

    public NavigationHeaderBackground NavigationHeaderBackground => NavigationHeaderBackground.Dark;

    public bool ShowProgress
    {
      get => this.showProgress;
      set => this.SetProperty<bool>(ref this.showProgress, value, nameof (ShowProgress));
    }

    public bool IsUpdating
    {
      get => this.isUpdating;
      set => this.SetProperty<bool>(ref this.isUpdating, value, nameof (IsUpdating));
    }

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    private void Cancel()
    {
      PersonalizeBandViewModel.Logger.Debug((object) "<START> cancel saving BandTheme settings");
      this.bandThemeManager.RevertTheme();
      this.smoothNavService.GoHome();
      PersonalizeBandViewModel.Logger.Debug((object) "<END> cancel saving BandTheme settings");
    }

    public ICommand UpdateThemeCommand => (ICommand) this.updateThemeCommand ?? (ICommand) (this.updateThemeCommand = new HealthCommand(new Action(this.UpdateTheme)));

    private async void UpdateTheme()
    {
      PersonalizeBandViewModel.Logger.Debug((object) "<START> updating band BandTheme");
      try
      {
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        this.IsUpdating = true;
        await this.tileManager.SaveCurrentThemeToBandAsync(CancellationToken.None);
        ApplicationTelemetry.LogBandThemeChange(this.bandThemeManager.CurrentTheme.BackgroundStyle.Name, this.bandThemeManager.CurrentTheme.ColorSet.Name, false);
        PersonalizeBandViewModel.Logger.Debug((object) "<END> updating band BandTheme");
      }
      catch (Exception ex)
      {
        PersonalizeBandViewModel.Logger.Error(ex, "<FAILED> updating band BandTheme");
        this.bandThemeManager.RevertTheme();
        this.ShowAppBar();
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      finally
      {
        this.IsUpdating = false;
      }
    }

    public PersonalizeBandControlViewModel PersonalizeBandControlViewModel => this.personalizeBandControlViewModel;

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      PersonalizeBandViewModel.Logger.Debug((object) "<START> loadingd personalize band page");
      try
      {
        this.IsUpdating = false;
        this.ShowProgress = true;
        PersonalizeBandViewModel.Logger.Debug((object) "<START> fetching current theme information from band");
        AppBandTheme themeFromBandAsync = await this.bandThemeManager.GetCurrentThemeFromBandAsync(CancellationToken.None);
        PersonalizeBandViewModel.Logger.Debug((object) "<END> fetching current theme information from band");
        this.Invalidate();
        this.ShowProgress = false;
        if (this.bandThemeManager.IsThemeNotSaved)
          this.ShowAppBar();
        PersonalizeBandViewModel.Logger.Debug((object) "<END> loading personalize band page");
      }
      catch (Exception ex)
      {
        PersonalizeBandViewModel.Logger.Error(ex, "<FAILED> loading personalize band page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.smoothNavService.GoBack();
      }
    }

    private void Invalidate() => this.RaisePropertyChanged("PersonalizeBandControlViewModel");

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      this.Invalidate();
      PersonalizeBandViewModel.Logger.Debug((object) "<START> refreshing personalize band page");
      if (!this.bandThemeManager.IsThemeNotSaved)
        return;
      this.ShowAppBar();
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.UpdateThemeCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });
  }
}
