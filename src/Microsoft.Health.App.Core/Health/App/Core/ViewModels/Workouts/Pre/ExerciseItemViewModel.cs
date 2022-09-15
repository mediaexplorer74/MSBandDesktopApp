// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Pre.ExerciseItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Pre
{
  public class ExerciseItemViewModel : WorkoutItemViewModel
  {
    private HealthCommand openExerciseVideoCommand;

    public EmbeddedOrRemoteImageSource Image { get; set; }

    public string VideoId { get; set; }

    public string Completion { get; set; }

    public string Id { get; set; }

    public string Number { get; set; }

    public ICommand OpenExerciseVideoCommand => (ICommand) this.openExerciseVideoCommand ?? (ICommand) (this.openExerciseVideoCommand = new HealthCommand((Action) (() =>
    {
      if (this.Parent == null)
        return;
      this.Parent.OpenExerciseVideo(this);
    })));
  }
}
