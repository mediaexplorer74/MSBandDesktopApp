// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutDetailTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.Workouts.Panels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  [PageTaxonomy(new string[] {"Guided Workout Details"})]
  [MetricTileType(MetricTileType.Preview)]
  public class WorkoutDetailTileViewModel : MetricTileViewModel
  {
    public const string WorkoutPlanNameParameter = "WorkoutPlanName";
    public const string WorkoutDetailNameParameter = "WorkoutDetailName";

    public WorkoutDetailTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      WorkoutDetailViewModel workoutDetailViewModel,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      WorkoutDetailViewModel workoutDetailViewModel1 = workoutDetailViewModel;
      workoutDetailViewModel1.ShowFooter = false;
      this.Pivots.Add(new PivotDefinition(string.Empty, (object) workoutDetailViewModel1));
    }

    protected override Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters != null)
      {
        string str1;
        if (parameters.TryGetValue("WorkoutPlanName", out str1))
          this.PivotHeader = str1;
        string str2;
        if (parameters.TryGetValue("WorkoutDetailName", out str2) && this.Pivots.Count > 0)
          this.Pivots[0].Header = str2;
      }
      return Task.FromResult<bool>(true);
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.Subheader = (string) null;
    }
  }
}
