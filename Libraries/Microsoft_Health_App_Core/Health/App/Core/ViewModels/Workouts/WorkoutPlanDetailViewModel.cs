// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.WorkoutPlanDetailViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Workouts
{
  public sealed class WorkoutPlanDetailViewModel : HealthViewModelBase
  {
    private readonly WorkoutPlanDetail workout;
    private IList<WorkoutPlanStatViewModel> stats;

    public WorkoutPlanDetailViewModel(WorkoutPlanDetail workout)
    {
      Assert.ParamIsNotNull((object) workout, nameof (workout));
      this.workout = workout;
    }

    public IList<WorkoutPlanStatViewModel> Stats
    {
      get
      {
        if (this.stats == null)
          this.stats = (IList<WorkoutPlanStatViewModel>) new List<WorkoutPlanStatViewModel>((IEnumerable<WorkoutPlanStatViewModel>) new WorkoutPlanStatViewModel[4]
          {
            new WorkoutPlanStatViewModel(AppResources.WorkoutLabelGoals, string.Join(Environment.NewLine, (IEnumerable<string>) this.Goals)),
            new WorkoutPlanStatViewModel(AppResources.WorkoutLabelDuration, this.DurationMinsFormatted),
            new WorkoutPlanStatViewModel(AppResources.WorkoutLabelBodyParts, string.Join(Environment.NewLine, (IEnumerable<string>) this.BodyParts)),
            new WorkoutPlanStatViewModel(AppResources.WorkoutLabelDifficulty, this.Level)
          });
        return this.stats;
      }
    }

    public IList<string> BodyParts => this.workout.BodyParts;

    public string Description => this.workout.Description;

    public string DisplayType => this.workout.DisplayType;

    public IList<WorkoutDetailItem> Details => this.workout.Details;

    public string DurationMinsFormatted => (string) Formatter.FormatDurationMinutesLocked(TimeSpan.FromMinutes((double) this.workout.Duration));

    public IList<string> Focus => this.workout.Focus;

    public string Gender => this.workout.Gender;

    public IList<string> Goals => this.workout.Goals;

    public string HowTo => this.workout.HowTo;

    public string WorkoutPlanId => this.workout.WorkoutPlanId;

    public string ImageAttributes => this.workout.ImageAttributes;

    public string Level => this.workout.Level;

    public string Market => this.workout.Market;

    public string Name => this.workout.Name;

    public string Image => this.workout.Image;

    public IList<RelatedWorkoutPlan> Related => this.workout.Related;

    public string Type => this.workout.Type;

    public IList<WorkoutPlanWeek> Weeks => this.workout.Weeks;
  }
}
