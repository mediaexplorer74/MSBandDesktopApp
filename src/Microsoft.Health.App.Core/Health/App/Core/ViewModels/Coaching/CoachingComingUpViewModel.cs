// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.CoachingComingUpViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  [PageTaxonomy(new string[] {"Fitness", "Coaching Plan", "Coming Up"})]
  public class CoachingComingUpViewModel : PanelViewModelBase, INavigationListener
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Coaching\\CoachingComingUpViewModel.cs");
    private static readonly EmbeddedAsset[] InsightImages = new EmbeddedAsset[5]
    {
      EmbeddedAsset.CoachingInsightBlue,
      EmbeddedAsset.CoachingInsightHills,
      EmbeddedAsset.CoachingInsightOrange,
      EmbeddedAsset.CoachingInsightSun,
      EmbeddedAsset.CoachingInsightYellow
    };
    private readonly ICoachingProvider coachingProvider;
    private readonly IInsightsProvider insightsProvider;
    private readonly IUserProfileService userProfileService;
    private readonly ITileManagementService tileManagementService;
    private readonly IDateTimeService dateTimeService;
    private readonly ISmoothNavService smoothNavService;
    private readonly ICoachingService coachingService;
    private bool showPinCalendarBanner;
    private bool showInsight;
    private string insightText;
    private bool showDismissButton;
    private bool showOpenGoalButton;
    private bool showImage;
    private EmbeddedOrRemoteImageSource image;
    private Range<DateTimeOffset> dayRange;
    private DayRemindersViewModel remindersForToday;
    private DayRemindersViewModel remindersForTomorrow;
    private RaisedInsight shownInsight;
    private HealthCommand dismissCommand;
    private HealthCommand openGoalCommand;
    private HealthCommand pinCalendarCommand;

    public CoachingComingUpViewModel(
      INetworkService networkService,
      ICoachingProvider coachingProvider,
      IInsightsProvider insightsProvider,
      IUserProfileService userProfileService,
      ITileManagementService tileManagementService,
      IDateTimeService dateTimeService,
      ISmoothNavService smoothNavService,
      ICoachingService coachingService)
      : base(networkService)
    {
      this.coachingProvider = coachingProvider;
      this.insightsProvider = insightsProvider;
      this.userProfileService = userProfileService;
      this.tileManagementService = tileManagementService;
      this.dateTimeService = dateTimeService;
      this.smoothNavService = smoothNavService;
      this.coachingService = coachingService;
    }

    public bool ShowPinCalendarBanner
    {
      get => this.showPinCalendarBanner;
      set => this.SetProperty<bool>(ref this.showPinCalendarBanner, value, nameof (ShowPinCalendarBanner));
    }

    public bool ShowInsight
    {
      get => this.showInsight;
      set => this.SetProperty<bool>(ref this.showInsight, value, nameof (ShowInsight));
    }

    public string InsightText
    {
      get => this.insightText;
      set => this.SetProperty<string>(ref this.insightText, value, nameof (InsightText));
    }

    public bool ShowDismissButton
    {
      get => this.showDismissButton;
      set => this.SetProperty<bool>(ref this.showDismissButton, value, nameof (ShowDismissButton));
    }

    public bool ShowOpenGoalButton
    {
      get => this.showOpenGoalButton;
      set => this.SetProperty<bool>(ref this.showOpenGoalButton, value, nameof (ShowOpenGoalButton));
    }

    public bool ShowImage
    {
      get => this.showImage;
      set => this.SetProperty<bool>(ref this.showImage, value, nameof (ShowImage));
    }

    public EmbeddedOrRemoteImageSource ImageSource
    {
      get => this.image;
      set => this.SetProperty<EmbeddedOrRemoteImageSource>(ref this.image, value, nameof (ImageSource));
    }

    public DayRemindersViewModel RemindersForToday
    {
      get => this.remindersForToday;
      set => this.SetProperty<DayRemindersViewModel>(ref this.remindersForToday, value, nameof (RemindersForToday));
    }

    public DayRemindersViewModel RemindersForTomorrow
    {
      get => this.remindersForTomorrow;
      set => this.SetProperty<DayRemindersViewModel>(ref this.remindersForTomorrow, value, nameof (RemindersForTomorrow));
    }

    public ICommand DismissCommand => (ICommand) this.dismissCommand ?? (ICommand) (this.dismissCommand = new HealthCommand((Action) (async () =>
    {
      try
      {
        this.ShowInsight = false;
        await this.insightsProvider.AcknowledgeInsightAsync(this.shownInsight.RaisedInsightId, CancellationToken.None);
      }
      catch (Exception ex)
      {
        CoachingComingUpViewModel.Logger.Error((object) "Dismissing insight on coaching tile failed.", ex);
      }
    })));

    public ICommand OpenGoalCommand => (ICommand) this.openGoalCommand ?? (ICommand) (this.openGoalCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (SleepPlanRecommendationViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "InsightId",
        this.shownInsight.RaisedInsightId
      }
    }))));

    public ICommand PinCalendarCommand => (ICommand) this.pinCalendarCommand ?? (ICommand) (this.pinCalendarCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (ManageTilesViewModel)))));

    public void OpenReminder(ReminderViewModel reminderViewModel)
    {
      Assert.ParamIsNotNull((object) reminderViewModel, nameof (reminderViewModel));
      this.smoothNavService.Navigate(typeof (EditReminderViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "EditHabit.HabitId",
          reminderViewModel.HabitId
        },
        {
          "EditHabit.HabitIdDuplicateIndex",
          reminderViewModel.HabitIdDuplicateIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)
        },
        {
          "EditHabit.Day",
          reminderViewModel.Day.ToString("o")
        },
        {
          "EditHabit.RangeStart",
          this.dayRange.Low.ToString("o")
        },
        {
          "EditHabit.RangeEnd",
          this.dayRange.High.ToString("o")
        }
      });
    }

    public async void RefreshData() => await this.LoadDataAsync((IDictionary<string, string>) null);

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.coachingService.ComingUpPendingRefresh = false;
      DateTimeOffset startDay = this.dateTimeService.GetToday().WithZeroOffset();
      this.dayRange = new Range<DateTimeOffset>()
      {
        Low = startDay,
        High = startDay.AddDays(2.0)
      };
      DateTimeOffset tomorrow = startDay.AddDays(1.0);
      Task<Timeline> timelineTask = this.coachingProvider.GetCoachingTimelineAsync(startDay, tomorrow, CancellationToken.None);
      Task<IList<RaisedInsight>> insightsTask = this.insightsProvider.GetWellnessPlanInsightsAsync(CancellationToken.None);
      if (this.userProfileService.IsRegisteredBandPaired)
      {
        Guid calendarTileId = Guid.Parse("ec149021-ce45-40e9-aeee-08f86e4746a7");
        this.ShowPinCalendarBanner = !this.tileManagementService.EnabledTiles.Any<AppBandTile>((Func<AppBandTile, bool>) (t => t.TileId == calendarTileId));
      }
      else
        this.ShowPinCalendarBanner = false;
      await Task.WhenAll((Task) timelineTask, (Task) insightsTask);
      Timeline result1 = timelineTask.Result;
      IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder> source = result1 != null ? result1.Reminders : (IList<Microsoft.Health.App.Core.Providers.Coaching.Reminder>) new List<Microsoft.Health.App.Core.Providers.Coaching.Reminder>();
      IList<RaisedInsight> result2 = insightsTask.Result;
      this.RemindersForToday = new DayRemindersViewModel(this, AppResources.Today, AppResources.CoachingComingUpNoEventsToday);
      this.RemindersForTomorrow = new DayRemindersViewModel(this, AppResources.Tomorrow, AppResources.CoachingComingUpNoEventsTomorrow);
      List<ReminderViewModel> list = source.Select<Microsoft.Health.App.Core.Providers.Coaching.Reminder, ReminderViewModel>((Func<Microsoft.Health.App.Core.Providers.Coaching.Reminder, ReminderViewModel>) (r => ReminderViewModel.FromProviderModel(r))).ToList<ReminderViewModel>();
      this.UpdateHabitIdDupicateIndices((IEnumerable<ReminderViewModel>) list);
      foreach (ReminderViewModel reminder in list)
      {
        if (reminder.Day < tomorrow)
          this.RemindersForToday.AddReminder(reminder);
        else if (reminder.Day >= this.dayRange.High)
          CoachingComingUpViewModel.Logger.Warn((object) ("Event " + reminder.Name + " had a date past tomorrow, dropping."));
        else
          this.RemindersForTomorrow.AddReminder(reminder);
      }
      if (result2 != null && result2.Count > 0)
        this.ChooseAndShowInsight(result2);
      else if (!this.RemindersForToday.ShowReminders && !this.RemindersForTomorrow.ShowReminders)
      {
        this.shownInsight = (RaisedInsight) null;
        this.ShowInsight = true;
        this.InsightText = AppResources.CoachingComingUpNoEventsHeader;
        this.ShowDismissButton = false;
        this.ShowOpenGoalButton = false;
        this.ShowImage = true;
        this.PopulateRandomImage();
      }
      else
      {
        this.shownInsight = (RaisedInsight) null;
        this.ShowInsight = false;
      }
    }

    private void ChooseAndShowInsight(IList<RaisedInsight> insights)
    {
      RaisedInsight raisedInsight = insights.FirstOrDefault<RaisedInsight>((Func<RaisedInsight, bool>) (i => i.CategoryPivot.Contains(InsightCategoryPivot.SleepCareNewGoal)));
      if (raisedInsight == null)
      {
        this.shownInsight = insights.FirstOrDefault<RaisedInsight>((Func<RaisedInsight, bool>) (i => i.CategoryPivot.Contains(InsightCategoryPivot.ActivityPlanNewGoal)));
        if (this.shownInsight == null)
          this.shownInsight = insights.FirstOrDefault<RaisedInsight>((Func<RaisedInsight, bool>) (i => i.CategoryPivot.Contains(InsightCategoryPivot.SleepCarePlanProgress)));
        if (this.shownInsight == null)
          this.shownInsight = insights.FirstOrDefault<RaisedInsight>((Func<RaisedInsight, bool>) (i => i.CategoryPivot.Contains(InsightCategoryPivot.ActivityPlanProgress)));
        if (this.shownInsight == null)
          this.shownInsight = insights[0];
        this.ShowDismissButton = true;
        this.ShowOpenGoalButton = false;
        this.PopulateRandomImage();
      }
      else
      {
        this.shownInsight = raisedInsight;
        this.ShowDismissButton = false;
        this.ShowOpenGoalButton = true;
        this.PopulateSleepDurationGoalImage();
      }
      this.ShowInsight = true;
      this.InsightText = this.shownInsight.IM_Msg;
      this.ShowImage = true;
    }

    public void OnBackNavigation(bool isTileOpen)
    {
      if (!this.coachingService.ComingUpPendingRefresh)
        return;
      this.Refresh();
    }

    private void UpdateHabitIdDupicateIndices(IEnumerable<ReminderViewModel> reminderViewModels)
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      DateTimeOffset dateTimeOffset = this.dateTimeService.GetToday().AddDays(-1.0);
      foreach (ReminderViewModel reminderViewModel in reminderViewModels)
      {
        if (reminderViewModel.Day.Date != dateTimeOffset.Date)
        {
          dictionary.Clear();
          dateTimeOffset = reminderViewModel.Day;
        }
        if (!dictionary.ContainsKey(reminderViewModel.HabitId))
          dictionary.Add(reminderViewModel.HabitId, 0);
        int num = dictionary[reminderViewModel.HabitId];
        reminderViewModel.HabitIdDuplicateIndex = num;
        dictionary[reminderViewModel.HabitId] = num + 1;
      }
    }

    private void PopulateRandomImage()
    {
      int index = new Random().Next(CoachingComingUpViewModel.InsightImages.Length);
      this.ImageSource = new EmbeddedOrRemoteImageSource(CoachingComingUpViewModel.InsightImages[index]);
    }

    private void PopulateSleepDurationGoalImage() => this.ImageSource = new EmbeddedOrRemoteImageSource(EmbeddedAsset.CoachingAlarm);
  }
}
