// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.WorkoutsProviderBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class WorkoutsProviderBase : IWorkoutsProvider
  {
    protected const uint CargoStatusGWTileOpen = 2693660674;
    protected static readonly string[] BrowseWorkoutsBrandOrder = new string[6]
    {
      "microsoft",
      "gold's gym",
      "shape",
      "men's fitness",
      "muscle & fitness",
      "benchmark wod"
    };
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Providers\\WorkoutsProviderBase.cs");
    private readonly IConfig config;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IBingHealthAndFitnessClient healthAndFitnessClient;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IUserProfileService userProfileService;
    private readonly IEnvironmentService applicationEnvironmentService;
    private bool throwOnUploadWorkoutBandFileNoRetry;

    protected IConfig Config => this.config;

    protected IBingHealthAndFitnessClient HealthAndFitnessClient => this.healthAndFitnessClient;

    protected IHealthCloudClient HealthCloudClient => this.healthCloudClient;

    protected IUserProfileService UserProfileService => this.userProfileService;

    protected IEnvironmentService ApplicationEnvironmentService => this.applicationEnvironmentService;

    protected bool ThrowOnUploadWorkoutBandFileNoRetry
    {
      get => this.throwOnUploadWorkoutBandFileNoRetry;
      set => this.throwOnUploadWorkoutBandFileNoRetry = value;
    }

    public WorkoutsProviderBase(
      IConfig config,
      IBandConnectionFactory cargoConnectionFactory,
      IBingHealthAndFitnessClient healthAndFitnessClient,
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService,
      IEnvironmentService applicationEnvironmentService)
    {
      this.config = config;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.healthAndFitnessClient = healthAndFitnessClient;
      this.healthCloudClient = healthCloudClient;
      this.userProfileService = userProfileService;
      this.applicationEnvironmentService = applicationEnvironmentService;
      this.ThrowOnUploadWorkoutBandFileNoRetry = false;
    }

    public DateTimeOffset? LastSubscriptionChanged { get; private set; }

    public event EventHandler SubscriptionChanged;

    public async Task<IList<DisplaySubType>> GetWorkoutTypesAsync()
    {
      List<DisplaySubType> types = new List<DisplaySubType>();
      WorkoutSearch workoutSearch = await this.HealthAndFitnessClient.SearchWorkoutsAsync(new WorkoutSearchOptions(), CancellationToken.None).ConfigureAwait(false);
      if (workoutSearch != null && workoutSearch.WorkoutResults != null && workoutSearch.WorkoutResults.Results != null)
        types = workoutSearch.WorkoutResults.Results.Select<WorkoutSearchResult, DisplaySubType>((Func<WorkoutSearchResult, DisplaySubType>) (result => result.DisplaySubType)).Distinct<DisplaySubType>().Except<DisplaySubType>((IEnumerable<DisplaySubType>) new DisplaySubType[1]).ToList<DisplaySubType>();
      return (IList<DisplaySubType>) types;
    }

    public async Task<IList<BrowseCategory>> GetWorkoutBrandsAsync()
    {
      List<BrowseCategory> categories = new List<BrowseCategory>();
      WorkoutSearch workoutSearch = await this.HealthAndFitnessClient.SearchWorkoutsAsync(new WorkoutSearchOptions(), CancellationToken.None);
      List<string> stringList1 = new List<string>((IEnumerable<string>) WorkoutsProviderBase.BrowseWorkoutsBrandOrder);
      List<string> stringList2 = new List<string>();
      foreach (WorkoutSearchResult result in (IEnumerable<WorkoutSearchResult>) workoutSearch.WorkoutResults.Results)
      {
        if (!stringList2.Contains(result.PartnerName) && !string.IsNullOrWhiteSpace(result.PartnerName))
        {
          stringList2.Add(result.PartnerName);
          int num1 = stringList1.IndexOf(result.PartnerName.ToLowerInvariant());
          int num2 = num1 >= 0 ? num1 : workoutSearch.WorkoutResults.Count + 1;
          categories.Add(new BrowseCategory()
          {
            Name = result.PartnerName,
            Image = result.PartnerLogo,
            DisplayOrder = num2
          });
        }
      }
      return (IList<BrowseCategory>) categories.OrderBy<BrowseCategory, int>((Func<BrowseCategory, int>) (c => c.DisplayOrder)).ThenBy<BrowseCategory, string>((Func<BrowseCategory, string>) (c => c.Name)).ToList<BrowseCategory>();
    }

    public async Task<WorkoutSearch> GetWorkoutsAsync(
      CancellationToken cancellationToken,
      string query,
      WorkoutPublisher workoutPublisher,
      IList<WorkoutSearchFilter> filters)
    {
      WorkoutSearchOptions options = new WorkoutSearchOptions(filters.Select<WorkoutSearchFilter, KeyValuePair<string, string>>((Func<WorkoutSearchFilter, KeyValuePair<string, string>>) (filter => new KeyValuePair<string, string>(filter.FilterName, filter.Id))));
      if (!string.IsNullOrWhiteSpace(query))
        options.Query = query;
      options.PublishedBy = new WorkoutPublisher?(workoutPublisher);
      return await this.HealthAndFitnessClient.SearchWorkoutsAsync(options, cancellationToken) ?? new WorkoutSearch();
    }

    public async Task<WorkoutPlanDetail> GetWorkoutAsync(string id)
    {
      WorkoutPlanDetail workoutAsync;
      try
      {
        workoutAsync = await this.HealthAndFitnessClient.GetWorkoutAsync(id, CancellationToken.None);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Unable to get the workout details for {0}", (object) id);
        throw;
      }
      return workoutAsync;
    }

    public Task<IList<FavoriteWorkout>> GetFavoriteWorkoutsAsync(
      CancellationToken cancellationToken)
    {
      try
      {
        return this.HealthCloudClient.GetFavoriteWorkoutsAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error((object) "Unable to get the favorite workouts.");
        throw;
      }
    }

    public async Task FavoriteWorkoutAsync(string id)
    {
      try
      {
        await this.HealthCloudClient.FavoriteWorkoutAsync(id, CancellationToken.None);
        ApplicationTelemetry.LogWorkoutPlanFavorite(id, true);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Cloud: Unable to favorite {0}", (object) id);
        throw;
      }
    }

    public async Task UnFavoriteWorkoutAsync(string id)
    {
      try
      {
        await this.HealthCloudClient.UnFavoriteWorkoutAsync(id, CancellationToken.None);
        ApplicationTelemetry.LogWorkoutPlanFavorite(id, false);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Cloud: Unable to unfavorite {0}", (object) id);
        throw;
      }
    }

    public async Task SubscribeWorkoutAsync(string id)
    {
      try
      {
        await this.HealthCloudClient.SubscribeWorkoutAsync(id, CancellationToken.None);
        ApplicationTelemetry.LogWorkoutPlanSubscription(id, true);
        this.NotifySubscriptionChanged();
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Cloud: Unable to subscribe {0}", (object) id);
        throw;
      }
    }

    public async Task UnsubscribeWorkoutAsync(string id)
    {
      try
      {
        await this.HealthCloudClient.UnsubscribeWorkoutAsync(id, CancellationToken.None);
        ApplicationTelemetry.LogWorkoutPlanSubscription(id, false);
        this.NotifySubscriptionChanged();
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Cloud: Unable to unsubscribe {0}", (object) id);
        throw;
      }
    }

    public async Task<IList<WorkoutStatus>> GetWorkoutsInPlanAsync(
      string workoutPlanId,
      string instanceId)
    {
      IList<WorkoutStatus> workoutsInPlanAsync;
      try
      {
        workoutsInPlanAsync = await this.HealthCloudClient.GetWorkoutsInPlanAsync(workoutPlanId, instanceId, CancellationToken.None);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Unable to get status list for {0} instance {1}", (object) workoutPlanId, (object) instanceId);
        throw;
      }
      return workoutsInPlanAsync;
    }

    public Task<WorkoutStatus> GetLastSyncedWorkoutAsync(
      CancellationToken cancellationToken)
    {
      try
      {
        return this.HealthCloudClient.GetLastSyncedWorkoutAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error((object) "Unable to get the last synced workout.");
        throw;
      }
    }

    public Task SetLastSyncedWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId)
    {
      return this.HealthCloudClient.SetLastSyncedWorkoutAsync(workoutPlanId, workoutIndex, weekId, dayId, workoutPlanInstanceId, CancellationToken.None);
    }

    public Task SetNextWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId)
    {
      return this.HealthCloudClient.SetNextWorkoutAsync(workoutPlanId, workoutIndex, weekId, dayId, CancellationToken.None);
    }

    public async Task SkipWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int week,
      int day)
    {
      try
      {
        await this.HealthCloudClient.SkipWorkoutAsync(workoutPlanId, workoutIndex, week, day, CancellationToken.None);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Unable to skip workout {0}, index:{1}, week:{2}, day:{3}", (object) workoutPlanId, (object) workoutIndex, (object) week, (object) day);
        throw;
      }
    }

    public async Task<IList<FeaturedWorkout>> GetFeaturedWorkoutsAsync()
    {
      BandUserProfile currentUserProfile = this.UserProfileService.CurrentUserProfile;
      int age = 25;
      Gender gender1 = Gender.Male;
      string gender2 = gender1.ToString();
      if (currentUserProfile != null)
      {
        age = currentUserProfile.Birthdate.GetAge();
        gender1 = currentUserProfile.Gender;
        gender2 = gender1.ToString();
      }
      return await this.HealthCloudClient.GetFeaturedWorkoutsAsync(age, gender2, CancellationToken.None);
    }

    public async Task UploadWorkoutBandFileAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken,
      GuidedWorkoutSyncMode mode = GuidedWorkoutSyncMode.SyncPlan)
    {
      using (ITimedTelemetryEvent timedEvent = CommonTelemetry.TimeWorkoutSync(workoutPlanId, workoutIndex, weekId, dayId, workoutPlanInstanceId, mode))
      {
        try
        {
          using (Stream workoutStream = await this.HealthCloudClient.GetWorkoutBandFileAsync(workoutPlanId, workoutIndex, weekId, dayId, workoutPlanInstanceId, CancellationToken.None).ConfigureAwait(false))
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              await workoutStream.CopyToAsync((Stream) memoryStream).ConfigureAwait(false);
              bool retry = true;
              bool success = false;
              string failureErrorBody = AppResources.WorkoutUploadFailureAndRetry;
              Exception bandException = (Exception) null;
              while (!success & retry)
              {
                memoryStream.Seek(0L, SeekOrigin.Begin);
                try
                {
                  using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
                    await cargoConnection.SendWorkoutToBandAsync((Stream) memoryStream, CancellationToken.None);
                  success = true;
                }
                catch (Exception ex)
                {
                  BandOperationException operationException = ex.Find<BandOperationException>();
                  if (operationException != null)
                  {
                    bandException = (Exception) operationException;
                    if (operationException.HResult == 2693660674U)
                      failureErrorBody = AppResources.WorkoutUploadTileOpenFailureAndRetry;
                  }
                  else
                    throw;
                }
                if (!success)
                {
                  retry = await this.CheckRetryAsync(failureErrorBody, AppResources.WorkoutUploadFailureAndRetryTitle);
                  if (retry)
                    WorkoutsProviderBase.Logger.Debug("Retrying upload of workout to band {0}, index:{1}, week:{2}, day:{3}, instance:{4}", (object) workoutPlanId, (object) workoutIndex, (object) weekId, (object) dayId, (object) workoutPlanInstanceId);
                  else if (this.ThrowOnUploadWorkoutBandFileNoRetry)
                    throw new BandSyncException("Workout upload retry declined.", bandException, true);
                }
              }
              if (!success)
                throw new BandSyncException("Failed to upload workout to band", bandException);
              await this.SetLastSyncedWorkoutAsync(workoutPlanId, workoutIndex, weekId, dayId, workoutPlanInstanceId);
              WorkoutsProviderBase.Logger.Debug("Successful upload of workout to band {0}, index:{1}, week:{2}, day:{3}, instance:{4}", (object) workoutPlanId, (object) workoutIndex, (object) weekId, (object) dayId, (object) workoutPlanInstanceId);
              failureErrorBody = (string) null;
              bandException = (Exception) null;
            }
          }
        }
        catch (Exception ex)
        {
          Microsoft.Band.Admin.Logger.LogException(LogLevel.Error, ex);
          WorkoutsProviderBase.Logger.Error(ex, "Unable to upload workout to band {0}, index:{1}, week:{2}, day:{3}, instance:{4}", (object) workoutPlanId, (object) workoutIndex, (object) weekId, (object) dayId, (object) workoutPlanInstanceId);
          timedEvent.Cancel();
          throw;
        }
      }
    }

    public virtual async Task<bool> CheckRetryAsync(
      string failureErrorBody,
      string failureErrorTitle)
    {
      return await Task.Run<bool>((Func<bool>) (() => false));
    }

    public async Task<string> GetVideoUrlAsync(
      string id,
      double screenWidth,
      double screenHeight)
    {
      string videoUrlAsync;
      try
      {
        videoUrlAsync = await this.HealthAndFitnessClient.GetVideoUrlAsync(id, screenWidth, screenHeight, CancellationToken.None);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Unable to get video url for id {0}", (object) id);
        throw;
      }
      return videoUrlAsync;
    }

    public async Task<GuidedWorkoutInfo> GetSubscribedWorkoutAsync()
    {
      GuidedWorkoutInfo subscribedWorkout = (GuidedWorkoutInfo) null;
      foreach (FavoriteWorkout favoriteWorkout in (IEnumerable<FavoriteWorkout>) await this.HealthCloudClient.GetFavoriteWorkoutsAsync(CancellationToken.None))
      {
        if (favoriteWorkout.IsSubscribed)
        {
          subscribedWorkout = new GuidedWorkoutInfo()
          {
            WorkoutPlanId = favoriteWorkout.WorkoutPlanId,
            WorkoutPlanInstanceId = favoriteWorkout.CurrentInstanceId,
            IsSubscribed = true,
            WorkoutPlanName = favoriteWorkout.WorkoutPlanBrowseDetails != null ? favoriteWorkout.WorkoutPlanBrowseDetails.Name : string.Empty
          };
          break;
        }
      }
      return subscribedWorkout;
    }

    public async Task<GuidedWorkoutState> GetWorkoutStateAsync(
      CancellationToken cancellationToken)
    {
      GuidedWorkoutState guidedWorkoutState = new GuidedWorkoutState()
      {
        State = GuidedWorkoutSyncState.Unknown
      };
      WorkoutState workoutState = await this.HealthCloudClient.GetWorkoutStateAsync(cancellationToken);
      if (workoutState != null)
      {
        guidedWorkoutState.State = workoutState.State;
        if (workoutState.WorkoutInfo != null)
        {
          GuidedWorkoutInfo subscribedWorkoutAsync = await this.GetSubscribedWorkoutAsync();
          guidedWorkoutState.WorkoutInfo = new GuidedWorkoutInfo()
          {
            WorkoutPlanId = workoutState.WorkoutInfo.WorkoutPlanId,
            WorkoutIndex = workoutState.WorkoutInfo.WorkoutIndex,
            WeekId = workoutState.WorkoutInfo.WeekId,
            DayId = workoutState.WorkoutInfo.Day,
            WorkoutPlanInstanceId = workoutState.WorkoutInfo.WorkoutPlanInstanceId,
            IsSyncedToBand = guidedWorkoutState.State == GuidedWorkoutSyncState.OnBand,
            IsRestDay = guidedWorkoutState.State == GuidedWorkoutSyncState.RestDay,
            IsSubscribed = subscribedWorkoutAsync != null && workoutState.WorkoutInfo.WorkoutPlanId == subscribedWorkoutAsync.WorkoutPlanId
          };
        }
      }
      return guidedWorkoutState;
    }

    private WorkoutScheduleDay GetRestDay(int weekId, int dayId) => new WorkoutScheduleDay()
    {
      PlanValues = new WorkoutPlanDay()
      {
        DayId = dayId,
        Workouts = (IList<string>) new List<string>()
        {
          AppResources.WorkoutRestDayLabel
        }
      },
      State = WorkoutDayState.Default,
      WeekId = weekId,
      IsRestDay = true
    };

    public async Task<WorkoutEvent> GetWorkoutEventAsync(string eventId)
    {
      WorkoutEvent eventAsync;
      try
      {
        eventAsync = await this.HealthCloudClient.GetEventAsync<WorkoutEvent>(eventId, CancellationToken.None, true, true, true);
      }
      catch (Exception ex)
      {
        WorkoutsProviderBase.Logger.Error("Unable to get the guided workout with event id {0}", (object) eventId);
        throw;
      }
      return eventAsync;
    }

    public async Task PatchWorkoutEventAsync(string eventId, string name) => await this.healthCloudClient.PatchEventAsync(eventId, name, EventType.GuidedWorkout, CancellationToken.None);

    public async Task<WorkoutEvent> GetLastCompletedWorkoutAsync()
    {
      WorkoutEvent lastEvent = (WorkoutEvent) null;
      IList<WorkoutEvent> topEventsAsync = await this.HealthCloudClient.GetTopEventsAsync<WorkoutEvent>(1, EventType.GuidedWorkout, CancellationToken.None, true, true, true);
      if (topEventsAsync.Count > 0)
        lastEvent = topEventsAsync[0];
      return lastEvent;
    }

    public async Task DeleteWorkoutEventAsync(string eventId) => await this.HealthCloudClient.DeleteEventAsync(eventId, EventType.GuidedWorkout, CancellationToken.None);

    private WorkoutDayState GetWorkoutStateFromStatus(WorkoutStatusValue status)
    {
      WorkoutDayState workoutDayState = WorkoutDayState.Default;
      switch (status)
      {
        case WorkoutStatusValue.Skipped:
          workoutDayState = WorkoutDayState.Skipped;
          break;
        case WorkoutStatusValue.Completed:
          workoutDayState = WorkoutDayState.Completed;
          break;
        case WorkoutStatusValue.NotStarted:
          workoutDayState = WorkoutDayState.ReadyToSync;
          break;
      }
      return workoutDayState;
    }

    private async Task<IList<WorkoutWeekGrouping>> SetScheduleStatusAsync(
      IList<WorkoutWeekGrouping> schedule,
      string workoutPlanId,
      int workoutInstanceId,
      bool isSubscribed)
    {
      IList<WorkoutStatus> workoutsInPlanAsync = await this.GetWorkoutsInPlanAsync(workoutPlanId, workoutInstanceId.ToString());
      WorkoutStatus lastCompletedWorkout = (WorkoutStatus) null;
      foreach (WorkoutStatus workoutStatus in (IEnumerable<WorkoutStatus>) workoutsInPlanAsync)
      {
        if (isSubscribed)
        {
          schedule[workoutStatus.WeekId - 1][workoutStatus.Day - 1].State = this.GetWorkoutStateFromStatus(workoutStatus.Status);
          if (workoutStatus.Status == WorkoutStatusValue.Completed || workoutStatus.Status == WorkoutStatusValue.Skipped)
            lastCompletedWorkout = workoutStatus;
        }
        else
          schedule[workoutStatus.WeekId - 1][workoutStatus.Day - 1].State = WorkoutDayState.ReadyToSync;
        schedule[workoutStatus.WeekId - 1][workoutStatus.Day - 1].LastUpdatedTime = new DateTimeOffset?(workoutStatus.LastUpdateTime);
        schedule[workoutStatus.WeekId - 1][workoutStatus.Day - 1].WorkoutIndex = workoutStatus.WorkoutIndex;
      }
      if (lastCompletedWorkout != null)
      {
        schedule[lastCompletedWorkout.WeekId - 1][lastCompletedWorkout.Day - 1].IsLastCompleted = true;
        for (int index = 0; index < lastCompletedWorkout.WeekId; ++index)
        {
          foreach (WorkoutScheduleDay workoutScheduleDay in (List<WorkoutScheduleDay>) schedule[index])
          {
            if (workoutScheduleDay.IsRestDay)
              workoutScheduleDay.State = WorkoutDayState.Completed;
            if (workoutScheduleDay.IsLastCompleted)
              break;
          }
        }
        TimeSpan timeProgressedSinceLastCompleted = DateTimeOffset.Now - lastCompletedWorkout.LastUpdateTime;
        if (this.Config.GWTimeProgressedDays > 0)
        {
          timeProgressedSinceLastCompleted = TimeSpan.FromDays((double) this.Config.GWTimeProgressedDays);
          WorkoutsProviderBase.Logger.Debug("Guided Workouts Time Progressed Override by {0} day(s)", (object) this.Config.GWTimeProgressedDays);
        }
        if (timeProgressedSinceLastCompleted.TotalDays > 0.0)
          WorkoutsProviderBase.MarkDaysAsCompleted(schedule, lastCompletedWorkout, timeProgressedSinceLastCompleted);
      }
      return schedule;
    }

    private static void MarkDaysAsCompleted(
      IList<WorkoutWeekGrouping> schedule,
      WorkoutStatus lastCompletedWorkout,
      TimeSpan timeProgressedSinceLastCompleted)
    {
      int num = Convert.ToInt32(timeProgressedSinceLastCompleted.TotalDays);
      bool flag = false;
      for (int index = lastCompletedWorkout.WeekId - 1; index < schedule.Count && num > 0; ++index)
      {
        foreach (WorkoutScheduleDay workoutScheduleDay in (List<WorkoutScheduleDay>) schedule[index])
        {
          if (workoutScheduleDay.IsLastCompleted)
            flag = true;
          else if (flag)
          {
            if (workoutScheduleDay.IsRestDay)
            {
              workoutScheduleDay.State = WorkoutDayState.Completed;
              --num;
              if (num <= 0)
                break;
            }
            else
            {
              num = 0;
              break;
            }
          }
        }
      }
    }

    public async Task<IList<WorkoutWeekGrouping>> GetScheduleAsync(
      string workoutPlanId,
      int workoutPlanInstanceId = 0,
      bool isSubscribed = false)
    {
      if (workoutPlanId == null)
        throw new ArgumentNullException(nameof (workoutPlanId));
      IList<WorkoutWeekGrouping> grouping = (IList<WorkoutWeekGrouping>) new List<WorkoutWeekGrouping>();
      WorkoutPlanDetail workoutAsync = await this.GetWorkoutAsync(workoutPlanId);
      if (workoutAsync == null)
        return (IList<WorkoutWeekGrouping>) null;
      int currentWeekNumber = 1;
      string str1 = string.Empty;
      foreach (WorkoutPlanWeek week in (IEnumerable<WorkoutPlanWeek>) workoutAsync.Weeks)
      {
        int num1 = 1;
        int num2 = week.Id.IndexOf(' ');
        if (num2 >= 0)
        {
          str1 = week.Id.Substring(0, num2);
          string[] strArray = week.Id.Substring(num2).Trim().Split('-');
          if (strArray.Length == 2)
          {
            int num3 = int.Parse(strArray[0]);
            int num4 = int.Parse(strArray[1]);
            num1 += num4 - num3;
          }
        }
        for (int index = 0; index < num1; ++index)
        {
          WorkoutWeekGrouping workoutWeekGrouping1 = new WorkoutWeekGrouping((IEnumerable<WorkoutScheduleDay>) this.CreateWorkoutScheduleDays(week, currentWeekNumber));
          workoutWeekGrouping1.PlanName = workoutAsync.Name;
          WorkoutWeekGrouping workoutWeekGrouping2 = workoutWeekGrouping1;
          string str2;
          if (num1 <= 1)
            str2 = week.Id;
          else
            str2 = string.Format("{0} {1}", new object[2]
            {
              (object) str1,
              (object) currentWeekNumber
            });
          workoutWeekGrouping2.WeekName = str2;
          grouping.Add(workoutWeekGrouping1);
          ++currentWeekNumber;
        }
      }
      grouping = await this.SetScheduleStatusAsync(grouping, workoutPlanId, workoutPlanInstanceId, isSubscribed);
      WorkoutStatus syncedWorkoutAsync = await this.GetLastSyncedWorkoutAsync(CancellationToken.None);
      if (syncedWorkoutAsync != null && syncedWorkoutAsync.WorkoutPlanId == workoutPlanId && syncedWorkoutAsync.WeekId - 1 >= 0 && syncedWorkoutAsync.Day - 1 >= 0)
      {
        WorkoutDayState state = grouping[syncedWorkoutAsync.WeekId - 1][syncedWorkoutAsync.Day - 1].State;
        grouping[syncedWorkoutAsync.WeekId - 1][syncedWorkoutAsync.Day - 1].State = state == WorkoutDayState.Completed ? WorkoutDayState.SyncedToBandAndCompleted : WorkoutDayState.SyncedToBand;
      }
      if (grouping.Count == 1)
        grouping[0].IsCollapsible = false;
      return grouping;
    }

    private List<WorkoutScheduleDay> CreateWorkoutScheduleDays(
      WorkoutPlanWeek week,
      int currentWeekNumber)
    {
      List<WorkoutScheduleDay> workoutScheduleDayList = new List<WorkoutScheduleDay>();
      foreach (WorkoutPlanDay workoutPlanDay in (IEnumerable<WorkoutPlanDay>) week.Days.OrderBy<WorkoutPlanDay, int>((Func<WorkoutPlanDay, int>) (d => d.DayId)))
      {
        if (workoutPlanDay.DayId - 1 > workoutScheduleDayList.Count)
        {
          for (int index = workoutPlanDay.DayId - (workoutScheduleDayList.Count + 1); index > 0; --index)
            workoutScheduleDayList.Add(this.GetRestDay(currentWeekNumber, workoutPlanDay.DayId - index));
        }
        WorkoutScheduleDay workoutScheduleDay = new WorkoutScheduleDay()
        {
          WeekId = currentWeekNumber,
          PlanValues = workoutPlanDay,
          State = WorkoutDayState.ReadyToSync
        };
        workoutScheduleDayList.Add(workoutScheduleDay);
      }
      int num = 7 - workoutScheduleDayList.Count;
      for (int index = 0; index < num; ++index)
        workoutScheduleDayList.Add(this.GetRestDay(currentWeekNumber, workoutScheduleDayList.Count + 1));
      return workoutScheduleDayList;
    }

    public async Task<GuidedWorkoutTileState> GetGuidedWorkoutTileStateAsync()
    {
      if (this.ApplicationEnvironmentService.IsEmulated)
        return GuidedWorkoutTileState.Enabled;
      bool retry = true;
      while (retry)
      {
        try
        {
          using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          {
            using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
            {
              StartStrip withoutImagesAsync = await cargoConnection.GetStartStripWithoutImagesAsync(cancellationTokenSource.Token);
              if (withoutImagesAsync != null)
                return !new StartStrip((IEnumerable<AdminBandTile>) withoutImagesAsync).Contains(Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922")) ? GuidedWorkoutTileState.Disabled : GuidedWorkoutTileState.Enabled;
            }
          }
        }
        catch (Exception ex)
        {
          WorkoutsProviderBase.Logger.Error(ex, "Error verifying guided workout tile");
          retry = await this.CheckRetryAsync(AppResources.WorkoutUploadFailureAndRetry, AppResources.WorkoutUploadFailureAndRetryTitle);
          if (retry)
            WorkoutsProviderBase.Logger.Debug((object) "Retrying verifying of guided workout tile");
        }
      }
      return GuidedWorkoutTileState.Unknown;
    }

    public async Task<string> GetWorkoutNameAsync(string workoutPlanId, int weekId, int dayId)
    {
      if (weekId < 1)
        throw new ArgumentOutOfRangeException(nameof (weekId));
      if (dayId < 1)
        throw new ArgumentOutOfRangeException(nameof (dayId));
      string name = string.Empty;
      IList<WorkoutWeekGrouping> workoutWeekGroupingList = await this.GetScheduleAsync(workoutPlanId, 0, false).ConfigureAwait(false);
      if (workoutWeekGroupingList != null && workoutWeekGroupingList.Count >= weekId)
      {
        WorkoutWeekGrouping workoutWeekGrouping = workoutWeekGroupingList[weekId - 1];
        if (workoutWeekGrouping != null && workoutWeekGrouping.Count >= dayId)
        {
          WorkoutScheduleDay workoutScheduleDay = workoutWeekGrouping[dayId - 1];
          if (workoutScheduleDay != null && workoutScheduleDay.PlanValues != null && workoutScheduleDay.PlanValues.Workouts != null && workoutScheduleDay.PlanValues.Workouts.Count > 0)
            name = workoutScheduleDay.PlanValues.Workouts[0];
        }
      }
      return name;
    }

    public async Task SyncNextWorkoutAsync(CancellationToken cancellationToken)
    {
      GuidedWorkoutState workoutStateAsync = await this.GetWorkoutStateAsync(cancellationToken);
      if (workoutStateAsync.State != GuidedWorkoutSyncState.SyncRequired || workoutStateAsync.WorkoutInfo == null || cancellationToken.IsCancellationRequested)
        return;
      WorkoutsProviderBase.Logger.Debug((object) ("Attempting workout upload; " + WorkoutsProviderBase.FormatWorkoutPlan(workoutStateAsync.WorkoutInfo)));
      await this.UploadWorkoutBandFileAsync(workoutStateAsync.WorkoutInfo.WorkoutPlanId, workoutStateAsync.WorkoutInfo.WorkoutIndex, workoutStateAsync.WorkoutInfo.WeekId, workoutStateAsync.WorkoutInfo.DayId, workoutStateAsync.WorkoutInfo.WorkoutPlanInstanceId, cancellationToken, GuidedWorkoutSyncMode.SyncPlan);
    }

    private static string FormatWorkoutPlan(GuidedWorkoutInfo status)
    {
      if (status == null)
        return "<null>";
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "WorkoutPlanId:{0}, WorkoutIndex:{1}, WeekId:{2}, DayId:{3}, Instance:{4}", (object) status.WorkoutPlanId, (object) status.WorkoutIndex, (object) status.WeekId, (object) status.DayId, (object) status.WorkoutPlanInstanceId);
    }

    private void NotifySubscriptionChanged()
    {
      this.LastSubscriptionChanged = new DateTimeOffset?(DateTimeOffset.Now);
      EventHandler subscriptionChanged = this.SubscriptionChanged;
      if (subscriptionChanged == null)
        return;
      subscriptionChanged((object) this, EventArgs.Empty);
    }
  }
}
