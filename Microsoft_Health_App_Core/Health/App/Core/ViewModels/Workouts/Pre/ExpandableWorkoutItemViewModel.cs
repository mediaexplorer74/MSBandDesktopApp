// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Pre.ExpandableWorkoutItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Pre
{
  public class ExpandableWorkoutItemViewModel : WorkoutItemViewModel
  {
    private HealthCommand toggleExpandedCommand;

    public ICommand ToggleExpandedCommand => (ICommand) this.toggleExpandedCommand ?? (ICommand) (this.toggleExpandedCommand = new HealthCommand((Action) (() =>
    {
      if (this.Parent == null)
        return;
      this.Parent.ToggleExpanded((WorkoutItemViewModel) this);
    })));
  }
}
