// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.FinanceSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.TileSettings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class FinanceSettingsViewModel : SettingsViewModelBase<FinancePendingTileSettings>
  {
    private readonly ISmoothNavService navService;
    private IList<Stock> stocks;

    public FinanceSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManagementService,
      IErrorHandlingService cargoExceptionUtils,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManagementService, cargoExceptionUtils, navService, cargoConnectionFactory)
    {
      this.navService = navService;
    }

    public override bool CanEdit => true;

    public override string TileGuid => "5992928a-bd79-4bb5-9678-f08246d03e68";

    public IList<Stock> Stocks
    {
      get => this.stocks;
      set => this.SetProperty<IList<Stock>>(ref this.stocks, value, nameof (Stocks));
    }

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      this.Header = AppResources.FinanceSettings;
      this.Subheader = AppResources.FinanceSettingsCustomizeStocks;
      this.Stocks = this.PendingTileSettings.Stocks;
    }

    protected override void OnBackNavigation()
    {
      this.Refresh();
      base.OnBackNavigation();
    }

    protected override void OnEdit() => this.navService.Navigate(typeof (EditStocksViewModel));
  }
}
