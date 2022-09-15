// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.GuidedWorkoutResultTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.Workouts.Panels;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Guided Workout Results"})]
  public class GuidedWorkoutResultTileViewModel : EventTileViewModelBase<WorkoutEvent>
  {
    private static readonly TimeSpan AgeCurrentThreshold = TimeSpan.FromDays(14.0);
    private readonly IBestEventProvider bestEventProvider;
    private readonly INetworkService networkService;
    private readonly IFormattingService formattingService;
    private readonly IEventTrackingService eventTracker;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private string workoutDetailName;
    private WorkoutPlanDetail workoutPlan;

    public GuidedWorkoutResultTileViewModel(
      ISmoothNavService navigation,
      IEventTrackingService eventTracker,
      IWorkoutsProvider provider,
      IBestEventProvider bestEventProvider,
      INetworkService networkService,
      IMessageSender messageSender,
      IFormattingService formattingService,
      WorkoutSummaryViewModel workoutSummaryViewModel,
      WorkoutResultViewModel workoutResultViewModel,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(eventTracker, EventType.GuidedWorkout, networkService, messageSender, navigation, (EventSummaryViewModelBase<WorkoutEvent>) workoutSummaryViewModel, firstTimeUse)
    {
      this.TileIcon = "\uE003";
      this.provider = provider;
      this.navigation = navigation;
      this.eventTracker = eventTracker;
      this.bestEventProvider = bestEventProvider;
      this.networkService = networkService;
      this.formattingService = formattingService;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotSummary, (object) workoutSummaryViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.WorkoutDetailsPanelTitle, (object) workoutResultViewModel));
      this.FirstTimeUse.IsSupported = true;
      this.FirstTimeUse.Message = AppResources.TileFirstTimeUseGuidedWorkoutMessage;
      this.FirstTimeUse.Type = TileFirstTimeUseViewModel.FirstTimeUseType.GuidedWorkout;
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EventTileViewModelBase<WorkoutEvent>.Logger.Debug((object) "<START> loading the guided workout result tile");
      string eventId;
      if (parameters != null && parameters.TryGetValue("ID", out eventId))
        this.Event = await this.provider.GetWorkoutEventAsync(eventId);
      else
        this.Event = await this.provider.GetLastCompletedWorkoutAsync();
      if (this.Event != null)
      {
        GuidedWorkoutResultTileViewModel resultTileViewModel = this;
        WorkoutPlanDetail workoutPlan = resultTileViewModel.workoutPlan;
        WorkoutPlanDetail workoutAsync = await this.provider.GetWorkoutAsync(this.Event.WorkoutPlanId);
        resultTileViewModel.workoutPlan = workoutAsync;
        resultTileViewModel = (GuidedWorkoutResultTileViewModel) null;
      }
      this.RefreshColorLevel();
      EventTileViewModelBase<WorkoutEvent>.Logger.Debug((object) "<END> loading the guided workout result tile");
      if (this.Event == null && !this.networkService.IsInternetAvailable)
        throw new InternetException("Internet not available.");
      return this.Event != null && this.workoutPlan != null;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.IsBest = (await this.bestEventProvider.GetLabelForEventAsync(this.Event.EventId, EventType.GuidedWorkout)).Key;
      this.workoutDetailName = (string) null;
      if (this.Event.WorkoutWeekId >= 1 && this.Event.WorkoutDayId >= 1)
      {
        try
        {
          GuidedWorkoutResultTileViewModel resultTileViewModel = this;
          string workoutDetailName = resultTileViewModel.workoutDetailName;
          string workoutNameAsync = await this.provider.GetWorkoutNameAsync(this.workoutPlan.WorkoutPlanId, this.Event.WorkoutWeekId, this.Event.WorkoutDayId);
          resultTileViewModel.workoutDetailName = workoutNameAsync;
          resultTileViewModel = (GuidedWorkoutResultTileViewModel) null;
        }
        catch (Exception ex)
        {
          await this.OnTransitionToErrorStateAsync(ex);
          return;
        }
      }
      this.Header = this.formattingService.FormatCalories(new int?(this.Event.CaloriesBurned), true);
      this.Subheader = this.formattingService.FormatTileTime(this.Event.StartTime);
      if (!string.IsNullOrEmpty(this.Event.Name))
        this.Subheader += string.Format("\n{0}", new object[1]
        {
          (object) this.Event.Name
        });
      else if (string.IsNullOrWhiteSpace(this.workoutDetailName) || this.workoutPlan.Details.Count == 1 && this.workoutPlan.Weeks.Count == 1 && this.workoutPlan.Weeks[0].Days.Count == 1)
        this.Subheader += string.Format("\n{0}", new object[1]
        {
          (object) this.workoutPlan.Name
        });
      else
        this.Subheader += string.Format("\n{0} - {1}", new object[2]
        {
          (object) this.workoutPlan.Name,
          (object) this.workoutDetailName
        });
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.GuidedWorkoutResultTileNoContent;
    }

    protected override void RefreshColorLevel()
    {
      base.RefreshColorLevel();
      this.UpdateColorLevel(GuidedWorkoutResultTileViewModel.AgeCurrentThreshold);
    }

    protected override void OnOpen()
    {
      base.OnOpen();
      if (this.Event == null)
        return;
      this.eventTracker.ReportView(this.Event.EventId);
    }

    public override void Open()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("Tile", this.GetType().Name);
      if (this.Event != null && this.workoutPlan != null)
      {
        dictionary.Add("WorkoutPlanId", this.workoutPlan.WorkoutPlanId);
        dictionary.Add("WeekId", this.Event.WorkoutWeekId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        dictionary.Add("DayId", this.Event.WorkoutDayId.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        dictionary.Add("WorkoutIndex", this.Event.WorkoutIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      this.navigation.Navigate(typeof (TilesViewModel), (IDictionary<string, string>) dictionary);
    }
  }
}
