// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.SleepPlanRecommendationViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class SleepPlanRecommendationViewModel : PageViewModelBase
  {
    public const string InsightIdParameterName = "InsightId";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Coaching\\SleepPlanRecommendationViewModel.cs");
    private readonly ICoachingProvider coachingProvider;
    private readonly ICoachingService coachingService;
    private readonly IInsightsProvider insightsProvider;
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private string insightId;
    private SleepDurationRecommendation recommendation;
    private string question;
    private StyledSpan newGoalDuration;
    private IList<RecommendationChoiceViewModel> choices;
    private RecommendationChoiceViewModel selectedChoice;
    private string extraText;
    private AsyncHealthCommand saveCommand;
    private HealthCommand cancelCommand;

    public SleepPlanRecommendationViewModel(
      INetworkService networkService,
      ICoachingProvider coachingProvider,
      ICoachingService coachingService,
      IInsightsProvider insightsProvider,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService)
      : base(networkService)
    {
      this.coachingProvider = coachingProvider;
      this.coachingService = coachingService;
      this.insightsProvider = insightsProvider;
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.choices = (IList<RecommendationChoiceViewModel>) new ObservableCollection<RecommendationChoiceViewModel>();
    }

    public string Question
    {
      get => this.question;
      set => this.SetProperty<string>(ref this.question, value, nameof (Question));
    }

    public StyledSpan NewGoalDuration
    {
      get => this.newGoalDuration;
      set => this.SetProperty<StyledSpan>(ref this.newGoalDuration, value, nameof (NewGoalDuration));
    }

    public IList<RecommendationChoiceViewModel> Choices => this.choices;

    public RecommendationChoiceViewModel SelectedChoice
    {
      get => this.selectedChoice;
      set => this.SetProperty<RecommendationChoiceViewModel>(ref this.selectedChoice, value, nameof (SelectedChoice));
    }

    public string ExtraText
    {
      get => this.extraText;
      set => this.SetProperty<string>(ref this.extraText, value, nameof (ExtraText));
    }

    public ICommand SaveCommand => (ICommand) this.saveCommand ?? (ICommand) (this.saveCommand = AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      try
      {
        this.LoadState = LoadState.Loading;
        this.AppBar = (Microsoft.Health.App.Core.Models.AppBar.AppBar) null;
        await this.coachingProvider.UpdateSleepPlanDurationGoalAsync(this.recommendation, this.SelectedChoice.Option, this.insightId, CancellationToken.None);
        this.coachingService.ComingUpPendingRefresh = true;
        this.coachingService.TilePendingRefresh = true;
        this.ExitPage();
      }
      catch (Exception ex)
      {
        this.LoadState = LoadState.Loaded;
        this.ShowAppBar();
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    })));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand((Action) (() => this.ExitPage())));

    public EmbeddedOrRemoteImageSource ImageSource => new EmbeddedOrRemoteImageSource(EmbeddedAsset.CoachingAlarm);

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      RaisedInsight raisedInsight = (await this.insightsProvider.GetSleepPlanGoalInsightsAsync(CancellationToken.None)).FirstOrDefault<RaisedInsight>((Func<RaisedInsight, bool>) (i => !i.Acknowledged));
      if (raisedInsight == null)
      {
        SleepPlanRecommendationViewModel.Logger.Warn((object) "No unacknowledged sleep insights were found.");
        this.ExitPage();
      }
      else
      {
        this.insightId = raisedInsight.RaisedInsightId;
        SleepPlanRecommendationViewModel recommendationViewModel = this;
        SleepDurationRecommendation recommendation = recommendationViewModel.recommendation;
        SleepDurationRecommendation recommendationAsync = await this.coachingProvider.GetSleepDurationRecommendationAsync(CancellationToken.None);
        recommendationViewModel.recommendation = recommendationAsync;
        recommendationViewModel = (SleepPlanRecommendationViewModel) null;
        if (this.recommendation == null)
          throw new InvalidOperationException("Goal recommendation not found on plan. Cannot proceed.");
        this.choices.Clear();
        TimeSpan timeSpan = this.recommendation.RecommendedSleepDuration - this.recommendation.CurrentSleepDuration;
        bool flag = timeSpan > TimeSpan.Zero;
        RecommendationChoiceViewModel recommendationChoiceViewModel1 = new RecommendationChoiceViewModel(SleepPlanRecommendationOption.ChangeBedtime, flag ? AppResources.CoachingNewSleepTargetEarlierBed : AppResources.CoachingNewSleepTargetLaterBed, Formatter.FormatTime(this.recommendation.CurrentWakeUpTime - this.recommendation.RecommendedSleepDuration));
        RecommendationChoiceViewModel recommendationChoiceViewModel2 = new RecommendationChoiceViewModel(SleepPlanRecommendationOption.ChangeWakeUpTime, flag ? AppResources.CoachingNewSleepTargetLaterWakeUp : AppResources.CoachingNewSleepTargetEarlierWakeUp, Formatter.FormatTime(this.recommendation.CurrentWakeUpTime + timeSpan));
        int totalMinutes = (int) timeSpan.TotalMinutes;
        string text;
        if (!flag)
          text = AppResources.CoachingNewSleepTargetChangeDecline;
        else
          text = string.Format(AppResources.CoachingNewSleepTargetIncreaseDecline, new object[1]
          {
            (object) totalMinutes
          });
        RecommendationChoiceViewModel recommendationChoiceViewModel3 = new RecommendationChoiceViewModel(SleepPlanRecommendationOption.Decline, text);
        this.choices.Add(recommendationChoiceViewModel1);
        this.choices.Add(recommendationChoiceViewModel2);
        this.choices.Add(recommendationChoiceViewModel3);
        string str;
        if (!flag)
          str = AppResources.CoachingNewSleepTargetChangeQuestion;
        else
          str = string.Format(AppResources.CoachingNewSleepTargetIncreaseQuestion, new object[1]
          {
            (object) totalMinutes
          });
        this.Question = str;
        this.NewGoalDuration = Formatter.FormatTimeSpan(this.recommendation.RecommendedSleepDuration, Formatter.TimeSpanFormat.OneChar, false);
        this.SelectedChoice = recommendationChoiceViewModel1;
        this.ShowAppBar();
      }
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.SaveCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    private void ExitPage()
    {
      if (this.smoothNavService.CanGoBack)
        this.smoothNavService.GoBack();
      else
        this.smoothNavService.Navigate(typeof (TilesViewModel), action: NavigationStackAction.RemovePrevious);
    }
  }
}
