// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WebTileAuthenticationViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin.WebTiles;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.FullScreenWebTile)]
  public class WebTileAuthenticationViewModel : WebTileViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WebTileAuthenticationViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IWebTileService webTileService;
    private ICommand confirmCommand;
    private IWebTileResource resource;
    private bool showError;
    private string username;
    private string password;
    private int resourceIndex;
    private int resourceCount;

    public WebTileAuthenticationViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService errorHandlingService,
      ITileManagementService tileManager,
      IWebTileService webTileService,
      ITileUpdateService tileUpdateService)
      : base(networkService, smoothNavService, messageBoxService, errorHandlingService, tileManager, webTileService, tileUpdateService)
    {
      this.errorHandlingService = errorHandlingService;
      this.webTileService = webTileService;
    }

    public override ICommand ConfirmCommand => this.confirmCommand ?? (this.confirmCommand = (ICommand) new HealthCommand((Action) (async () =>
    {
      this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
      this.ShowError = false;
      bool authenticated = false;
      try
      {
        await this.webTileService.CurrentWebTile.SetAuthenticationHeaderAsync(this.resource, this.Username, this.Password);
        authenticated = await this.webTileService.CurrentWebTile.AuthenticateResourceAsync(this.resource);
      }
      catch (Exception ex)
      {
        WebTileAuthenticationViewModel.Logger.Error((object) ex);
      }
      if (!authenticated)
      {
        this.ShowError = true;
        this.ShowAppBar();
      }
      else
      {
        this.resource = this.GetNextResource();
        if (this.resource == null)
        {
          await this.InstallWebTileAsync();
        }
        else
        {
          this.Username = string.Empty;
          this.Password = string.Empty;
          this.RefreshResourceProgress();
          this.ShowAppBar();
        }
      }
    })));

    public bool ShowError
    {
      get => this.showError;
      private set => this.SetProperty<bool>(ref this.showError, value, nameof (ShowError));
    }

    public string Username
    {
      get => this.username;
      set => this.SetProperty<string>(ref this.username, value, nameof (Username));
    }

    public string Password
    {
      get => this.password;
      set => this.SetProperty<string>(ref this.password, value, nameof (Password));
    }

    public string ResourceIndexProgress
    {
      get
      {
        if (this.resource == null)
          return string.Empty;
        return string.Format(AppResources.WebTileResourceIndexProgressFormat, new object[2]
        {
          (object) (this.resourceIndex + 1),
          (object) this.resourceCount
        });
      }
    }

    public string ResourceSource => this.resource == null ? string.Empty : this.resource.Url;

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      WebTileAuthenticationViewModel.Logger.Debug((object) "<START> loading web tile authentication page");
      this.Status = AppResources.WebTileProcessingMessage;
      this.resourceIndex = -1;
      try
      {
        this.IsUpdating = true;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        await base.LoadDataAsync(parameters);
        if (this.webTileService.CurrentWebTile == null || this.webTileService.ResourcesRequiringAuthentication == null || this.webTileService.ResourcesRequiringAuthentication.Count == 0)
          throw new InvalidOperationException("resources requiring authentication cannot be null or empty");
        this.resourceCount = this.webTileService.ResourcesRequiringAuthentication.Count;
        this.resource = this.GetNextResource();
        this.RefreshResourceProgress();
        this.ShowAppBar();
        WebTileAuthenticationViewModel.Logger.Debug((object) "<END> loading web tile authentication page");
      }
      catch (Exception ex)
      {
        WebTileAuthenticationViewModel.Logger.Error(ex, "<FAILED> loading web tile authentication page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.GoToManageTiles();
      }
      this.IsUpdating = false;
    }

    protected override WebTileIconViewModel CreateWebTileIcon() => this.webTileService.CurrentWebTile == null ? (WebTileIconViewModel) null : new WebTileIconViewModel(this.webTileService.CurrentWebTile.Name, this.webTileService.CurrentWebTile.TileBandIcon, this.webTileService.CurrentWebTile.Description);

    private IWebTileResource GetNextResource() => this.webTileService.ResourcesRequiringAuthentication.ElementAtOrDefault<IWebTileResource>(++this.resourceIndex);

    private void RefreshResourceProgress()
    {
      this.RaisePropertyChanged("ResourceIndexProgress");
      this.RaisePropertyChanged("ResourceSource");
    }
  }
}
