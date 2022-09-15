// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Panels.WorkoutPlanScheduleDayViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Panels
{
  public class WorkoutPlanScheduleDayViewModel : HealthObservableObject
  {
    private readonly WorkoutPlanScheduleViewModel parent;
    private readonly WorkoutScheduleDay workoutScheduleDay;
    private HealthCommand syncWorkoutToDevice;
    private bool isCollapsed;

    public WorkoutPlanScheduleDayViewModel(
      WorkoutPlanScheduleViewModel parent,
      WorkoutScheduleDay workoutScheduleDay)
    {
      Assert.ParamIsNotNull((object) parent, nameof (parent));
      Assert.ParamIsNotNull((object) workoutScheduleDay, nameof (workoutScheduleDay));
      this.parent = parent;
      this.workoutScheduleDay = workoutScheduleDay;
    }

    public ICommand SyncWorkoutToDeviceCommand => (ICommand) this.syncWorkoutToDevice ?? (ICommand) (this.syncWorkoutToDevice = new HealthCommand((Action) (() => this.parent.CheckSyncStatus(this.workoutScheduleDay))));

    public int DayId => this.workoutScheduleDay.PlanValues.DayId;

    public string Workout
    {
      get
      {
        Assert.IsTrue(this.workoutScheduleDay.PlanValues.Workouts != null && this.workoutScheduleDay.PlanValues.Workouts.Count >= 1, "The workout information is not complete.");
        return this.workoutScheduleDay.PlanValues.Workouts[0];
      }
    }

    public bool IsRestDay => this.workoutScheduleDay.IsRestDay;

    public bool ShowBandItems => this.parent.ShowBandItems;

    public bool IsCollapsed
    {
      get => this.isCollapsed;
      set => this.SetProperty<bool>(ref this.isCollapsed, value, nameof (IsCollapsed));
    }

    public WorkoutDayState State => this.workoutScheduleDay.State;

    public WorkoutScheduleDay GetRawModel() => this.workoutScheduleDay;
  }
}
