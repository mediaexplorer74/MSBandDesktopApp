// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WebTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles"})]
  [PageMetadata(PageContainerType.FullScreenWebTile)]
  public class WebTileViewModel : WebTileViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WebTileViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IWebTileService webTileService;
    private readonly ISmoothNavService smoothNavService;
    private ICommand confirmCommand;
    private bool errorEncountered;

    public WebTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService errorHandlingService,
      ITileManagementService tileManager,
      IWebTileService webTileService,
      ITileUpdateService tileUpdateService)
      : base(networkService, smoothNavService, messageBoxService, errorHandlingService, tileManager, webTileService, tileUpdateService)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.webTileService = webTileService;
    }

    public override ICommand ConfirmCommand => this.confirmCommand ?? (this.confirmCommand = (ICommand) new HealthCommand((Action) (async () =>
    {
      if (this.webTileService.ResourcesRequiringAuthentication.Count == 0)
        await this.InstallWebTileAsync();
      else
        this.smoothNavService.Navigate(typeof (WebTileAuthenticationViewModel), action: NavigationStackAction.RemovePrevious);
    })));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      WebTileViewModel.Logger.Debug((object) "<START> loading web tile page");
      this.errorEncountered = false;
      this.Status = AppResources.WebTileProcessingMessage;
      try
      {
        this.IsUpdating = true;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        WebTileViewModel.Logger.Debug((object) "<START> loading web tile information from package");
        await this.webTileService.LoadWebTileFromCurrentPackageAsync(CancellationToken.None);
        WebTileViewModel.Logger.Debug((object) "<END> loading web tile information from package");
        WebTileViewModel.Logger.Debug((object) "<START> loading web tile resources requiring authentication");
        await this.webTileService.LoadResourcesRequiringAuthenticationAsync(CancellationToken.None);
        WebTileViewModel.Logger.Debug((object) "<END> loading web tile resources requiring authentication");
        await base.LoadDataAsync(parameters);
        this.ShowAppBar();
        WebTileViewModel.Logger.Debug((object) "<END> loading web tile page");
      }
      catch (Exception ex)
      {
        this.errorEncountered = true;
        WebTileViewModel.Logger.Error(ex, "<FAILED> loading web tile page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.GoToManageTiles();
      }
      this.IsUpdating = false;
    }

    protected override WebTileIconViewModel CreateWebTileIcon() => this.webTileService.CurrentWebTile == null ? (WebTileIconViewModel) null : new WebTileIconViewModel(this.webTileService.CurrentWebTile.Name, this.webTileService.CurrentWebTile.TileBandIcon, this.webTileService.CurrentWebTile.Description, this.webTileService.CurrentWebTile.Author, this.webTileService.CurrentWebTile.Organization, this.webTileService.CurrentWebTile.VersionString, ((IEnumerable<IWebTileResource>) this.webTileService.CurrentWebTile.Resources).Select<IWebTileResource, string>((Func<IWebTileResource, string>) (p => p.Url)));

    protected override void OnNavigatedTo()
    {
      WebTileViewModel.Logger.Debug((object) "<START> refreshing web tile page");
      base.OnNavigatedTo();
      WebTileViewModel.Logger.Debug((object) "<END> refreshing web tile page");
    }

    public override async Task ChangeAsync(IDictionary<string, string> parameters = null)
    {
      await base.ChangeAsync(parameters);
      await this.LoadDataAsync(parameters);
    }

    public override void OnResume()
    {
      base.OnResume();
      if (!this.errorEncountered)
        return;
      this.GoToManageTiles();
    }
  }
}
