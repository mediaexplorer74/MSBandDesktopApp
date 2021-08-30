// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.GuidedWorkoutSplitTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public sealed class GuidedWorkoutSplitTileViewModel : SplitTileViewModel
  {
    private readonly ISmoothNavService navigationService;
    private readonly IWorkoutsProvider workoutsProvider;
    private GuidedWorkoutState nextWorkout;
    private GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState tileState;

    public GuidedWorkoutSplitTileViewModel(
      ISmoothNavService navigationService,
      IWorkoutsProvider workoutsProvider)
    {
      Assert.ParamIsNotNull((object) navigationService, nameof (navigationService));
      Assert.ParamIsNotNull((object) workoutsProvider, nameof (workoutsProvider));
      this.navigationService = navigationService;
      this.workoutsProvider = workoutsProvider;
      this.TileState = GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.FindWorkout;
    }

    public GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState TileState
    {
      get => this.tileState;
      private set
      {
        this.SetProperty<GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState>(ref this.tileState, value, nameof (TileState));
        this.UpdateHeader();
      }
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      await this.UpdateStateAsync();
      return true;
    }

    protected override async void OnNavigatedBack()
    {
      base.OnNavigatedBack();
      await this.UpdateStateAsync();
    }

    protected override void OnTileCommand()
    {
      switch (this.TileState)
      {
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.FindWorkout:
          this.navigationService.Navigate(typeof (WorkoutPlanLandingViewModel));
          break;
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.WorkoutOnBand:
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.RestDay:
          this.navigationService.Navigate(typeof (PivotDetailsViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Type",
              "GuidedWorkoutsCalendar"
            },
            {
              "Page.HeaderType",
              HeaderType.Normal.ToString()
            },
            {
              "Page.TransitionPageType",
              TransitionPageType.L1WithHeader.ToString()
            }
          });
          break;
      }
    }

    private async Task UpdateStateAsync()
    {
      try
      {
        GuidedWorkoutSplitTileViewModel splitTileViewModel = this;
        GuidedWorkoutState nextWorkout = splitTileViewModel.nextWorkout;
        GuidedWorkoutState workoutStateAsync = await this.workoutsProvider.GetWorkoutStateAsync(CancellationToken.None);
        splitTileViewModel.nextWorkout = workoutStateAsync;
        splitTileViewModel = (GuidedWorkoutSplitTileViewModel) null;
        this.TileState = this.nextWorkout == null || this.nextWorkout.State == GuidedWorkoutSyncState.NoWorkout || this.nextWorkout.State == GuidedWorkoutSyncState.Unknown ? GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.FindWorkout : (this.nextWorkout.State != GuidedWorkoutSyncState.RestDay ? GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.WorkoutOnBand : GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.RestDay);
      }
      catch
      {
        this.TileState = GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.FindWorkout;
      }
    }

    private void UpdateHeader()
    {
      switch (this.TileState)
      {
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.FindWorkout:
          this.Header = AppResources.GuidedWorkoutSplitTileFindWorkout;
          break;
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.WorkoutOnBand:
          this.Header = AppResources.GuidedWorkoutSplitTileWorkoutOnBand;
          break;
        case GuidedWorkoutSplitTileViewModel.GuidedWorkoutTileState.RestDay:
          this.Header = AppResources.GuidedWorkoutSplitTileRestDay;
          break;
      }
    }

    public enum GuidedWorkoutTileState
    {
      FindWorkout,
      WorkoutOnBand,
      RestDay,
    }
  }
}
