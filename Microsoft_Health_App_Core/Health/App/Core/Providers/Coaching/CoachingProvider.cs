// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.CoachingProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class CoachingProvider : ICoachingProvider
  {
    internal const string SleepNotificationCommand = "SET_SLEEP_NOTIFICATION";
    internal const string LightExposureCommand = "SET_LIGHT_EXPO_NOTIFICATION";
    internal const string Heading1Key = "Heading1";
    internal const string Heading2Key = "Heading2";
    private readonly IWellnessPlanClient wellnessPlanClient;
    private readonly IDateTimeService dateTimeService;
    private readonly IInsightsProvider insightsProvider;

    public CoachingProvider(
      IWellnessPlanClient wellnessPlanClient,
      IDateTimeService dateTimeService,
      IInsightsProvider insightsProvider)
    {
      this.wellnessPlanClient = wellnessPlanClient;
      this.dateTimeService = dateTimeService;
      this.insightsProvider = insightsProvider;
    }

    public async Task<IList<CoachingProgressSection>> GetProgressSectionsAsync(
      CancellationToken token)
    {
      DateTimeOffset today = this.dateTimeService.GetToday();
      DateTimeOffset tomorrow = this.dateTimeService.GetTomorrow();
      int dayOfWeek = (int) today.DayOfWeek;
      DateTimeOffset sunday = today.AddDays((double) -dayOfWeek);
      UserWellnessPlansResponse plansAsync = await this.wellnessPlanClient.GetPlansAsync(token, status: new WellnessPlanStatus?(WellnessPlanStatus.Active));
      UserWellnessPlan userWellnessPlan1 = plansAsync.UserWellnessPlans.FirstOrDefault<UserWellnessPlan>((Func<UserWellnessPlan, bool>) (p => p.PlanType == WellnessPlanType.Activity));
      UserWellnessPlan userWellnessPlan2 = plansAsync.UserWellnessPlans.FirstOrDefault<UserWellnessPlan>((Func<UserWellnessPlan, bool>) (p => p.PlanType == WellnessPlanType.Sleep));
      List<Task> taskList = new List<Task>();
      Task<UserWellnessPlanProgress> activityTask = (Task<UserWellnessPlanProgress>) null;
      Task<UserWellnessPlanProgress> sleepTask = (Task<UserWellnessPlanProgress>) null;
      if (userWellnessPlan1 != null)
      {
        activityTask = this.wellnessPlanClient.GetPlanProgressAsync(userWellnessPlan1.Id, sunday, tomorrow, token);
        taskList.Add((Task) activityTask);
      }
      if (userWellnessPlan2 != null)
      {
        sleepTask = this.wellnessPlanClient.GetPlanProgressAsync(userWellnessPlan2.Id, sunday, tomorrow, token);
        taskList.Add((Task) sleepTask);
      }
      await Task.WhenAll((IEnumerable<Task>) taskList);
      bool flag = false;
      List<CoachingProgressSection> coachingProgressSectionList = new List<CoachingProgressSection>();
      if (activityTask != null && activityTask.Result != null)
      {
        IList<WeeklyActivityProgressSummary> activityProgressSummary = activityTask.Result.WeeklyActivityProgressSummary;
        if (activityProgressSummary != null && activityProgressSummary.Any<WeeklyActivityProgressSummary>())
        {
          coachingProgressSectionList.Add(CoachingProgressSection.FromCloudActivityProgress(activityProgressSummary.First<WeeklyActivityProgressSummary>()));
          flag = true;
        }
      }
      if (sleepTask != null)
      {
        flag = false;
        if (sleepTask.Result != null)
        {
          IList<WeeklySleepProgressSummary> sleepProgressSummary = sleepTask.Result.WeeklySleepProgressSummary;
          if (sleepProgressSummary != null && sleepProgressSummary.Any<WeeklySleepProgressSummary>())
          {
            coachingProgressSectionList.Add(CoachingProgressSection.FromCloudSleepProgress(sleepProgressSummary.First<WeeklySleepProgressSummary>()));
            flag = true;
          }
        }
      }
      if (!flag)
        throw new InvalidOperationException("Unable to fetch Wellness Plan progress.");
      return (IList<CoachingProgressSection>) coachingProgressSectionList;
    }

    public async Task<bool> IsOnPlanAsync(CancellationToken token)
    {
      DateTimeOffset today = this.dateTimeService.GetToday();
      return (await this.GetSortedScheduleAsync(today, today.AddDays(1.0), token)).HasActivePlans;
    }

    public async Task<Timeline> GetCoachingTimelineAsync(
      DateTimeOffset startDay,
      DateTimeOffset endDay,
      CancellationToken token)
    {
      startDay = startDay.WithCurrentOffset();
      endDay = endDay.WithCurrentOffset();
      WellnessSchedule sortedScheduleAsync = await this.GetSortedScheduleAsync(startDay, endDay.AddDays(1.0), token);
      if (sortedScheduleAsync == null)
        return (Timeline) null;
      List<Reminder> list = sortedScheduleAsync.Reminders.Select<Microsoft.Health.Cloud.Client.Reminder, Reminder>(new Func<Microsoft.Health.Cloud.Client.Reminder, Reminder>(Reminder.FromCloudModel)).ToList<Reminder>();
      IDictionary<string, string> sleepCommandSettings;
      SleepNotificationInfo notificationInfo1 = sortedScheduleAsync.BandSettings == null || !sortedScheduleAsync.BandSettings.CommandSettings.TryGetValue("SET_SLEEP_NOTIFICATION", out sleepCommandSettings) ? (SleepNotificationInfo) null : new SleepNotificationInfo(sleepCommandSettings.ContainsKey("Heading1") ? sleepCommandSettings["Heading1"] : (string) null, sleepCommandSettings.ContainsKey("Heading2") ? sleepCommandSettings["Heading2"] : (string) null);
      IDictionary<string, string> lightExposureCommandSettings;
      LightExposureNotificationInfo notificationInfo2 = sortedScheduleAsync.BandSettings == null || !sortedScheduleAsync.BandSettings.CommandSettings.TryGetValue("SET_LIGHT_EXPO_NOTIFICATION", out lightExposureCommandSettings) ? (LightExposureNotificationInfo) null : new LightExposureNotificationInfo(lightExposureCommandSettings.ContainsKey("Heading1") ? lightExposureCommandSettings["Heading1"] : (string) null, lightExposureCommandSettings.ContainsKey("Heading2") ? lightExposureCommandSettings["Heading2"] : (string) null);
      SleepNotificationInfo sleepNotificationInfo = notificationInfo1;
      LightExposureNotificationInfo lightExposureNotificationInfo = notificationInfo2;
      return new Timeline((IList<Reminder>) list, sleepNotificationInfo, lightExposureNotificationInfo);
    }

    public async Task<Reminder> GetReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token)
    {
      return Reminder.FromCloudModel(CoachingProvider.PickReminder(await this.GetSortedScheduleAsync(timelineDayRange.Low.WithCurrentOffset(), timelineDayRange.High.WithCurrentOffset(), token), habitId, habitIdDuplicateIndex, day));
    }

    public async Task SaveReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      DateTimeOffset newStartTime,
      bool isReminderEnabled,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token)
    {
      DateTimeOffset dayStart = timelineDayRange.Low.WithCurrentOffset();
      DateTimeOffset dayEnd = timelineDayRange.High.WithCurrentOffset();
      WellnessSchedule sortedScheduleAsync = await this.GetSortedScheduleAsync(dayStart, dayEnd, token, true);
      Microsoft.Health.Cloud.Client.Reminder reminder = CoachingProvider.PickReminder(sortedScheduleAsync, habitId, habitIdDuplicateIndex, day);
      reminder.TargetUtc = newStartTime.WithOffset(TimeSpan.FromMinutes((double) reminder.TargetTZOffsetMin)).ToUniversalTime();
      reminder.Enabled = isReminderEnabled;
      await this.wellnessPlanClient.PutScheduleAsync(sortedScheduleAsync, dayStart, dayEnd, token).ConfigureAwait(false);
    }

    public async Task SkipReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token)
    {
      DateTimeOffset dayStart = timelineDayRange.Low.WithCurrentOffset();
      DateTimeOffset dayEnd = timelineDayRange.High.WithCurrentOffset();
      WellnessSchedule sortedScheduleAsync = await this.GetSortedScheduleAsync(dayStart, dayEnd, token, true);
      Microsoft.Health.Cloud.Client.Reminder reminder = CoachingProvider.PickReminder(sortedScheduleAsync, habitId, habitIdDuplicateIndex, day);
      sortedScheduleAsync.Reminders.Remove(reminder);
      await this.wellnessPlanClient.PutScheduleAsync(sortedScheduleAsync, dayStart, dayEnd, token).ConfigureAwait(false);
    }

    public async Task<SleepDurationRecommendation> GetSleepDurationRecommendationAsync(
      CancellationToken token)
    {
      IWellnessPlanClient wellnessPlanClient = this.wellnessPlanClient;
      CancellationToken cancellationToken = token;
      WellnessPlanType? planType = new WellnessPlanType?(WellnessPlanType.Sleep);
      List<string> stringList = new List<string>();
      stringList.Add("Goals");
      WellnessPlanStatus? status = new WellnessPlanStatus?(WellnessPlanStatus.Active);
      UserWellnessPlansResponse wellnessPlansResponse = await wellnessPlanClient.GetPlansAsync(cancellationToken, planType, (IList<string>) stringList, status).ConfigureAwait(false);
      if (wellnessPlansResponse == null || wellnessPlansResponse.UserWellnessPlans.Count == 0)
        return (SleepDurationRecommendation) null;
      UserWellnessPlan userWellnessPlan = wellnessPlansResponse.UserWellnessPlans.FirstOrDefault<UserWellnessPlan>((Func<UserWellnessPlan, bool>) (p => p.PlanType == WellnessPlanType.Sleep));
      if (userWellnessPlan == null)
        return (SleepDurationRecommendation) null;
      UsersGoal usersGoal1 = userWellnessPlan.Goals.FirstOrDefault<UsersGoal>((Func<UsersGoal, bool>) (g => g.Type == GoalType.SleepMinutesGoal));
      if (usersGoal1 == null)
        return (SleepDurationRecommendation) null;
      GoalValueSummary goalValueSummary1 = usersGoal1.ValueSummary.Last<GoalValueSummary>();
      TimeSpan currentSleepDuration = TimeSpan.FromMinutes((double) int.Parse(goalValueSummary1.ValueTemplate.Threshold.ToString()));
      object recommended = goalValueSummary1.ValueTemplate.Recommended;
      if (recommended == null)
        return (SleepDurationRecommendation) null;
      TimeSpan recommendedSleepDuration = TimeSpan.FromMinutes((double) int.Parse(recommended.ToString()));
      UsersGoal usersGoal2 = userWellnessPlan.Goals.FirstOrDefault<UsersGoal>((Func<UsersGoal, bool>) (g => g.Type == GoalType.WakeUpTimeGoal));
      if (usersGoal2 == null)
        return (SleepDurationRecommendation) null;
      GoalValueSummary goalValueSummary2 = usersGoal2.ValueSummary.Last<GoalValueSummary>();
      int num = int.Parse(goalValueSummary2.ValueTemplate.Threshold.ToString());
      DateTimeOffset currentWakeUpTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMinutes((double) num);
      return new SleepDurationRecommendation(currentSleepDuration, recommendedSleepDuration, currentWakeUpTime, userWellnessPlan.Id, usersGoal1.Id, usersGoal2.Id, goalValueSummary1.ValueTemplate.Name, goalValueSummary2.ValueTemplate.Name);
    }

    public async Task UpdateSleepPlanDurationGoalAsync(
      SleepDurationRecommendation recommendation,
      SleepPlanRecommendationOption chosenOption,
      string insightId,
      CancellationToken token)
    {
      await this.insightsProvider.AcknowledgeInsightAsync(insightId, token).ConfigureAwait(false);
      Task sleepDurationGoalTask;
      switch (chosenOption)
      {
        case SleepPlanRecommendationOption.ChangeBedtime:
          IWellnessPlanClient wellnessPlanClient1 = this.wellnessPlanClient;
          string planId1 = recommendation.PlanId;
          string sleepDurationGoalId1 = recommendation.SleepDurationGoalId;
          WellnessGoalUpdate update1 = new WellnessGoalUpdate();
          update1.GoalValueTemplateName = recommendation.SleepDurationGoalTemplateName;
          update1.InterimValue = recommendation.RecommendedSleepDuration.TotalMinutes.ToString();
          update1.UserGoalUpdateAction = WellnessGoalUpdateAction.AcceptRecommendation;
          CancellationToken token1 = token;
          await wellnessPlanClient1.UpdatePlanGoalAsync(planId1, sleepDurationGoalId1, update1, token1).ConfigureAwait(false);
          break;
        case SleepPlanRecommendationOption.ChangeWakeUpTime:
          IWellnessPlanClient wellnessPlanClient2 = this.wellnessPlanClient;
          string planId2 = recommendation.PlanId;
          string sleepDurationGoalId2 = recommendation.SleepDurationGoalId;
          WellnessGoalUpdate update2 = new WellnessGoalUpdate();
          update2.GoalValueTemplateName = recommendation.SleepDurationGoalTemplateName;
          TimeSpan timeSpan = recommendation.RecommendedSleepDuration;
          update2.InterimValue = timeSpan.TotalMinutes.ToString();
          update2.UserGoalUpdateAction = WellnessGoalUpdateAction.AcceptRecommendation;
          CancellationToken token2 = token;
          sleepDurationGoalTask = wellnessPlanClient2.UpdatePlanGoalAsync(planId2, sleepDurationGoalId2, update2, token2);
          DateTimeOffset dateTime = recommendation.CurrentWakeUpTime + (recommendation.RecommendedSleepDuration - recommendation.CurrentSleepDuration);
          timeSpan = dateTime - dateTime.RoundDown(TimeSpan.FromDays(1.0));
          int totalMinutes = (int) timeSpan.TotalMinutes;
          IWellnessPlanClient wellnessPlanClient3 = this.wellnessPlanClient;
          string planId3 = recommendation.PlanId;
          string wakeUpTimeGoalId = recommendation.WakeUpTimeGoalId;
          WellnessGoalUpdate update3 = new WellnessGoalUpdate();
          update3.GoalValueTemplateName = recommendation.WakeUpTimeGoalTemplateName;
          update3.InterimValue = totalMinutes.ToString();
          update3.UserGoalUpdateAction = WellnessGoalUpdateAction.ManualUpdate;
          CancellationToken token3 = token;
          await wellnessPlanClient3.UpdatePlanGoalAsync(planId3, wakeUpTimeGoalId, update3, token3).ConfigureAwait(false);
          await sleepDurationGoalTask.ConfigureAwait(false);
          break;
        case SleepPlanRecommendationOption.Decline:
          IWellnessPlanClient wellnessPlanClient4 = this.wellnessPlanClient;
          string planId4 = recommendation.PlanId;
          string sleepDurationGoalId3 = recommendation.SleepDurationGoalId;
          WellnessGoalUpdate update4 = new WellnessGoalUpdate();
          update4.GoalValueTemplateName = recommendation.SleepDurationGoalTemplateName;
          update4.InterimValue = recommendation.CurrentSleepDuration.TotalMinutes.ToString();
          update4.UserGoalUpdateAction = WellnessGoalUpdateAction.DeclineRecommendation;
          CancellationToken token4 = token;
          await wellnessPlanClient4.UpdatePlanGoalAsync(planId4, sleepDurationGoalId3, update4, token4).ConfigureAwait(false);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (chosenOption));
      }
      sleepDurationGoalTask = (Task) null;
    }

    private static Microsoft.Health.Cloud.Client.Reminder PickReminder(
      WellnessSchedule schedule,
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day)
    {
      if (schedule == null)
        throw new InvalidOperationException("Cannot complete operation: plan is no longer active.");
      int num = 0;
      Microsoft.Health.Cloud.Client.Reminder reminder1 = (Microsoft.Health.Cloud.Client.Reminder) null;
      foreach (Microsoft.Health.Cloud.Client.Reminder reminder2 in (IEnumerable<Microsoft.Health.Cloud.Client.Reminder>) schedule.Reminders)
      {
        if (reminder2.HabitId == habitId && reminder2.TargetUtc.AddMinutes((double) reminder2.TargetTZOffsetMin).Date == day.Date)
        {
          if (num == habitIdDuplicateIndex)
          {
            reminder1 = reminder2;
            break;
          }
          ++num;
        }
      }
      return reminder1 != null ? reminder1 : throw new ArgumentException(string.Format("Could not find reminder with habitId '{0}' and duplicate index {1} on date {2}.", new object[3]
      {
        (object) habitId,
        (object) habitIdDuplicateIndex,
        (object) day.Date
      }));
    }

    private async Task<WellnessSchedule> GetSortedScheduleAsync(
      DateTimeOffset start,
      DateTimeOffset end,
      CancellationToken token,
      bool bypassCache = false)
    {
      WellnessSchedule wellnessSchedule = await this.wellnessPlanClient.GetScheduleAsync(start, end, token, bypassCache).ConfigureAwait(false);
      if (wellnessSchedule != null)
      {
        List<Microsoft.Health.Cloud.Client.Reminder> list = wellnessSchedule.Reminders.OrderBy<Microsoft.Health.Cloud.Client.Reminder, DateTimeOffset>((Func<Microsoft.Health.Cloud.Client.Reminder, DateTimeOffset>) (r => r.TargetUtc.AddMinutes((double) r.TargetTZOffsetMin))).ToList<Microsoft.Health.Cloud.Client.Reminder>();
        wellnessSchedule.Reminders.Clear();
        foreach (Microsoft.Health.Cloud.Client.Reminder reminder in list)
          wellnessSchedule.Reminders.Add(reminder);
      }
      return wellnessSchedule;
    }
  }
}
