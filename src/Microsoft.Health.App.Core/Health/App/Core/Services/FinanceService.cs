// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.FinanceService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Http;
using Microsoft.Health.App.Core.Http.Clients.Bing;
using Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class FinanceService : IFinanceService
  {
    private const int StockCount = 20;
    private const int MaxStockSymbolLength = 8;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\FinanceService.cs");
    private readonly Lazy<BingFinanceClient> lazyFinanceClient = new Lazy<BingFinanceClient>((Func<BingFinanceClient>) (() => new BingFinanceClient(HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging(), ServiceLocator.Current.GetInstance<IDynamicConfigurationService>().Configuration.Features.Finance.ServiceUrl)));

    public async Task<IList<Microsoft.Health.App.Core.Band.Stock>> GetStockInformationAsync(
      IList<string> stockIds,
      CancellationToken cancellationToken)
    {
      if (!stockIds.Any<string>())
        return (IList<Microsoft.Health.App.Core.Band.Stock>) new Microsoft.Health.App.Core.Band.Stock[0];
      IList<Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance.Stock> stockList1 = await this.lazyFinanceClient.Value.GetStockInformationAsync((ICollection<string>) stockIds, cancellationToken).ConfigureAwait(false);
      List<Microsoft.Health.App.Core.Band.Stock> stockList2 = new List<Microsoft.Health.App.Core.Band.Stock>();
      if (stockList1 == null)
        return (IList<Microsoft.Health.App.Core.Band.Stock>) stockList2;
      for (int index = 0; index < stockList1.Count; ++index)
      {
        Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance.Stock stock = stockList1[index];
        if (stock == null)
        {
          if (index < stockIds.Count)
            FinanceService.Logger.Error((object) ("Stock symbol was not recognized by the service: " + stockIds[index]));
          else
            FinanceService.Logger.Error((object) "A stock symbol was not recognized by the service.");
        }
        else
        {
          string symbol = stock.Eqsm;
          if (!string.IsNullOrEmpty(symbol) && symbol.Length > 0 && (symbol[0] == '!' || symbol[0] == '$'))
            symbol = stock.FrNm;
          if (!string.IsNullOrEmpty(symbol) && symbol.Length > 8)
            symbol = symbol.Substring(0, 8);
          double lp = stock.Lp;
          double chp = stock.Chp;
          stockList2.Add(new Microsoft.Health.App.Core.Band.Stock(string.Empty, symbol)
          {
            Value = lp,
            Change = chp
          });
        }
      }
      return (IList<Microsoft.Health.App.Core.Band.Stock>) stockList2;
    }

    public async Task<IList<Microsoft.Health.App.Core.Band.Stock>> SearchStocksAsync(
      string query,
      CancellationToken cancellationToken)
    {
      List<Microsoft.Health.App.Core.Band.Stock> stocks = new List<Microsoft.Health.App.Core.Band.Stock>();
      cancellationToken.ThrowIfCancellationRequested();
      SearchStocksResult searchStocksResult = await this.lazyFinanceClient.Value.SearchStocksAsync(query, 20, cancellationToken).ConfigureAwait(false);
      cancellationToken.ThrowIfCancellationRequested();
      if (searchStocksResult.Data != null)
      {
        foreach (Datum datum in (IEnumerable<Datum>) searchStocksResult.Data)
        {
          string symbol = !(datum.OS010 == "XI") ? datum.OS001 : datum.FriendlyName;
          if (!string.IsNullOrEmpty(symbol) && symbol.Length > 8)
            symbol = symbol.Substring(0, 8);
          string os01W = datum.OS01W;
          string fullInstrument = datum.FullInstrument;
          string ac040 = datum.AC040;
          string stockTypeDisplayName = FinanceService.GetStockTypeDisplayName(datum.OS010);
          stocks.Add(new Microsoft.Health.App.Core.Band.Stock(fullInstrument, symbol)
          {
            Name = os01W,
            Exchange = ac040,
            Type = stockTypeDisplayName
          });
        }
      }
      return (IList<Microsoft.Health.App.Core.Band.Stock>) stocks;
    }

    private static string GetStockTypeDisplayName(string stockType)
    {
      if (stockType.Equals("ST"))
        return AppResources.StockTypeST;
      if (stockType.Equals("FO"))
        return AppResources.StockTypeFO;
      if (stockType.Equals("FE"))
        return AppResources.StockTypeFE;
      return stockType.Equals("XI") ? AppResources.StockTypeXI : string.Empty;
    }
  }
}
