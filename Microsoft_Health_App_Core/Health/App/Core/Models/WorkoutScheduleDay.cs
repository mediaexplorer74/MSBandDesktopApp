// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.WorkoutScheduleDay
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;

namespace Microsoft.Health.App.Core.Models
{
  public class WorkoutScheduleDay : HealthObservableObject
  {
    private bool isCollapsed;
    private WorkoutDayState state;

    public WorkoutPlanDay PlanValues { get; set; }

    public bool IsRestDay { get; set; }

    public int WeekId { get; set; }

    public DateTimeOffset? LastUpdatedTime { get; set; }

    public int WorkoutIndex { get; set; }

    public bool IsLastCompleted { get; set; }

    public bool IsSyncedToBand { get; set; }

    public WorkoutDayState State
    {
      get => this.state;
      set => this.SetProperty<WorkoutDayState>(ref this.state, value, nameof (State));
    }

    public bool IsCollapsed
    {
      get => this.isCollapsed;
      set => this.SetProperty<bool>(ref this.isCollapsed, value, nameof (IsCollapsed));
    }
  }
}
