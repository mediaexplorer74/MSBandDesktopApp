// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.Clients.Bing.BingWeatherClient
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Http.Clients.Bing
{
  public class BingWeatherClient
  {
    private const string WeatherAppId = "appid=3FB8A36C-B005-4332-96F1-CAFAD7A25D2C&formcode=KAPP";
    private const string WeatherBaseUrl = "http://service.weather.microsoft.com/";
    private const string WeatherLocationPath = "locations/search";
    private const string WeatherLocationUrlFormat = "http://service.weather.microsoft.com/locations/search/{0:F4},{1:F4}?appid=3FB8A36C-B005-4332-96F1-CAFAD7A25D2C&formcode=KAPP";
    private const string WeatherPath = "weather/summary";
    private const string WeatherUrlFormat = "http://service.weather.microsoft.com/weather/summary/{0},{1}?days={2}&units={3}&appid=3FB8A36C-B005-4332-96F1-CAFAD7A25D2C&formcode=KAPP";
    private const int WeatherDayCount = 6;
    private readonly HttpClient httpClient;

    public BingWeatherClient(HttpMessageHandler messageHandler) => this.httpClient = new HttpClient(messageHandler);

    public async Task<WeatherSummary> GetDailyWeatherAsync(
      double latitude,
      double longitude,
      TemperatureUnit units,
      CancellationToken token)
    {
      return await (await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://service.weather.microsoft.com/weather/summary/{0},{1}?days={2}&units={3}&appid=3FB8A36C-B005-4332-96F1-CAFAD7A25D2C&formcode=KAPP", (object) latitude, (object) longitude, (object) 6, (object) BingWeatherClient.GetPreferredUnitsString(units))), token).ConfigureAwait(false)).ReadJsonAsync<WeatherSummary>();
    }

    public async Task<LocationsSearchResult> GetLocationsAsync(
      double latitude,
      double longitude,
      CancellationToken token)
    {
      return await (await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "http://service.weather.microsoft.com/locations/search/{0:F4},{1:F4}?appid=3FB8A36C-B005-4332-96F1-CAFAD7A25D2C&formcode=KAPP", new object[2]
      {
        (object) latitude,
        (object) longitude
      })), token).ConfigureAwait(false)).ReadJsonAsync<LocationsSearchResult>();
    }

    private static string GetPreferredUnitsString(TemperatureUnit units)
    {
      if (units == TemperatureUnit.Metric)
        return "Metric";
      if (units == TemperatureUnit.Imperial)
        return "Imperial";
      throw new InvalidOperationException("Unsupported unit " + (object) units);
    }
  }
}
