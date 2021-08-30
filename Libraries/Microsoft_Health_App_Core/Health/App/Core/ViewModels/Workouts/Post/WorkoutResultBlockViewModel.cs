// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Post.WorkoutResultBlockViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Post
{
  public class WorkoutResultBlockViewModel : WorkoutResultItemViewModel
  {
    public CircuitType CircuitType { get; set; }

    public override IList<WorkoutResultItemViewModel> ViewChildren => (IList<WorkoutResultItemViewModel>) new List<WorkoutResultItemViewModel>();
  }
}
