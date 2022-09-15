// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.WeatherService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Http;
using Microsoft.Health.App.Core.Http.Clients.Bing;
using Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather;
using Microsoft.Health.App.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class WeatherService : IWeatherService
  {
    private BingWeatherClient weatherClient = new BingWeatherClient(HttpMessageHandlerFactory.CreateMessageHandler().WithHttpLogging());
    private static readonly IDictionary<ushort, WeatherService.WeatherInfo> WeatherInfoMap = (IDictionary<ushort, WeatherService.WeatherInfo>) ((IEnumerable<WeatherService.WeatherInfo>) new WeatherService.WeatherInfo[98]
    {
      new WeatherService.WeatherInfo((ushort) 1, AppResources.WeatherCaptionClear, (ushort) 0),
      new WeatherService.WeatherInfo((ushort) 2, AppResources.WeatherCaptionClear, (ushort) 0),
      new WeatherService.WeatherInfo((ushort) 3, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 4, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 5, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 6, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 7, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 8, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 9, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 10, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 11, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 12, AppResources.WeatherCaptionHaze, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 13, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 14, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 15, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 16, AppResources.WeatherCaptionIce, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 17, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 18, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 19, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 20, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 21, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 22, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 23, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 24, AppResources.WeatherCaptionSleet, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 25, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 26, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 27, AppResources.WeatherCaptionThunderstorm, (ushort) 4),
      new WeatherService.WeatherInfo((ushort) 28, AppResources.WeatherCaptionClear, (ushort) 1),
      new WeatherService.WeatherInfo((ushort) 29, AppResources.WeatherCaptionClear, (ushort) 1),
      new WeatherService.WeatherInfo((ushort) 30, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 31, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 32, AppResources.WeatherCaptionCloudy, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 33, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 34, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 35, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 36, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 37, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 38, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 39, AppResources.WeatherCaptionHaze, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 40, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 41, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 42, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 43, AppResources.WeatherCaptionIce, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 44, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 45, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 46, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 47, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 48, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 49, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 50, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 51, AppResources.WeatherCaptionSleet, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 52, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 53, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 54, AppResources.WeatherCaptionThunderstorm, (ushort) 4),
      new WeatherService.WeatherInfo((ushort) 55, string.Empty, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 56, string.Empty, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 57, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 58, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 59, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 60, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 61, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 62, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 63, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 64, AppResources.WeatherCaptionFog, (ushort) 7),
      new WeatherService.WeatherInfo((ushort) 65, AppResources.WeatherCaptionHail, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 66, AppResources.WeatherCaptionHail, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 67, AppResources.WeatherCaptionThunderstorm, (ushort) 4),
      new WeatherService.WeatherInfo((ushort) 68, AppResources.WeatherCaptionThunderstorm, (ushort) 4),
      new WeatherService.WeatherInfo((ushort) 69, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 70, AppResources.WeatherCaptionDrizzle, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 71, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 72, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 73, AppResources.WeatherCaptionHail, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 74, AppResources.WeatherCaptionHail, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 75, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 76, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 77, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 78, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 79, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 80, AppResources.WeatherCaptionRain, (ushort) 3),
      new WeatherService.WeatherInfo((ushort) 81, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 82, AppResources.WeatherCaptionSnow, (ushort) 6),
      new WeatherService.WeatherInfo((ushort) 83, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 84, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 85, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 86, AppResources.WeatherCaptionRainSnow, (ushort) 5),
      new WeatherService.WeatherInfo((ushort) 87, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 88, AppResources.WeatherCaptionDust, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 89, AppResources.WeatherCaptionSmoke, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 90, AppResources.WeatherCaptionSmoke, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 91, AppResources.WeatherCaptionSquall, (ushort) 9),
      new WeatherService.WeatherInfo((ushort) 92, AppResources.WeatherCaptionSquall, (ushort) 9),
      new WeatherService.WeatherInfo((ushort) 93, AppResources.WeatherCaptionSand, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 94, AppResources.WeatherCaptionSand, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 95, AppResources.WeatherCaptionSand, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 96, AppResources.WeatherCaptionSand, (ushort) 8),
      new WeatherService.WeatherInfo((ushort) 101, AppResources.WeatherCaptionFair, (ushort) 2),
      new WeatherService.WeatherInfo((ushort) 102, AppResources.WeatherCaptionFair, (ushort) 2)
    }).ToDictionary<WeatherService.WeatherInfo, ushort>((Func<WeatherService.WeatherInfo, ushort>) (x => x.IconCode));

    public async Task<IList<Microsoft.Health.App.Core.Band.WeatherDay>> GetDailyWeatherAsync(
      double latitude,
      double longitude,
      TemperatureUnitType units,
      CancellationToken token)
    {
      WeatherSummary weatherSummary = await this.weatherClient.GetDailyWeatherAsync(latitude, longitude, WeatherService.ToWeatherTemperatureUnit(units), token).ConfigureAwait(false);
      WeatherSummaryResponse weatherSummaryResponse = weatherSummary == null || weatherSummary.Responses == null ? (WeatherSummaryResponse) null : weatherSummary.Responses.FirstOrDefault<WeatherSummaryResponse>();
      Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherInfo weatherInfo1 = weatherSummaryResponse == null || weatherSummaryResponse.Weather == null ? (Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherInfo) null : weatherSummaryResponse.Weather.FirstOrDefault<Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherInfo>();
      CurrentWeather current = weatherInfo1?.Current;
      IList<Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherDay> weatherDayList1 = weatherInfo1 == null || weatherInfo1.Forecast == null ? (IList<Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherDay>) null : weatherInfo1.Forecast.Days;
      List<Microsoft.Health.App.Core.Band.WeatherDay> weatherDayList2 = new List<Microsoft.Health.App.Core.Band.WeatherDay>();
      if (current != null)
      {
        short temp = (short) current.Temp;
        ushort icon = (ushort) current.Icon;
        ushort num = 2;
        string str = string.Empty;
        WeatherService.WeatherInfo weatherInfo2;
        if (WeatherService.WeatherInfoMap.TryGetValue(icon, out weatherInfo2))
        {
          num = weatherInfo2.IconId;
          str = weatherInfo2.Caption;
        }
        weatherDayList2.Add(new Microsoft.Health.App.Core.Band.WeatherDay()
        {
          High = temp,
          IconId = (uint) num,
          Caption = str
        });
      }
      if (weatherDayList1 != null)
      {
        foreach (Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherDay weatherDay in (IEnumerable<Microsoft.Health.App.Core.Http.Clients.Bing.Models.Weather.WeatherDay>) weatherDayList1)
        {
          short tempHi = (short) weatherDay.TempHi;
          short tempLo = (short) weatherDay.TempLo;
          DateTimeOffset valid = weatherDay.Valid;
          ushort icon = (ushort) weatherDay.Icon;
          ushort num = 2;
          WeatherService.WeatherInfo weatherInfo3;
          if (WeatherService.WeatherInfoMap.TryGetValue(icon, out weatherInfo3))
            num = weatherInfo3.IconId;
          weatherDayList2.Add(new Microsoft.Health.App.Core.Band.WeatherDay()
          {
            Date = valid,
            IconId = (uint) num,
            High = tempHi,
            Low = tempLo
          });
        }
      }
      return (IList<Microsoft.Health.App.Core.Band.WeatherDay>) weatherDayList2;
    }

    public async Task<string> GetLocationNameAsync(
      double latitude,
      double longitude,
      CancellationToken token)
    {
      LocationsSearchResult locationsSearchResult = await this.weatherClient.GetLocationsAsync(latitude, longitude, token).ConfigureAwait(false);
      LocationsSearchResponse locationsSearchResponse = locationsSearchResult.Responses != null ? locationsSearchResult.Responses.FirstOrDefault<LocationsSearchResponse>() : (LocationsSearchResponse) null;
      return (locationsSearchResponse == null || locationsSearchResponse.Locations == null ? (Location) null : locationsSearchResponse.Locations.FirstOrDefault<Location>())?.DisplayName;
    }

    private static TemperatureUnit ToWeatherTemperatureUnit(TemperatureUnitType units)
    {
      if (units == TemperatureUnitType.Imperial)
        return TemperatureUnit.Imperial;
      if (units == TemperatureUnitType.Metric)
        return TemperatureUnit.Metric;
      throw new InvalidOperationException("Unsupported unit " + (object) units);
    }

    private static class WeatherIconIds
    {
      public const ushort ClearDay = 0;
      public const ushort ClearNight = 1;
      public const ushort Cloudy = 2;
      public const ushort Rain = 3;
      public const ushort Lightning = 4;
      public const ushort RainSnow = 5;
      public const ushort Snow = 6;
      public const ushort Fog = 7;
      public const ushort Smoke = 8;
      public const ushort Squal = 9;
      public const ushort Default = 2;
    }

    private class WeatherInfo
    {
      public WeatherInfo(ushort iconCode, string caption, ushort iconId)
      {
        this.IconCode = iconCode;
        this.Caption = caption;
        this.IconId = iconId;
      }

      public ushort IconCode { get; private set; }

      public string Caption { get; private set; }

      public ushort IconId { get; private set; }
    }
  }
}
