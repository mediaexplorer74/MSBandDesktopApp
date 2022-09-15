// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutPlanDetailTileViewModel
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
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Guided Workout Plan Details"})]
  [MetricTileType(MetricTileType.Preview)]
  public class WorkoutPlanDetailTileViewModel : MetricTileViewModel
  {
    public const string WorkoutPlanIdParameter = "WorkoutPlanId";
    private readonly IWorkoutsProvider provider;
    private readonly IServiceLocator serviceLocator;

    public WorkoutPlanDetailTileViewModel(
      IWorkoutsProvider provider,
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      WorkoutPlanOverviewViewModel workoutPlanOverviewViewModel,
      IServiceLocator serviceLocator,
      IMessageSender messageSender,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      this.provider = provider;
      this.serviceLocator = serviceLocator;
      this.Pivots.Add(new PivotDefinition(AppResources.WorkoutOverviewPanelTitle, (object) workoutPlanOverviewViewModel));
    }

    protected override string LoadingMessage => string.Empty;

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string id;
      if (parameters != null && parameters.TryGetValue("WorkoutPlanId", out id))
      {
        WorkoutPlanDetail workoutAsync = await this.provider.GetWorkoutAsync(id);
        if (workoutAsync != null)
        {
          this.PivotHeader = workoutAsync.Name;
          if (workoutAsync.Details.Count == 1 && workoutAsync.Weeks.Count == 1 && workoutAsync.Weeks[0].Days.Count == 1)
          {
            if (!this.Pivots.Any<PivotDefinition>((Func<PivotDefinition, bool>) (p => p.Content is WorkoutDetailViewModel)))
            {
              parameters["WeekId"] = "1";
              parameters["DayId"] = "1";
              parameters["WorkoutIndex"] = "0";
              this.Pivots.Add(new PivotDefinition(AppResources.WorkoutDetailsPanelTitle, (object) this.serviceLocator.GetInstance<WorkoutDetailViewModel>()));
            }
          }
          else if (!this.Pivots.Any<PivotDefinition>((Func<PivotDefinition, bool>) (p => p.Content is WorkoutPlanScheduleViewModel)))
            this.Pivots.Add(new PivotDefinition(AppResources.WorkoutSchedulePanelTitle, (object) this.serviceLocator.GetInstance<WorkoutPlanScheduleViewModel>()));
        }
      }
      return true;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.Subheader = (string) null;
    }
  }
}
