// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.CoachingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class CoachingService : ICoachingService, ICalendarTileUpdateListener
  {
    private const string IsPlanActiveKey = "CoachingService.IsPlanActive";
    private const string SleepNotificationFileName = "CachedSleepNotificationInfo.json";
    private const string LightExposureNoficationFileName = "CachedLightExposureNoficationInfo.json";
    private const string NotificationDefaultHeader = "NotificationHeader";
    private const string NotificationDefaultBody = "NotificationBody";
    private const int MaxCalendarEntries = 8;
    private const int SpecialRemindersSyncDays = 7;
    private const int CalendarEventsSyncDays = 14;
    private const int CalendarEventsNotificationTime = 0;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\CoachingService.cs");
    private readonly ICoachingProvider coachingProvider;
    private readonly IConfigProvider configProvider;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IDateTimeService dateTimeService;
    private readonly IFileObjectStorageService isoObjectStore;
    private readonly Lazy<SleepNotificationComparer> sleepNotificationComparer;
    private readonly Lazy<LightExposureNotificationComparer> lightExposureNotificationComparer;
    private readonly IHealthCloudClient healthCloudClient;

    public CoachingService(
      ICoachingProvider coachingProvider,
      IConfigProvider configProvider,
      IBandConnectionFactory cargoConnectionFactory,
      IBandHardwareService bandHardwareService,
      IDateTimeService dateTimeService,
      IFileObjectStorageService isoObjectStore,
      IHealthCloudClient healthCloudClient)
    {
      this.coachingProvider = coachingProvider;
      this.configProvider = configProvider;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.bandHardwareService = bandHardwareService;
      this.dateTimeService = dateTimeService;
      this.isoObjectStore = isoObjectStore;
      this.healthCloudClient = healthCloudClient;
      this.sleepNotificationComparer = new Lazy<SleepNotificationComparer>();
      this.lightExposureNotificationComparer = new Lazy<LightExposureNotificationComparer>();
    }

    public bool? IsPlanActive
    {
      get
      {
        bool flag;
        return this.configProvider.TryGetValue<bool>("CoachingService.IsPlanActive", out flag) ? new bool?(flag) : new bool?();
      }
    }

    public bool ComingUpPendingRefresh { get; set; }

    public bool TilePendingRefresh { get; set; }

    public async Task<bool> RefreshIsPlanActiveAsync(CancellationToken token)
    {
      bool flag = await this.coachingProvider.IsOnPlanAsync(token).ConfigureAwait(false);
      this.configProvider.Set<bool>("CoachingService.IsPlanActive", flag);
      return flag;
    }

    public async Task ClearSpecialRemindersCacheAsync(CancellationToken token)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = this.SaveLastSentSleepNotificationAsync((SleepNotification) null, token).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.SaveLastSentLightExposureNotificationAsync((LightExposureNotification) null, token).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public async Task SyncSpecialRemindersAsync(CancellationToken token)
    {
      try
      {
        if (await this.bandHardwareService.GetDeviceTypeAsync(token).ConfigureAwait(false) != BandClass.Envoy)
          return;
        bool? isPlanActive = this.IsPlanActive;
        int num;
        if (isPlanActive.HasValue)
        {
          isPlanActive = this.IsPlanActive;
          num = isPlanActive.Value ? 1 : 0;
        }
        else
          num = 0;
        bool setReminders = num != 0;
        ConfiguredTaskAwaitable configuredTaskAwaitable;
        if (setReminders)
        {
          DateTimeOffset startDay = this.dateTimeService.GetToday().WithZeroOffset();
          Timeline timeline = await this.coachingProvider.GetCoachingTimelineAsync(startDay, startDay.AddDays(7.0), token).ConfigureAwait(false);
          if (timeline != null)
          {
            IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> reminders = timeline.Reminders;
            List<Microsoft.Health.App.Core.Providers.Coaching.Reminder> reminderList = new List<Microsoft.Health.App.Core.Providers.Coaching.Reminder>();
            List<Microsoft.Health.App.Core.Providers.Coaching.Reminder> lightExpoRemiders = new List<Microsoft.Health.App.Core.Providers.Coaching.Reminder>();
            SleepNotificationInfo notificationInfo = timeline.SleepNotificationInfo;
            LightExposureNotificationInfo lightExposureNotificationInfo = timeline.LightExposureNotificationInfo;
            if (reminders != null && reminders.Count > 0)
            {
              foreach (Microsoft.Health.App.Core.Providers.Coaching.Reminder reminder in (IEnumerable<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) reminders)
              {
                if (reminder.AssociatedCommand == AssociatedCommand.SetSleepNotification)
                  reminderList.Add(reminder);
                else if (reminder.AssociatedCommand == AssociatedCommand.SetLightExposureNotification)
                  lightExpoRemiders.Add(reminder);
              }
              if (reminderList.Count > 0 && notificationInfo != null && !string.IsNullOrEmpty(notificationInfo.Header) && !string.IsNullOrEmpty(notificationInfo.Body))
              {
                await this.SendSleepNotificationAsync((IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) reminderList, notificationInfo.Header, notificationInfo.Body, token).ConfigureAwait(false);
              }
              else
              {
                configuredTaskAwaitable = this.DisableSleepNotificationAsync(token).ConfigureAwait(false);
                await configuredTaskAwaitable;
              }
              if (lightExpoRemiders.Count > 0 && lightExposureNotificationInfo != null && !string.IsNullOrEmpty(lightExposureNotificationInfo.Header) && !string.IsNullOrEmpty(lightExposureNotificationInfo.Body))
              {
                configuredTaskAwaitable = this.SendLightExposureNotificationAsync((IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) lightExpoRemiders, lightExposureNotificationInfo.Header, lightExposureNotificationInfo.Body, token).ConfigureAwait(false);
                await configuredTaskAwaitable;
              }
              else
              {
                configuredTaskAwaitable = this.DisableLightExposureNotificationAsync(token).ConfigureAwait(false);
                await configuredTaskAwaitable;
              }
            }
            else
              setReminders = false;
            lightExpoRemiders = (List<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) null;
            lightExposureNotificationInfo = (LightExposureNotificationInfo) null;
          }
          else
            setReminders = false;
        }
        if (setReminders)
          return;
        configuredTaskAwaitable = this.DisableSleepNotificationAsync(token).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = this.DisableLightExposureNotificationAsync(token).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "Sending Special Reminders to Band.");
      }
    }

    private async Task SendSleepNotificationAsync(
      IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> sleepReminders,
      string header,
      string body,
      CancellationToken token)
    {
      try
      {
        SleepNotification sleepNotification = CoachingService.GetSleepNotification(sleepReminders, header, body);
        if (sleepNotification != null)
        {
          bool flag = this.sleepNotificationComparer.Value.Equals(await this.GetCachedSleepNotificationAsync(token), sleepNotification);
          if (!token.IsCancellationRequested && !flag)
          {
            using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
              await cargoConnection.SetSleepNotificationAsync(sleepNotification, token);
            await this.SaveLastSentSleepNotificationAsync(sleepNotification, token);
          }
        }
        sleepNotification = (SleepNotification) null;
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "Sending SleeNotification To Band.");
      }
    }

    private static SleepNotification GetSleepNotification(
      IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> sleepReminders,
      string header,
      string body)
    {
      SleepNotification sleepNotification = CoachingService.CreateDefaultSleepNotification(header, body);
      if (sleepReminders.Count <= 0)
        return sleepNotification;
      foreach (Microsoft.Health.App.Core.Providers.Coaching.Reminder sleepReminder in (IEnumerable<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) sleepReminders)
      {
        DateTimeOffset targetTime = sleepReminder.TargetTime;
        DayOfWeek dayOfWeek = targetTime.DayOfWeek;
        NotificationTime notificationTime = new NotificationTime((byte) targetTime.Hour, (byte) targetTime.Minute, sleepReminder.Enabled);
        switch (dayOfWeek)
        {
          case DayOfWeek.Sunday:
            sleepNotification.Sunday = notificationTime;
            continue;
          case DayOfWeek.Monday:
            sleepNotification.Monday = notificationTime;
            continue;
          case DayOfWeek.Tuesday:
            sleepNotification.Tuesday = notificationTime;
            continue;
          case DayOfWeek.Wednesday:
            sleepNotification.Wednesday = notificationTime;
            continue;
          case DayOfWeek.Thursday:
            sleepNotification.Thursday = notificationTime;
            continue;
          case DayOfWeek.Friday:
            sleepNotification.Friday = notificationTime;
            continue;
          case DayOfWeek.Saturday:
            sleepNotification.Saturday = notificationTime;
            continue;
          default:
            throw new ArgumentException();
        }
      }
      return sleepNotification;
    }

    private async Task DisableSleepNotificationAsync(CancellationToken token)
    {
      try
      {
        SleepNotification sleepNotification = CoachingService.CreateDefaultSleepNotification("NotificationHeader", "NotificationBody");
        bool flag = this.sleepNotificationComparer.Value.Equals(await this.GetCachedSleepNotificationAsync(token), sleepNotification);
        if (!token.IsCancellationRequested && !flag)
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
            await cargoConnection.DisableSleepNotificationAsync(token);
          await this.SaveLastSentSleepNotificationAsync(sleepNotification, token);
        }
        sleepNotification = (SleepNotification) null;
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "Disabling Sleep Notification To Band.");
      }
    }

    private async Task SendLightExposureNotificationAsync(
      IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> lightExposureReminders,
      string header,
      string body,
      CancellationToken token)
    {
      try
      {
        LightExposureNotification lightExposureNotification = CoachingService.GetLightExposureNotification(lightExposureReminders, header, body);
        if (lightExposureNotification != null)
        {
          bool flag = this.lightExposureNotificationComparer.Value.Equals(await this.GetCachedLightExposureNotificationAsync(token), lightExposureNotification);
          if (!token.IsCancellationRequested && !flag)
          {
            using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
              await cargoConnection.SetLightExposureNotificationAsync(lightExposureNotification, token);
            await this.SaveLastSentLightExposureNotificationAsync(lightExposureNotification, token);
          }
        }
        lightExposureNotification = (LightExposureNotification) null;
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "Sending Light Exposure Notification To Band.");
      }
    }

    private static LightExposureNotification GetLightExposureNotification(
      IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> lightExposureReminders,
      string header,
      string body)
    {
      LightExposureNotification exposureNotification = CoachingService.CreateDefaultLightExposureNotification(header, body);
      if (lightExposureReminders.Count <= 0)
        return exposureNotification;
      foreach (Microsoft.Health.App.Core.Providers.Coaching.Reminder exposureReminder in (IEnumerable<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) lightExposureReminders)
      {
        DateTimeOffset targetTime = exposureReminder.TargetTime;
        DayOfWeek dayOfWeek = targetTime.DayOfWeek;
        NotificationTime notificationTime = new NotificationTime((byte) targetTime.Hour, (byte) targetTime.Minute, exposureReminder.Enabled);
        switch (dayOfWeek)
        {
          case DayOfWeek.Sunday:
            exposureNotification.Sunday = notificationTime;
            continue;
          case DayOfWeek.Monday:
            exposureNotification.Monday = notificationTime;
            continue;
          case DayOfWeek.Tuesday:
            exposureNotification.Tuesday = notificationTime;
            continue;
          case DayOfWeek.Wednesday:
            exposureNotification.Wednesday = notificationTime;
            continue;
          case DayOfWeek.Thursday:
            exposureNotification.Thursday = notificationTime;
            continue;
          case DayOfWeek.Friday:
            exposureNotification.Friday = notificationTime;
            continue;
          case DayOfWeek.Saturday:
            exposureNotification.Saturday = notificationTime;
            continue;
          default:
            throw new ArgumentException();
        }
      }
      return exposureNotification;
    }

    private async Task DisableLightExposureNotificationAsync(CancellationToken token)
    {
      try
      {
        LightExposureNotification lightExposureNotification = CoachingService.CreateDefaultLightExposureNotification("NotificationHeader", "NotificationBody");
        bool flag = this.lightExposureNotificationComparer.Value.Equals(await this.GetCachedLightExposureNotificationAsync(token), lightExposureNotification);
        if (!token.IsCancellationRequested && !flag)
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
            await cargoConnection.DisableLightExposureNotificationAsync(token);
          await this.SaveLastSentLightExposureNotificationAsync(lightExposureNotification, token);
        }
        lightExposureNotification = (LightExposureNotification) null;
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "Disabling Light Exposure Notification To Band.");
      }
    }

    private static SleepNotification CreateDefaultSleepNotification(
      string header,
      string body)
    {
      return new SleepNotification(header, body)
      {
        Monday = new NotificationTime((byte) 0, (byte) 0),
        Tuesday = new NotificationTime((byte) 0, (byte) 0),
        Wednesday = new NotificationTime((byte) 0, (byte) 0),
        Thursday = new NotificationTime((byte) 0, (byte) 0),
        Friday = new NotificationTime((byte) 0, (byte) 0),
        Saturday = new NotificationTime((byte) 0, (byte) 0),
        Sunday = new NotificationTime((byte) 0, (byte) 0)
      };
    }

    private static LightExposureNotification CreateDefaultLightExposureNotification(
      string header,
      string body)
    {
      return new LightExposureNotification(header, body)
      {
        Monday = new NotificationTime((byte) 0, (byte) 0),
        Tuesday = new NotificationTime((byte) 0, (byte) 0),
        Wednesday = new NotificationTime((byte) 0, (byte) 0),
        Thursday = new NotificationTime((byte) 0, (byte) 0),
        Friday = new NotificationTime((byte) 0, (byte) 0),
        Saturday = new NotificationTime((byte) 0, (byte) 0),
        Sunday = new NotificationTime((byte) 0, (byte) 0)
      };
    }

    private static CalendarEvent ConvertToCalendarEvent(Microsoft.Health.App.Core.Providers.Coaching.Reminder reminder)
    {
      TimeSpan timeSpan = DateTime.UtcNow - DateTime.Now;
      DateTime startTime = new DateTime(reminder.TargetTime.Ticks, DateTimeKind.Local);
      return new CalendarEvent(reminder.PlanName ?? string.Empty, reminder.HabitName, startTime, startTime.AddMinutes(15.0))
      {
        AllDay = false,
        AcceptedState = CalendarEventAcceptedState.Accepted,
        EventCategory = 2,
        NotificationTime = new ushort?((ushort) 0)
      };
    }

    public async Task<IList<CalendarEvent>> GetCalendarEventsAsync(
      CancellationToken token)
    {
      List<CalendarEvent> calendarList = new List<CalendarEvent>();
      try
      {
        CoachingService.Logger.Debug((object) "<START> get calendar events from coaching service.");
        if (await this.bandHardwareService.GetDeviceTypeAsync(CancellationToken.None) == BandClass.Cargo)
          return (IList<CalendarEvent>) calendarList;
        bool? isPlanActive = this.IsPlanActive;
        int num;
        if (isPlanActive.HasValue)
        {
          isPlanActive = this.IsPlanActive;
          num = isPlanActive.Value ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          DateTimeOffset startDay = this.dateTimeService.GetToday().WithZeroOffset();
          DateTimeOffset dayAfterTomorrow = startDay.AddDays(2.0);
          DateTimeOffset now = this.dateTimeService.Now.WithZeroOffset();
          Timeline timeline = await this.coachingProvider.GetCoachingTimelineAsync(startDay, startDay.AddDays(14.0), token).ConfigureAwait(false);
          if (timeline != null)
          {
            IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> reminders = timeline.Reminders;
            List<Microsoft.Health.App.Core.Providers.Coaching.Reminder> source = new List<Microsoft.Health.App.Core.Providers.Coaching.Reminder>();
            if (reminders != null)
            {
              foreach (Microsoft.Health.App.Core.Providers.Coaching.Reminder reminder in (IEnumerable<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) reminders)
              {
                if (reminder.Enabled)
                {
                  switch (reminder.AssociatedCommand)
                  {
                    case AssociatedCommand.SetSleepNotification:
                    case AssociatedCommand.SetLightExposureNotification:
                      continue;
                    default:
                      source.Add(reminder);
                      continue;
                  }
                }
              }
            }
            calendarList = source.Where<Microsoft.Health.App.Core.Providers.Coaching.Reminder>((Func<Microsoft.Health.App.Core.Providers.Coaching.Reminder, bool>) (p => p.TargetTime.AddMinutes(15.0) > now && p.TargetTime < dayAfterTomorrow)).OrderBy<Microsoft.Health.App.Core.Providers.Coaching.Reminder, DateTimeOffset>((Func<Microsoft.Health.App.Core.Providers.Coaching.Reminder, DateTimeOffset>) (p => p.TargetTime)).Take<Microsoft.Health.App.Core.Providers.Coaching.Reminder>(8).Select<Microsoft.Health.App.Core.Providers.Coaching.Reminder, CalendarEvent>(new Func<Microsoft.Health.App.Core.Providers.Coaching.Reminder, CalendarEvent>(CoachingService.ConvertToCalendarEvent)).ToList<CalendarEvent>();
          }
        }
        CoachingService.Logger.Debug((object) "<END> get calendar events from coaching service.");
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "<FAILED> get calendar events from coaching service.");
      }
      return (IList<CalendarEvent>) calendarList;
    }

    private async Task SaveLastSentSleepNotificationAsync(
      SleepNotification sleepNotification,
      CancellationToken token)
    {
      try
      {
        CoachingService.Logger.Debug((object) "<START> save sleep notification info to isolated storage.");
        await this.isoObjectStore.WriteObjectAsync((object) sleepNotification, "CachedSleepNotificationInfo.json", token).ConfigureAwait(false);
        CoachingService.Logger.Debug((object) "<END> save sleep notification info to isolated storage.");
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "<FAILED> save sleep notification info to isolated storage.");
      }
    }

    private async Task<SleepNotification> GetCachedSleepNotificationAsync(
      CancellationToken token)
    {
      SleepNotification notification = (SleepNotification) null;
      try
      {
        CoachingService.Logger.Debug((object) "<START> Get cached sleep notification info.");
        notification = await this.isoObjectStore.ReadObjectAsync<SleepNotification>("CachedSleepNotificationInfo.json", token).ConfigureAwait(false);
        CoachingService.Logger.Debug((object) "<END> Get cached sleep notification info.");
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "<FAILED> Get cached sleep notification info.");
      }
      return notification;
    }

    private async Task SaveLastSentLightExposureNotificationAsync(
      LightExposureNotification lightExposureNotification,
      CancellationToken token)
    {
      try
      {
        CoachingService.Logger.Debug((object) "<START> save light Exposure notification info to isolated storage.");
        await this.isoObjectStore.WriteObjectAsync((object) lightExposureNotification, "CachedLightExposureNoficationInfo.json", token).ConfigureAwait(false);
        CoachingService.Logger.Debug((object) "<END> save light Exposure notification info to isolated storage.");
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "<FAILED> save light Exposure notification info to isolated storage.");
      }
    }

    private async Task<LightExposureNotification> GetCachedLightExposureNotificationAsync(
      CancellationToken token)
    {
      LightExposureNotification notification = (LightExposureNotification) null;
      try
      {
        CoachingService.Logger.Debug((object) "<START> Get cached light Exposure notification info.");
        notification = await this.isoObjectStore.ReadObjectAsync<LightExposureNotification>("CachedLightExposureNoficationInfo.json", token).ConfigureAwait(false);
        CoachingService.Logger.Debug((object) "<END> Get cached light Exposure notification info.");
      }
      catch (Exception ex)
      {
        CoachingService.Logger.Error(ex, "<FAILED> Get cached light Exposure notification info.");
      }
      return notification;
    }

    public async Task SyncGoalsToBandAsync(CancellationToken cancellationToken)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        await cargoConnection.CheckConnectionWorkingAsync(cancellationToken);
        IList<UsersGoal> usersGoalsAsync = await this.healthCloudClient.GetUsersGoalsAsync(GoalType.Unknown, cancellationToken);
        Goals goals = new Goals();
        foreach (UsersGoal usersGoal in (IEnumerable<UsersGoal>) usersGoalsAsync)
        {
          switch (usersGoal.Type)
          {
            case GoalType.StepGoal:
              goals.StepsEnabled = true;
              goals.StepsGoal = (uint) usersGoal.Value;
              continue;
            case GoalType.CalorieGoal:
              goals.CaloriesEnabled = true;
              goals.CaloriesGoal = (uint) usersGoal.Value;
              continue;
            default:
              continue;
          }
        }
        if (goals.StepsGoal == 0U)
        {
          goals.StepsEnabled = true;
          goals.StepsGoal = 5000U;
        }
        if (goals.CaloriesGoal == 0U)
        {
          goals.CaloriesEnabled = true;
          goals.CaloriesGoal = 5000U;
        }
        CoachingService.Logger.Debug((object) ("<STEP GOALS SYNC> Current: " + (object) goals.StepsGoal));
        await cargoConnection.SaveGoalsToBandAsync(goals);
      }
    }
  }
}
