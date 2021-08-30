// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.EditStocksViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles", "Stocks", "Watchlist"})]
  public class EditStocksViewModel : PageViewModelBase
  {
    private const int MaxStocks = 7;
    private const int MinStocks = 1;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\EditStocksViewModel.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileManagementService tileManagementService;
    private readonly ISmoothNavService navService;
    private HealthCommand addCommand;
    private HealthCommand<Stock> removeCommand;
    private FinancePendingTileSettings pendingTileSettings;
    private IList<Stock> stocks;
    private bool reorderEnabled;
    private bool suppressReorderDisabledNavigation;

    public EditStocksViewModel(
      ISmoothNavService navService,
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      ITileManagementService tileManagementService)
      : base(networkService)
    {
      this.navService = navService;
      this.messageBoxService = messageBoxService;
      this.tileManagementService = tileManagementService;
      this.ReorderEnabled = true;
    }

    public IList<Stock> Stocks
    {
      get => this.stocks;
      set => this.SetProperty<IList<Stock>>(ref this.stocks, value, nameof (Stocks));
    }

    public bool ReorderEnabled
    {
      get => this.reorderEnabled;
      set
      {
        this.SetProperty<bool>(ref this.reorderEnabled, value, nameof (ReorderEnabled));
        if (value || this.suppressReorderDisabledNavigation)
          return;
        this.navService.GoBack();
      }
    }

    public ICommand AddCommand => (ICommand) this.addCommand ?? (ICommand) (this.addCommand = new HealthCommand(new Action(this.Add)));

    private async void Add()
    {
      if (this.Stocks.Count < 7)
      {
        this.navService.Navigate(typeof (AddStocksViewModel));
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorMaxStocksBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
      }
    }

    public ICommand RemoveCommand => (ICommand) this.removeCommand ?? (ICommand) (this.removeCommand = new HealthCommand<Stock>((Action<Stock>) (async stock =>
    {
      if (this.Stocks != null && this.Stocks.Count > 1)
        this.Stocks.Remove(stock);
      else
        await this.DisplayWarningAsync();
    })));

    private async Task DisplayWarningAsync()
    {
      this.suppressReorderDisabledNavigation = true;
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorMinStocksBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
      this.ReorderEnabled = true;
      this.suppressReorderDisabledNavigation = false;
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EditStocksViewModel editStocksViewModel = this;
      FinancePendingTileSettings pendingTileSettings = editStocksViewModel.pendingTileSettings;
      FinancePendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<FinancePendingTileSettings>();
      editStocksViewModel.pendingTileSettings = pendingSettingsAsync;
      editStocksViewModel = (EditStocksViewModel) null;
      this.Stocks = this.pendingTileSettings.Stocks;
    }

    protected override async void OnBackNavigation()
    {
      base.OnBackNavigation();
      await this.LoadAsync((IDictionary<string, string>) null);
    }
  }
}
