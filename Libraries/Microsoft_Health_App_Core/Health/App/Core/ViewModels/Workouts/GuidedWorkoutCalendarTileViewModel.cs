// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.GuidedWorkoutCalendarTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
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
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Guided Workouts Calendar"})]
  public class GuidedWorkoutCalendarTileViewModel : MetricTileViewModel
  {
    private readonly WorkoutDetailViewModel details;
    private readonly ISmoothNavService navigation;
    private readonly IWorkoutsProvider provider;
    private DateTimeOffset? lastSubscriptionChanged;
    private GuidedWorkoutState workoutStatus;
    private GuidedWorkoutInfo nextWorkout;
    private WorkoutPlanDetail workoutPlan;

    public GuidedWorkoutCalendarTileViewModel(
      IWorkoutsProvider provider,
      ISmoothNavService navigation,
      INetworkService networkService,
      IMessageSender messageSender,
      WorkoutDetailViewModel workoutDetailViewModel,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, navigation, messageSender, firstTimeUse)
    {
      this.TileIcon = "\uE055";
      this.provider = provider;
      this.navigation = navigation;
      this.details = workoutDetailViewModel;
      this.ShowPivotHeader = false;
      this.details.ShowFooter = true;
      this.Pivots.Add(new PivotDefinition(string.Empty, (object) this.details));
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      GuidedWorkoutCalendarTileViewModel calendarTileViewModel = this;
      GuidedWorkoutState workoutStatus = calendarTileViewModel.workoutStatus;
      GuidedWorkoutState workoutStateAsync = await this.provider.GetWorkoutStateAsync(CancellationToken.None);
      calendarTileViewModel.workoutStatus = workoutStateAsync;
      calendarTileViewModel = (GuidedWorkoutCalendarTileViewModel) null;
      this.workoutPlan = (WorkoutPlanDetail) null;
      if (this.workoutStatus.WorkoutInfo != null)
      {
        GuidedWorkoutInfo subscribedWorkoutAsync = await this.provider.GetSubscribedWorkoutAsync();
        this.nextWorkout = new GuidedWorkoutInfo()
        {
          DayId = this.workoutStatus.WorkoutInfo.DayId,
          WeekId = this.workoutStatus.WorkoutInfo.WeekId,
          WorkoutIndex = this.workoutStatus.WorkoutInfo.WorkoutIndex,
          WorkoutPlanId = this.workoutStatus.WorkoutInfo.WorkoutPlanId,
          WorkoutPlanInstanceId = this.workoutStatus.WorkoutInfo.WorkoutPlanInstanceId,
          IsRestDay = this.workoutStatus.State == GuidedWorkoutSyncState.RestDay,
          IsSyncedToBand = this.workoutStatus.State == GuidedWorkoutSyncState.OnBand,
          IsSubscribed = subscribedWorkoutAsync != null && this.workoutStatus.WorkoutInfo.WorkoutPlanId == subscribedWorkoutAsync.WorkoutPlanId
        };
        calendarTileViewModel = this;
        WorkoutPlanDetail workoutPlan = calendarTileViewModel.workoutPlan;
        WorkoutPlanDetail workoutAsync = await this.provider.GetWorkoutAsync(this.nextWorkout.WorkoutPlanId);
        calendarTileViewModel.workoutPlan = workoutAsync;
        calendarTileViewModel = (GuidedWorkoutCalendarTileViewModel) null;
        if (string.IsNullOrWhiteSpace(this.nextWorkout.WorkoutName))
        {
          GuidedWorkoutInfo guidedWorkoutInfo = this.nextWorkout;
          string workoutNameAsync = await this.provider.GetWorkoutNameAsync(this.nextWorkout.WorkoutPlanId, this.nextWorkout.WeekId, this.nextWorkout.DayId);
          guidedWorkoutInfo.WorkoutName = workoutNameAsync;
          guidedWorkoutInfo = (GuidedWorkoutInfo) null;
        }
      }
      this.CanOpen = true;
      return true;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      if (this.nextWorkout != null && this.workoutPlan != null)
      {
        if (this.workoutPlan.Name.Equals(this.nextWorkout.WorkoutName))
          this.Subheader = this.workoutPlan.Name;
        else
          this.Subheader = string.Format(AppResources.GuidedWorkoutsCalendarTileSubTitleFormat, new object[2]
          {
            (object) this.workoutPlan.Name,
            (object) this.nextWorkout.WorkoutName
          });
      }
      else
        this.Subheader = AppResources.GuidedWorkoutsNoWorkoutTileText;
      this.ColorLevel = TileColorLevel.Medium;
    }

    protected override async void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      DateTimeOffset? subscriptionChanged = this.provider.LastSubscriptionChanged;
      if (!subscriptionChanged.HasValue || this.lastSubscriptionChanged.HasValue && !(this.lastSubscriptionChanged.Value < subscriptionChanged.Value))
        return;
      await this.LoadAsync((IDictionary<string, string>) null);
      await this.details.RefreshAsync();
    }

    protected override void OnNavigatedFrom()
    {
      base.OnNavigatedFrom();
      this.lastSubscriptionChanged = this.provider.LastSubscriptionChanged;
    }

    public override void Open()
    {
      if (this.nextWorkout != null)
        base.Open();
      else
        this.navigation.Navigate(typeof (WorkoutPlanLandingViewModel));
    }

    public override bool WillOpenOnTileCommand() => this.nextWorkout != null && base.WillOpenOnTileCommand();
  }
}
