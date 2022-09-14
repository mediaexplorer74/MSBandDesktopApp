// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Post.WorkoutResultGroupViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Post
{
  public class WorkoutResultGroupViewModel : WorkoutResultItemViewModel
  {
    public WorkoutResultGroupViewModel() => this.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);

    public CircuitType CircuitType { get; set; }

    public CircuitGroupType CircuitGroupType { get; set; }

    public string Glyph { get; set; }

    public bool CanExpand => this.Children != null && this.Children.Count > 0;

    public bool BottomLineVisible => !this.Expanded || this.Children == null || this.Children.Count == 0;

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "Expanded"))
        return;
      this.RaisePropertyChanged("BottomLineVisible");
    }
  }
}
