// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.Clients.Bing.BingFinanceClient
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Http.Clients.Bing
{
  public class BingFinanceClient
  {
    private const string FinanceUrlFormat = "{0}/Market.svc/RealTimeQuotes?symbols={1}&clientType=projectk";
    private const string FinanceCompanyLookupUrlFormat = "{0}/Market.svc/MTAutocomplete?q={1}&locale=en:us&count={2}&clientType=projectk";
    private readonly Uri financeHost;
    private readonly HttpClient httpClient;

    public BingFinanceClient(HttpMessageHandler messageHandler, Uri financeHost)
    {
      this.httpClient = new HttpClient(messageHandler);
      this.financeHost = financeHost;
    }

    public async Task<IList<Stock>> GetStockInformationAsync(
      ICollection<string> stockIds,
      CancellationToken cancellationToken)
    {
      return await (await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, this.GetFinanceUrl((IEnumerable<string>) stockIds)), cancellationToken).ConfigureAwait(false)).ReadJsonAsync<IList<Stock>>();
    }

    public async Task<SearchStocksResult> SearchStocksAsync(
      string query,
      int maxCount,
      CancellationToken cancellationToken)
    {
      return await (await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/Market.svc/MTAutocomplete?q={1}&locale=en:us&count={2}&clientType=projectk", new object[3]
      {
        (object) this.financeHost,
        (object) query,
        (object) maxCount
      })), cancellationToken).ConfigureAwait(false)).ReadJsonAsync<SearchStocksResult>();
    }

    private string GetFinanceUrl(IEnumerable<string> stockIds) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}/Market.svc/RealTimeQuotes?symbols={1}&clientType=projectk", new object[2]
    {
      (object) this.financeHost,
      (object) Uri.EscapeDataString(string.Join(",", stockIds))
    });
  }
}
