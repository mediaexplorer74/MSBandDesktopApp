// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.AddStocksViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Settings", "Manage Tiles", "Stocks", "Search"})]
  public class AddStocksViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\AddStocksViewModel.cs");
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IFinanceService financeService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileManagementService tileManagementService;
    private readonly ISmoothNavService navService;
    private readonly SemaphoreSlim searchSemaphore = new SemaphoreSlim(1);
    private CancellationTokenSource searchStocksCancellationTokenSource;
    private ICommand addStockCommand;
    private ICommand backCommand;
    private FinancePendingTileSettings pendingTileSettings;
    private string query;
    private IList<Stock> result;
    private IList<Stock> stocks;

    public AddStocksViewModel(
      IFinanceService financeService,
      ISmoothNavService navService,
      IMessageBoxService messageBoxService,
      IErrorHandlingService errorHandlingService,
      INetworkService networkService,
      ITileManagementService tileManagementService)
      : base(networkService)
    {
      this.financeService = financeService;
      this.navService = navService;
      this.messageBoxService = messageBoxService;
      this.errorHandlingService = errorHandlingService;
      this.tileManagementService = tileManagementService;
      this.QueryChanged += new AddStocksViewModel.QueryEventHandler(this.OnQueryChanged);
    }

    public string Query
    {
      get => this.query;
      set
      {
        string query = this.query;
        this.SetProperty<string>(ref this.query, value, nameof (Query));
        if (!string.IsNullOrWhiteSpace(this.query))
        {
          if (!(query != this.query))
            return;
          AddStocksViewModel.QueryEventHandler queryChanged = this.QueryChanged;
          if (queryChanged == null)
            return;
          queryChanged();
        }
        else
          this.Result = (IList<Stock>) null;
      }
    }

    public IList<Stock> Result
    {
      get => this.result;
      set => this.SetProperty<IList<Stock>>(ref this.result, value, nameof (Result));
    }

    public IList<Stock> Stocks
    {
      get => this.stocks;
      set => this.SetProperty<IList<Stock>>(ref this.stocks, value, nameof (Stocks));
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand(new Action(this.GoBack)));

    public ICommand AddStockCommand => this.addStockCommand ?? (this.addStockCommand = (ICommand) new HealthCommand<Stock>((Action<Stock>) (async p => await this.AddStockAsync(p))));

    private void GoBack() => this.navService.GoBack();

    private event AddStocksViewModel.QueryEventHandler QueryChanged;

    protected override async void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      await this.LoadAsync((IDictionary<string, string>) null);
    }

    private async void OnQueryChanged()
    {
      try
      {
        try
        {
          this.searchStocksCancellationTokenSource?.Cancel();
          await this.searchSemaphore.WaitAsync();
          this.searchStocksCancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan);
          await Task.Delay(TimeSpan.FromMilliseconds(250.0), this.searchStocksCancellationTokenSource.Token);
          await this.SearchStocksAsync(this.Query, this.searchStocksCancellationTokenSource.Token);
        }
        finally
        {
          this.searchStocksCancellationTokenSource?.Dispose();
          this.searchStocksCancellationTokenSource = (CancellationTokenSource) null;
          this.searchSemaphore.Release();
        }
      }
      catch (OperationCanceledException ex)
      {
        AddStocksViewModel.Logger.Debug((object) "Canceled stock query");
      }
      catch (Exception ex)
      {
        AddStocksViewModel.Logger.Error(ex, "Exception encountered while trying to query the stocks");
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    }

    private async Task SearchStocksAsync(string stockQuery, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      IList<Stock> stockList = await this.financeService.SearchStocksAsync(Uri.EscapeDataString(stockQuery.Trim()), cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      this.Result = (IList<Stock>) new List<Stock>((IEnumerable<Stock>) stockList);
    }

    public async Task AddStockAsync(Stock selectedStock)
    {
      if (!this.Stocks.Contains(selectedStock))
      {
        this.Stocks.Add(selectedStock);
        this.navService.GoBack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.BandErrorDuplicateStockBody, AppResources.ApplicationTitle, PortableMessageBoxButton.OK);
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      AddStocksViewModel addStocksViewModel = this;
      FinancePendingTileSettings pendingTileSettings = addStocksViewModel.pendingTileSettings;
      FinancePendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<FinancePendingTileSettings>();
      addStocksViewModel.pendingTileSettings = pendingSettingsAsync;
      addStocksViewModel = (AddStocksViewModel) null;
      this.Stocks = this.pendingTileSettings.Stocks;
    }

    private delegate void QueryEventHandler();
  }
}
