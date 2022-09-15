// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.EditStarbucksCardViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles", "Starbucks", "Add Card"})]
  public class EditStarbucksCardViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\EditStarbucksCardViewModel.cs");
    private readonly ISmoothNavService navService;
    private readonly IMessageBoxService messageBoxService;
    private readonly INetworkService networkService;
    private readonly ITileManagementService tileManagementService;
    private readonly IRegionService regionService;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private string cardNumber;
    private HealthCommand confirmCommand;
    private HealthCommand cancelCommand;
    private StarbucksPendingTileSettings pendingTileSettings;

    public EditStarbucksCardViewModel(
      ISmoothNavService navService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IRegionService regionService,
      IDynamicConfigurationService dynamicConfigurationService)
      : base(networkService)
    {
      this.navService = navService;
      this.messageBoxService = messageBoxService;
      this.networkService = networkService;
      this.tileManagementService = tileManagementService;
      this.regionService = regionService;
      this.dynamicConfigurationService = dynamicConfigurationService;
    }

    public string CardNumber
    {
      get => this.cardNumber;
      set => this.SetProperty<string>(ref this.cardNumber, value, nameof (CardNumber));
    }

    public EmbeddedOrRemoteImageSource StarbucksCardBackSource
    {
      get
      {
        if (this.dynamicConfigurationService.Configuration.Features.Starbucks.CardBackUrl != (Uri) null && this.networkService.Connected)
          return new EmbeddedOrRemoteImageSource(this.dynamicConfigurationService.Configuration.Features.Starbucks.CardBackUrl.ToString());
        return this.regionService.CurrentRegion.TwoLetterISORegionName == "GB" ? new EmbeddedOrRemoteImageSource(EmbeddedAsset.StarbucksCardBackGb) : new EmbeddedOrRemoteImageSource(EmbeddedAsset.StarbucksCardBackUs);
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EditStarbucksCardViewModel starbucksCardViewModel = this;
      StarbucksPendingTileSettings pendingTileSettings = starbucksCardViewModel.pendingTileSettings;
      StarbucksPendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<StarbucksPendingTileSettings>();
      starbucksCardViewModel.pendingTileSettings = pendingSettingsAsync;
      starbucksCardViewModel = (EditStarbucksCardViewModel) null;
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.ShowAppBar();
    }

    public void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand((Action) (async () =>
    {
      if (this.CardNumber == null)
        return;
      try
      {
        if (this.CardNumber.Length == 16)
        {
          this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
          this.LoadState = LoadState.Loading;
          this.pendingTileSettings.CardNumber = this.CardNumber;
          this.navService.GoBack();
        }
        else
        {
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.StarbucksCardError, AppResources.StarbucksCardErrorTitle, PortableMessageBoxButton.OK);
        }
      }
      catch (Exception ex)
      {
        EditStarbucksCardViewModel.Logger.Error(ex, "Exception encountered while trying to save Starbucks card");
        this.ShowAppBar();
      }
      finally
      {
        ApplicationTelemetry.LogStarbucksCardAdd();
        this.LoadState = LoadState.Loaded;
      }
    })));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (() => this.navService.GoBack())));
  }
}
