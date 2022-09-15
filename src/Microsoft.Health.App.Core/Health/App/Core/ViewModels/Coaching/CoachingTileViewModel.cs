// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.CoachingTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class CoachingTileViewModel : MetricTileViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Coaching\\CoachingTileViewModel.cs");
    private readonly ICoachingProvider coachingProvider;
    private readonly ICoachingService coachingService;
    private readonly CoachingComingUpViewModel comingUpViewModel;
    private readonly IDateTimeService dateTimeService;
    private readonly ITimerService timerFactory;
    private readonly IHttpCacheService httpCacheService;
    private readonly IDispatchService dispatchService;
    private readonly IEnvironmentService environmentService;
    private readonly IEventWaitHandleService eventWaitHandlerService;
    private IList<Reminder> reminders;
    private ITimer timer;
    private CancellationTokenSource updateTokenSource;

    public CoachingTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      TileFirstTimeUseViewModel firstTimeUse,
      CoachingComingUpViewModel comingUpViewModel,
      CoachingMyProgressViewModel myProgressViewModel,
      ICoachingProvider coachingProvider,
      ICoachingService coachingService,
      IDateTimeService dateTimeService,
      IDispatchService dispatchService,
      IHttpCacheService httpCacheService,
      ITimerService timerFactory,
      IEnvironmentService environmentService,
      IEventWaitHandleService eventWaitHandlerService)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      this.coachingProvider = coachingProvider;
      this.coachingService = coachingService;
      this.dateTimeService = dateTimeService;
      this.timerFactory = timerFactory;
      this.comingUpViewModel = comingUpViewModel;
      this.httpCacheService = httpCacheService;
      this.dispatchService = dispatchService;
      this.environmentService = environmentService;
      this.eventWaitHandlerService = eventWaitHandlerService;
      this.TopText = AppResources.CoachingTileTopText;
      this.TileIcon = "\uE196";
      this.Pivots.Add(new PivotDefinition(AppResources.PivotComingUp, (object) comingUpViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotMyProgress, (object) myProgressViewModel));
    }

    private async void RegisterBackgroundListener()
    {
      using (this.updateTokenSource = new CancellationTokenSource())
        await this.StartListeningForUpdateFromBackgroundAsync(this.updateTokenSource.Token);
      this.updateTokenSource = (CancellationTokenSource) null;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.coachingService.TilePendingRefresh = false;
      DateTimeOffset now = this.dateTimeService.Now.WithZeroOffset();
      DateTimeOffset today = (DateTimeOffset) now.Date;
      DateTimeOffset tomorrow = today.AddDays(1.0);
      Timeline coachingTimelineAsync = await this.coachingProvider.GetCoachingTimelineAsync(today, tomorrow, CancellationToken.None);
      IList<Reminder> reminders = coachingTimelineAsync != null ? coachingTimelineAsync.Reminders : (IList<Reminder>) new List<Reminder>();
      this.reminders = reminders;
      this.PopulateTileInfo(CoachingTileViewModel.ChooseReminder(reminders, now, today), tomorrow);
      this.CanOpen = true;
      return true;
    }

    protected override void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      if (!this.coachingService.TilePendingRefresh)
        return;
      this.Refresh();
    }

    private async void Refresh() => await this.LoadAsync((IDictionary<string, string>) null);

    private static Reminder ChooseReminder(
      IList<Reminder> reminders,
      DateTimeOffset now,
      DateTimeOffset today)
    {
      Reminder reminder1 = (Reminder) null;
      Reminder reminder2 = (Reminder) null;
      foreach (Reminder reminder3 in (IEnumerable<Reminder>) reminders)
      {
        if (reminder3.TargetTime <= now)
          reminder1 = reminder3;
        if (reminder3.TargetTime > now)
        {
          reminder2 = reminder3;
          break;
        }
      }
      if (reminder1 != null && now - reminder1.TargetTime > TimeSpan.FromMinutes(14.0))
        reminder1 = (Reminder) null;
      if (reminder2 != null && reminder2.TargetTime > today.AddDays(2.0))
      {
        CoachingTileViewModel.Logger.Warn((object) ("Coaching reminder " + reminder2.HabitName + " is too far in the future, discarding."));
        reminder2 = (Reminder) null;
      }
      return reminder1 != null || reminder2 == null ? (reminder2 != null || reminder1 == null ? (reminder1 != null ? (!(now - reminder1.TargetTime < TimeSpan.FromMinutes(1.0)) ? (!(reminder2.TargetTime - now < TimeSpan.FromMinutes(15.0)) ? reminder1 : reminder2) : reminder1) : (Reminder) null) : reminder1) : reminder2;
    }

    private async Task StartListeningForUpdateFromBackgroundAsync(CancellationToken waitToken)
    {
      EventWaitHandle waitHandle = this.eventWaitHandlerService.GetEventWaitHandle(false, EventResetMode.AutoReset, "BackgroundUpdateEventWaitHandle");
      waitHandle.Reset();
      try
      {
        while (true)
        {
          CoachingTileViewModel.Logger.Debug((object) "[[PushNotification Received!]]");
          await waitHandle.WaitAsync(waitToken).ConfigureAwait(false);
          await this.httpCacheService.RemoveTagsAsync("WellnessPlan");
          await this.httpCacheService.RemoveTagsAsync("WellnessSchedule");
          this.Refresh();
          try
          {
            await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () => await this.comingUpViewModel.RefreshAsync()));
          }
          catch (Exception ex)
          {
          }
        }
      }
      catch (OperationCanceledException ex)
      {
      }
    }

    private void PopulateTileInfo(Reminder chosenReminder, DateTimeOffset tomorrow)
    {
      if (chosenReminder == null)
      {
        this.Header = (StyledSpan) null;
        this.Subheader = AppResources.CoachingTileNothingComingUp;
      }
      else
      {
        this.Header = Formatter.FormatTimeLower(chosenReminder.TargetTime);
        this.Subheader = string.Format(AppResources.CoachingTileSubheaderFormat, new object[2]
        {
          (object) (chosenReminder.TargetTime < tomorrow ? AppResources.Today : AppResources.Tomorrow),
          (object) chosenReminder.HabitName
        });
      }
    }

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.RefreshTileInfo();
      this.StartTimer();
      this.RegisterBackgroundListener();
    }

    protected override void OnNavigatedFrom()
    {
      this.StopTimer();
      base.OnNavigatedFrom();
      CancellationTokenSource updateTokenSource = this.updateTokenSource;
      if (updateTokenSource == null || !updateTokenSource.Token.CanBeCanceled)
        return;
      updateTokenSource.Cancel();
    }

    private void RefreshTileInfo()
    {
      if (this.reminders == null || this.reminders.Count <= 0)
        return;
      DateTimeOffset now = this.dateTimeService.Now.WithZeroOffset();
      DateTime date = now.Date;
      DateTime dateTime = date.AddDays(1.0);
      this.PopulateTileInfo(CoachingTileViewModel.ChooseReminder(this.reminders, now, (DateTimeOffset) date), (DateTimeOffset) dateTime);
    }

    private void StartTimer()
    {
      if (this.timer == null)
        this.timer = this.timerFactory.CreateTimer();
      this.StopTimer();
      DateTimeOffset now = this.dateTimeService.Now;
      this.timer.Interval = now.RoundUp(TimeSpan.FromMinutes(1.0)) - now;
      this.timer.Tick += new EventHandler<object>(this.Timer_Tick);
      this.timer.Start();
    }

    private void StopTimer()
    {
      if (this.timer == null || !this.timer.IsEnabled)
        return;
      this.timer.Stop();
      this.timer.Tick -= new EventHandler<object>(this.Timer_Tick);
    }

    private void Timer_Tick(object sender, object e)
    {
      this.timer.Interval = TimeSpan.FromMinutes(1.0);
      this.RefreshTileInfo();
    }
  }
}
