// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Pre.WorkoutItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.ViewModels.Workouts.Panels;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Pre
{
  public class WorkoutItemViewModel : HealthObservableObject
  {
    private bool visible = true;
    private bool expanded = true;

    public WorkoutDetailViewModel Parent { get; set; }

    public WorkoutItemViewModel()
    {
      this.Children = (IList<WorkoutItemViewModel>) new List<WorkoutItemViewModel>();
      this.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
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

    public string Text { get; set; }

    public IList<WorkoutItemViewModel> Children { get; set; }

    public virtual IList<WorkoutItemViewModel> ViewChildren => this.Children;

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "Expanded"))
        return;
      this.RaisePropertyChanged("ExpandGlyph");
    }
  }
}
