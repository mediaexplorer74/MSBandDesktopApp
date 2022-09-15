// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutPlanScheduleWeekGroupingViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  public class WorkoutPlanScheduleWeekGroupingViewModel : 
    HealthObservableObject,
    IEnumerable<WorkoutPlanScheduleDayViewModel>,
    IEnumerable
  {
    private readonly WorkoutWeekGrouping workoutWeekGrouping;
    private readonly IList<WorkoutPlanScheduleDayViewModel> days;
    private bool isCollapsed;
    private bool isCollapsible = true;

    public WorkoutPlanScheduleWeekGroupingViewModel(
      WorkoutPlanScheduleViewModel parent,
      WorkoutWeekGrouping workoutWeekGrouping)
    {
      Assert.ParamIsNotNull((object) parent, nameof (parent));
      Assert.ParamIsNotNull((object) workoutWeekGrouping, nameof (workoutWeekGrouping));
      this.workoutWeekGrouping = workoutWeekGrouping;
      this.days = (IList<WorkoutPlanScheduleDayViewModel>) new ObservableCollection<WorkoutPlanScheduleDayViewModel>(workoutWeekGrouping.Select<WorkoutScheduleDay, WorkoutPlanScheduleDayViewModel>((Func<WorkoutScheduleDay, WorkoutPlanScheduleDayViewModel>) (g => new WorkoutPlanScheduleDayViewModel(parent, g))));
    }

    public bool IsCollapsible
    {
      get => this.isCollapsible;
      set => this.SetProperty<bool>(ref this.isCollapsible, value, nameof (IsCollapsible));
    }

    public bool IsCollapsed
    {
      get => this.isCollapsed;
      set => this.SetProperty<bool>(ref this.isCollapsed, value, nameof (IsCollapsed));
    }

    public string PlanName => this.workoutWeekGrouping.PlanName;

    public string WeekName => this.workoutWeekGrouping.WeekName;

    public void ToggleExpansion()
    {
      if (!this.IsCollapsible)
        return;
      foreach (WorkoutPlanScheduleDayViewModel day in (IEnumerable<WorkoutPlanScheduleDayViewModel>) this.days)
        day.IsCollapsed = !day.IsCollapsed;
      this.IsCollapsed = !this.IsCollapsed;
    }

    public IEnumerator<WorkoutPlanScheduleDayViewModel> GetEnumerator() => this.days.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.days.GetEnumerator();
  }
}
