// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Post.WorkoutResultItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.ViewModels.Workouts.Panels;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Post
{
  public class WorkoutResultItemViewModel : HealthObservableObject
  {
    private bool visible = true;
    private bool expanded = true;
    private string metric;
    private string calculatedDistanceMetric;
    private HealthCommand toggleExpandedCommand;

    public WorkoutResultViewModel Parent { get; set; }

    public WorkoutResultItemViewModel()
    {
      this.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
      this.Children = (IList<WorkoutResultItemViewModel>) new List<WorkoutResultItemViewModel>();
    }

    public bool Visible
    {
      get => this.visible;
      set => this.SetProperty<bool>(ref this.visible, value, nameof (Visible));
    }

    public bool Expanded
    {
      get => this.expanded;
      set => this.SetProperty<bool>(ref this.expanded, value, nameof (Expanded));
    }

    public string ExpandGlyph => !this.Expanded ? "\uE100" : "\uE099";

    public ICommand ToggleExpandedCommand => (ICommand) this.toggleExpandedCommand ?? (ICommand) (this.toggleExpandedCommand = new HealthCommand((Action) (() => this.Parent?.ToggleExpanded(this))));

    public string Text { get; set; }

    public IList<WorkoutResultItemViewModel> Children { get; private set; }

    public virtual IList<WorkoutResultItemViewModel> ViewChildren => this.Children;

    public WorkoutResultData ResultData { get; set; }

    public string Metric
    {
      get => this.metric;
      set => this.SetProperty<string>(ref this.metric, value, nameof (Metric));
    }

    public string CalculatedDistanceMetric
    {
      get => this.calculatedDistanceMetric;
      set => this.SetProperty<string>(ref this.calculatedDistanceMetric, value, nameof (CalculatedDistanceMetric));
    }

    public void RollUpResultData()
    {
      this.ResultData = new WorkoutResultData();
      foreach (WorkoutResultItemViewModel child in (IEnumerable<WorkoutResultItemViewModel>) this.Children)
      {
        this.ResultData.Time += child.ResultData.Time;
        this.ResultData.Calories += child.ResultData.Calories;
        this.ResultData.Reps += child.ResultData.Reps;
        this.ResultData.Distance += child.ResultData.Distance;
        this.ResultData.CalculatedDistance += child.ResultData.CalculatedDistance;
        if (child.ResultData.CalculatedDistance > Length.Zero)
        {
          this.ResultData.TimeForCalculatedDistance += child.ResultData.Time;
          this.ResultData.Pace = Speed.FromDistanceAndTime(this.ResultData.CalculatedDistance, this.ResultData.TimeForCalculatedDistance);
        }
        if (child.ResultData.RepsAreEstimated)
          this.ResultData.RepsAreEstimated = true;
      }
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "Expanded"))
        return;
      this.RaisePropertyChanged("ExpandGlyph");
    }
  }
}
