// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileUpdateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class TileUpdateService : ITileUpdateService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileUpdateService.cs");
    private readonly IWeatherService weatherService;
    private readonly IFinanceService financeService;
    private readonly ICalendarService calendarService;
    private readonly IGeolocationService geolocationService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IUserProfileService userProfileService;
    private readonly IConfig config;
    private readonly IEnvironmentService environmentService;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly ICalendarTileAggregationService calendarTileAggregationService;
    private readonly IPermissionService permissionService;
    private readonly ITileManagementService tileManagementService;

    public TileUpdateService(
      IWeatherService weatherService,
      IFinanceService financeService,
      ICalendarService calendarService,
      IGeolocationService geolocationService,
      IBandConnectionFactory cargoConnectionFactory,
      IUserProfileService userProfileService,
      IConfig config,
      IEnvironmentService environmentService,
      IDynamicConfigurationService dynamicConfigurationService,
      ICalendarTileAggregationService calendarTileAggregationService,
      IPermissionService permissionService,
      ITileManagementService tileManagementService)
    {
      this.weatherService = weatherService;
      this.financeService = financeService;
      this.calendarService = calendarService;
      this.geolocationService = geolocationService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.userProfileService = userProfileService;
      this.config = config;
      this.environmentService = environmentService;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.calendarTileAggregationService = calendarTileAggregationService;
      this.permissionService = permissionService;
      this.tileManagementService = tileManagementService;
    }

    public async Task UpdateWeatherAsync(CancellationToken token)
    {
      bool flag = !this.config.IsWeatherEnabled;
      if (!flag)
        flag = !await this.permissionService.CheckPermissionsAsync(FeaturePermissions.Weather);
      if (flag)
        return;
      PortableGeoposition? position = new PortableGeoposition?();
      try
      {
        position = new PortableGeoposition?(await this.geolocationService.GetGeopositionAsync(token));
      }
      catch (Exception ex)
      {
        if (ex.HResult == -2147467260)
          TileUpdateService.Logger.Error(ex, "Could not get location for weather, is disabled in phone settings.");
        else
          TileUpdateService.Logger.Error(ex, "Unknown problem trying to get location for weather");
      }
      if (!position.HasValue)
        return;
      try
      {
        double latitude = position.Value.Latitude;
        double longitude = position.Value.Longitude;
        IList<WeatherDay> dailyConditions = await this.weatherService.GetDailyWeatherAsync(latitude, longitude, this.userProfileService.TemperatureUnitType, token);
        string location = await this.weatherService.GetLocationNameAsync(latitude, longitude, token);
        if (this.environmentService.IsPublicRelease)
          TileUpdateService.Logger.Debug((object) "Sending weather update to tile.");
        else
          TileUpdateService.Logger.Debug((object) ("Sending weather update for " + (object) latitude + ", " + (object) longitude + ": " + location));
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
          await cargoConnection.SendWeatherUpdateNotificationsAsync(dailyConditions, location, DateTimeOffset.UtcNow, token);
        dailyConditions = (IList<WeatherDay>) null;
        location = (string) null;
      }
      catch (TileNotFoundException ex)
      {
        TileUpdateService.Logger.Debug((object) "Weather tile not found. Disabling it in cache.");
        this.config.IsWeatherEnabled = false;
      }
    }

    public async Task UpdateFinanceAsync(CancellationToken token)
    {
      if (!this.config.IsFinanceEnabled)
        return;
      try
      {
        IList<Stock> stocks = await this.financeService.GetStockInformationAsync((IList<string>) (await this.tileManagementService.GetPendingSettingsAsync<FinancePendingTileSettings>()).Stocks.Select<Stock, string>((Func<Stock, string>) (stock => stock.ID)).ToList<string>(), token);
        if (stocks.Any<Stock>())
        {
          TileUpdateService.Logger.Debug((object) ("Sending finance update for " + string.Join(",", stocks.Select<Stock, string>((Func<Stock, string>) (s => s.Symbol)))));
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
            await cargoConnection.SendFinanceUpdateNotificationsAsync((IEnumerable<Stock>) stocks, DateTimeOffset.UtcNow, token);
        }
        else
          TileUpdateService.Logger.Debug((object) ("No finance updates for " + string.Join(",", stocks.Select<Stock, string>((Func<Stock, string>) (s => s.ID)))));
        stocks = (IList<Stock>) null;
      }
      catch (TileNotFoundException ex)
      {
        TileUpdateService.Logger.Debug((object) "Finance tile not found. Disabling it in cache.");
        this.config.IsFinanceEnabled = false;
      }
    }

    public async Task UpdateCalendarAsync(CancellationToken token)
    {
      bool flag = !this.config.IsCalendarEnabled;
      if (!flag)
        flag = !await this.permissionService.CheckPermissionsAsync(FeaturePermissions.Calendar);
      if (flag)
        return;
      try
      {
        IList<CalendarEvent> calendarEvents = await this.calendarTileAggregationService.GetCalendarEventsAsync(token);
        if (await this.calendarService.GetCalendarEventsHasChangedAsync(calendarEvents))
        {
          TileUpdateService.Logger.Debug((object) string.Format("Sending calendar update with [{0}] events", new object[1]
          {
            (object) calendarEvents.Count
          }));
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
          {
            await cargoConnection.SendCalendarEventsAsync((IEnumerable<CalendarEvent>) calendarEvents, token);
            this.calendarService.SaveLastSentCalendarEvents(calendarEvents);
          }
        }
        else
          TileUpdateService.Logger.Debug((object) "Skipping calendar sync since cached data has not changed.");
        calendarEvents = (IList<CalendarEvent>) null;
      }
      catch (Exception ex)
      {
        this.calendarService.SaveLastSentCalendarEvents((IList<CalendarEvent>) new List<CalendarEvent>());
        if (ex is TileNotFoundException)
        {
          TileUpdateService.Logger.Debug((object) "Calendar tile not found. Disabling it in cache.");
          this.config.IsCalendarEnabled = false;
        }
        else
          throw;
      }
    }

    public async Task UpdateStarbucksAsync(CancellationToken token)
    {
      if (!this.config.IsStarbucksEnabled)
        return;
      try
      {
        StarbucksPendingTileSettings settings = await this.tileManagementService.GetPendingSettingsAsync<StarbucksPendingTileSettings>();
        if (this.environmentService.IsPublicRelease)
          TileUpdateService.Logger.Debug((object) "Sending starbucks update");
        else
          TileUpdateService.Logger.Debug((object) ("Sending starbucks update for " + settings.CardNumber));
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
          await cargoConnection.SendStarbucksUpdateNotificationsAsync(settings.CardNumber, token);
        settings = (StarbucksPendingTileSettings) null;
      }
      catch (TileNotFoundException ex)
      {
        TileUpdateService.Logger.Debug((object) "Starbucks tile not found. Disabling it in cache.");
        this.config.IsStarbucksEnabled = false;
      }
    }

    public async Task UpdateTilesAsync(
      CancellationToken cancellationToken,
      SyncDebugResult syncResult = null,
      ITimedTelemetryEvent timedTelemetryEvent = null)
    {
      long weatherElapsed = 0;
      long financeElapsed = 0;
      long calendarElapsed = 0;
      List<Exception> exceptions = new List<Exception>();
      Func<CancellationToken, Task>[] funcArray = new Func<CancellationToken, Task>[4]
      {
        (Func<CancellationToken, Task>) (async token =>
        {
          if (!this.config.IsWeatherEnabled)
            return;
          // ISSUE: variable of a compiler-generated type
          TileUpdateService.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180;
          // ISSUE: reference to a compiler-generated field
          long weatherElapsed1 = cDisplayClass180.weatherElapsed;
          long num = await this.DoUpdateTileAsync((IList<Exception>) exceptions, "Weather", this.UpdateWeatherAsync(token), timedTelemetryEvent);
          // ISSUE: reference to a compiler-generated field
          cDisplayClass180.weatherElapsed = num;
          cDisplayClass180 = (TileUpdateService.\u003C\u003Ec__DisplayClass18_0) null;
        }),
        (Func<CancellationToken, Task>) (async token =>
        {
          if (!this.config.IsFinanceEnabled)
            return;
          // ISSUE: variable of a compiler-generated type
          TileUpdateService.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180;
          // ISSUE: reference to a compiler-generated field
          long financeElapsed1 = cDisplayClass180.financeElapsed;
          long num = await this.DoUpdateTileAsync((IList<Exception>) exceptions, "Finance", this.UpdateFinanceAsync(token), timedTelemetryEvent);
          // ISSUE: reference to a compiler-generated field
          cDisplayClass180.financeElapsed = num;
          cDisplayClass180 = (TileUpdateService.\u003C\u003Ec__DisplayClass18_0) null;
        }),
        (Func<CancellationToken, Task>) (async token =>
        {
          if (!this.config.IsCalendarEnabled)
            return;
          // ISSUE: variable of a compiler-generated type
          TileUpdateService.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180;
          // ISSUE: reference to a compiler-generated field
          long calendarElapsed1 = cDisplayClass180.calendarElapsed;
          long num = await this.DoUpdateTileAsync((IList<Exception>) exceptions, "Calendar", this.UpdateCalendarAsync(token), timedTelemetryEvent);
          // ISSUE: reference to a compiler-generated field
          cDisplayClass180.calendarElapsed = num;
          cDisplayClass180 = (TileUpdateService.\u003C\u003Ec__DisplayClass18_0) null;
        }),
        (Func<CancellationToken, Task>) (async token =>
        {
          if (!this.config.IsStarbucksEnabled)
            return;
          long num = await this.DoUpdateTileAsync((IList<Exception>) exceptions, "Starbucks", this.UpdateStarbucksAsync(token), timedTelemetryEvent);
        })
      };
      for (int index = 0; index < funcArray.Length; ++index)
      {
        Func<CancellationToken, Task> func = funcArray[index];
        if (!cancellationToken.IsCancellationRequested)
        {
          try
          {
            await func(cancellationToken);
          }
          catch (Exception ex)
          {
            exceptions.Add(ex);
          }
        }
      }
      funcArray = (Func<CancellationToken, Task>[]) null;
      if (syncResult != null)
      {
        syncResult.Weather = weatherElapsed;
        syncResult.Finance = financeElapsed;
        syncResult.Calendar = calendarElapsed;
      }
      if (exceptions.Any<Exception>())
        throw new AggregateException("Error(s) encountered updating tiles.", (IEnumerable<Exception>) exceptions);
    }

    private async Task<long> DoUpdateTileAsync(
      IList<Exception> exceptions,
      string tileName,
      Task task,
      ITimedTelemetryEvent timedTelemetryEvent)
    {
      Stopwatch updateTimer = Stopwatch.StartNew();
      try
      {
        TileUpdateService.Logger.Debug((object) ("<START> update tiles : " + tileName));
        await task;
        TileUpdateService.Logger.Debug((object) ("<END> update tiles : " + tileName));
      }
      catch (Exception ex)
      {
        exceptions.Add(ex);
      }
      updateTimer.Stop();
      timedTelemetryEvent?.AddMetric(tileName, (double) updateTimer.ElapsedMilliseconds);
      return updateTimer.ElapsedMilliseconds;
    }

    public async Task SyncWebTilesAsync(bool forceSync, CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.SyncWebTilesAsync(forceSync, cancellationToken);
    }

    public async Task SyncWebTileAsync(Guid tileId, CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        await cargoConnection.SyncWebTileAsync(tileId, cancellationToken);
    }
  }
}
