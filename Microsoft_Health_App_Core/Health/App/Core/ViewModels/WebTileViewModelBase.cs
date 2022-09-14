// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WebTileViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles"})]
  [PageMetadata(PageContainerType.FullScreenWebTile)]
  public abstract class WebTileViewModelBase : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WebTileViewModelBase.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ISmoothNavService smoothNavService;
    private readonly ITileManagementService tileManager;
    private readonly IWebTileService webTileService;
    private readonly ITileUpdateService tileUpdateService;
    private WebTileIconViewModel webTileIcon;
    private HealthCommand cancelCommand;
    private bool isUpdating;
    private bool errorEncountered;
    private string status;

    protected WebTileViewModelBase(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService errorHandlingService,
      ITileManagementService tileManager,
      IWebTileService webTileService,
      ITileUpdateService tileUpdateService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.messageBoxService = messageBoxService;
      this.errorHandlingService = errorHandlingService;
      this.tileManager = tileManager;
      this.webTileService = webTileService;
      this.tileUpdateService = tileUpdateService;
    }

    public string Status
    {
      get => this.status;
      set => this.SetProperty<string>(ref this.status, value, nameof (Status));
    }

    public bool IsUpdating
    {
      get => this.isUpdating;
      set => this.SetProperty<bool>(ref this.isUpdating, value, nameof (IsUpdating));
    }

    public WebTileIconViewModel WebTileIcon
    {
      get => this.webTileIcon;
      set => this.SetProperty<WebTileIconViewModel>(ref this.webTileIcon, value, nameof (WebTileIcon));
    }

    public abstract ICommand ConfirmCommand { get; }

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (async () => await this.CancelAsync())));

    private async Task CancelAsync()
    {
      WebTileViewModelBase.Logger.Debug((object) "<START> reject installing web tile");
      this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      await this.webTileService.ClearWebTileAsync(CancellationToken.None);
      this.GoToManageTiles();
      WebTileViewModelBase.Logger.Debug((object) "<END> reject installing web tile");
    }

    protected async Task InstallWebTileAsync()
    {
      WebTileViewModelBase.Logger.Debug((object) "<START> installing web tile");
      this.errorEncountered = false;
      this.Status = AppResources.WebTileInstallationMessage;
      string webTileSource = this.webTileService.WebTileUri == (Uri) null ? this.webTileService.WebTileFileName : this.webTileService.WebTileUri.ToString();
      try
      {
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        this.IsUpdating = true;
        if (await this.tileManager.GetRemainingCapacityAsync(CancellationToken.None) > 0)
        {
          BandClass bandClass = await this.tileManager.GetBandClassAsync(CancellationToken.None);
          await this.webTileService.InstallWebTileAsync(CancellationToken.None);
          AdminBandTile tile = await this.webTileService.GetAdminBandTileFromWebTileAsync(CancellationToken.None, bandClass);
          if (await this.tileManager.AddTileToBandAsync(tile, CancellationToken.None))
          {
            ApplicationTelemetry.LogWebTileInstallationSuccess(this.webTileService.CurrentWebTile.Name, webTileSource);
            try
            {
              await this.tileUpdateService.SyncWebTileAsync(tile.TileId, CancellationToken.None);
            }
            catch (Exception ex)
            {
              object[] objArray = new object[1]
              {
                (object) ex.Message
              };
              ApplicationTelemetry.LogWebTileInstallationFailure(this.webTileService.CurrentWebTile.Name, webTileSource, string.Format("Syncing failed after installing new webtile: {0}", objArray));
            }
          }
          else
          {
            ApplicationTelemetry.LogWebTileInstallationFailure(this.webTileService.CurrentWebTile.Name, webTileSource, "AddTileToBandAsync failed");
            this.errorEncountered = true;
          }
          tile = (AdminBandTile) null;
        }
        else
        {
          ApplicationTelemetry.LogWebTileInstallationFailure(this.webTileService.CurrentWebTile.Name, webTileSource, "Band lacks capacity");
          this.errorEncountered = true;
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorTileCapacityBody, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);
        }
        WebTileViewModelBase.Logger.Debug((object) "<END> installing web tile");
        await this.webTileService.ClearWebTileAsync(CancellationToken.None);
        this.GoToManageTiles();
      }
      catch (Exception ex)
      {
        this.errorEncountered = true;
        ApplicationTelemetry.LogWebTileInstallationFailure(this.webTileService.CurrentWebTile.Name, webTileSource, ex.Message);
        WebTileViewModelBase.Logger.Error(ex, "<FAILED> installing web tile");
        this.ShowAppBar();
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
      this.IsUpdating = false;
    }

    public override void OnResume()
    {
      base.OnResume();
      if (!this.errorEncountered)
        return;
      this.GoToManageTiles();
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.WebTileIcon = this.CreateWebTileIcon();
      return (Task) Task.FromResult<bool>(true);
    }

    protected void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    protected void GoToManageTiles() => this.smoothNavService.Navigate(typeof (ManageTilesViewModel), action: NavigationStackAction.RemovePrevious);

    protected abstract WebTileIconViewModel CreateWebTileIcon();
  }
}
