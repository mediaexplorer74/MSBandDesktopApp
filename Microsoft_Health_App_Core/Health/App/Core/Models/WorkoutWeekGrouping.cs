// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.WorkoutWeekGrouping
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.Health.App.Core.Models
{
  public class WorkoutWeekGrouping : List<WorkoutScheduleDay>, INotifyPropertyChanged
  {
    private bool isCollapsed;
    private bool isCollapsible = true;

    public WorkoutWeekGrouping(IEnumerable<WorkoutScheduleDay> items)
      : base(items)
    {
    }

    public bool IsCollapsible
    {
      get => this.isCollapsible;
      set
      {
        this.isCollapsible = value;
        this.NotifyPropertyChanged(nameof (IsCollapsible));
      }
    }

    public bool IsCollapsed
    {
      get => this.isCollapsed;
      set
      {
        this.isCollapsed = value;
        this.NotifyPropertyChanged(nameof (IsCollapsed));
      }
    }

    public string PlanName { get; set; }

    public string WeekName { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public void ToggleExpansion()
    {
      if (!this.IsCollapsible)
        return;
      foreach (WorkoutScheduleDay workoutScheduleDay in (List<WorkoutScheduleDay>) this)
        workoutScheduleDay.IsCollapsed = !workoutScheduleDay.IsCollapsed;
      this.IsCollapsed = !this.IsCollapsed;
    }
  }
}
