// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.FinancePendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class FinancePendingTileSettings : PendingTileSettings
  {
    private readonly IConfig config;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private ObservableCollection<Stock> stocks;

    public FinancePendingTileSettings(
      IConfig config,
      IDynamicConfigurationService dynamicConfigurationService)
    {
      this.config = config;
      this.dynamicConfigurationService = dynamicConfigurationService;
    }

    public IList<Stock> Stocks => (IList<Stock>) this.stocks;

    public override Task LoadSettingsAsync(CancellationToken token)
    {
      if (this.config.UserStocks != null)
        this.stocks = new ObservableCollection<Stock>((IEnumerable<Stock>) this.config.UserStocks);
      if (this.stocks == null)
      {
        IReadOnlyList<Stock> defaultStockList = this.dynamicConfigurationService.Configuration.Features.Finance.DefaultStockList;
        this.stocks = new ObservableCollection<Stock>((IEnumerable<Stock>) defaultStockList);
        this.config.UserStocks = (IList<Stock>) defaultStockList.ToList<Stock>();
      }
      this.stocks.CollectionChanged += (NotifyCollectionChangedEventHandler) ((o, e) => this.IsChanged = true);
      return (Task) Task.FromResult<bool>(true);
    }

    public override Task ApplyChangesAsync()
    {
      this.config.UserStocks = this.Stocks;
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
