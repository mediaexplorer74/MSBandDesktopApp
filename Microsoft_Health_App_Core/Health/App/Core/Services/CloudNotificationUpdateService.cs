// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.CloudNotificationUpdateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class CloudNotificationUpdateService : ICloudNotificationUpdateService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\CloudNotificationUpdateService.cs");
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IEnvironmentService environmentService;
    private readonly IWorkoutsProvider workoutsProvider;
    private readonly ICalendarService calendarService;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly ICoachingService coachingService;
    private readonly ICalendarTileAggregationService tileAggregationService;
    private readonly IEventWaitHandleService eventWaitHandlerService;
    public const string BackgroundUpdateEventWaitHandle = "BackgroundUpdateEventWaitHandle";
    private bool updatedTimeline;

    public CloudNotificationUpdateService(
      IHealthCloudClient healthCloudClient,
      IEnvironmentService environmentService,
      IWorkoutsProvider workoutsProvider,
      ICalendarService calendarService,
      IBandConnectionFactory cargoConnectionFactory,
      ICoachingService coachingService,
      ICalendarTileAggregationService tileAggregationService,
      IEventWaitHandleService eventWaitHandlerService)
    {
      Assert.ParamIsNotNull((object) healthCloudClient, nameof (healthCloudClient));
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      Assert.ParamIsNotNull((object) workoutsProvider, nameof (workoutsProvider));
      Assert.ParamIsNotNull((object) calendarService, nameof (calendarService));
      Assert.ParamIsNotNull((object) cargoConnectionFactory, nameof (cargoConnectionFactory));
      this.healthCloudClient = healthCloudClient;
      this.environmentService = environmentService;
      this.workoutsProvider = workoutsProvider;
      this.calendarService = calendarService;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.coachingService = coachingService;
      this.tileAggregationService = tileAggregationService;
      this.eventWaitHandlerService = eventWaitHandlerService;
    }

    public async Task HandleAvailableUpdatesAsync(CancellationToken cancellationToken)
    {
      UpdatesResponse updatesAsync = await this.healthCloudClient.GetUpdatesAsync(this.environmentService.PhoneId, cancellationToken);
      this.updatedTimeline = false;
      CloudNotificationUpdateService.Logger.Info((object) string.Format("<FLAG> Retrieved Updates : {0} updates to process", new object[1]
      {
        (object) updatesAsync.Updates.Count
      }));
      List<string> succesfulUpdates = new List<string>(updatesAsync.Updates.Count);
      try
      {
        foreach (Update update in (IEnumerable<Update>) updatesAsync.Updates)
        {
          await this.RunUpdateTaskAsync(update, cancellationToken);
          succesfulUpdates.Add(update.Id);
        }
      }
      catch (Exception ex)
      {
        CloudNotificationUpdateService.Logger.Warn((object) string.Format("Exception while handling cloud notification updates {0}", new object[1]
        {
          (object) ex
        }));
        throw;
      }
      finally
      {
        if (succesfulUpdates.Count > 0)
          await this.ClearUpdatesAsync((IList<string>) succesfulUpdates, cancellationToken);
      }
    }

    public async Task ClearUpdatesAsync(
      IList<string> updateIds,
      CancellationToken cancellationToken)
    {
      string updateIds1 = "*";
      if (updateIds != null && updateIds.Count > 0)
        updateIds1 = string.Join(",", (IEnumerable<string>) updateIds);
      await this.healthCloudClient.DeleteUpdatesAsync(this.environmentService.PhoneId, updateIds1, cancellationToken);
    }

    private async Task RunUpdateTaskAsync(Update task, CancellationToken cancellationToken)
    {
      if (task == null)
        return;
      switch (task.TypeId)
      {
        case UpdateType.PlanUpdate:
        case UpdateType.TimelineUpdate:
          if (this.updatedTimeline)
          {
            CloudNotificationUpdateService.Logger.Debug((object) "<SKIP> update service : Already processed Plan Update for this batch of notifications.");
            break;
          }
          this.updatedTimeline = true;
          CloudNotificationUpdateService.Logger.Debug((object) "<START> update service : Process Plan Update");
          await this.UpdateCalendarAsync(cancellationToken);
          await this.coachingService.SyncSpecialRemindersAsync(cancellationToken).ConfigureAwait(false);
          await this.coachingService.SyncGoalsToBandAsync(cancellationToken).ConfigureAwait(false);
          CloudNotificationUpdateService.Logger.Debug((object) "<END> update service : Process Plan Update");
          goto case UpdateType.GoalUpdate;
        case UpdateType.GoalUpdate:
        case UpdateType.CustomWorkoutUpdate:
        case UpdateType.Registration:
        case UpdateType.Message:
          this.OnPushNotificationEvent(task);
          break;
        default:
          throw new ArgumentException(string.Format("The '{0}' update type is not supported.", new object[1]
          {
            (object) task.TypeId
          }), "update");
      }
    }

    private async Task UpdateCalendarAsync(CancellationToken token)
    {
      IList<CalendarEvent> calendarEvents = await this.tileAggregationService.GetCalendarEventsAsync(token);
      CloudNotificationUpdateService.Logger.Debug((object) string.Format("Sending calendar update with [{0}] events", new object[1]
      {
        (object) calendarEvents.Count
      }));
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
        await cargoConnection.SendCalendarEventsAsync((IEnumerable<CalendarEvent>) calendarEvents, token);
      this.calendarService.SaveLastSentCalendarEvents(calendarEvents);
    }

    protected virtual void OnPushNotificationEvent(Update updateType)
    {
      EventWaitHandle result;
      if (!this.eventWaitHandlerService.TryOpenExisting("BackgroundUpdateEventWaitHandle", out result))
        return;
      result.Set();
    }
  }
}
