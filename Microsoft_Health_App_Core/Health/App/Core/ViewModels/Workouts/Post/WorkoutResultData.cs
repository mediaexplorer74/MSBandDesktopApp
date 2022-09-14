// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Workouts.Post.WorkoutResultData
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Workouts.Post
{
  public class WorkoutResultData
  {
    public TimeSpan Time { get; set; }

    public int Calories { get; set; }

    public int Reps { get; set; }

    public bool RepsAreEstimated { get; set; }

    public Length Distance { get; set; }

    public Speed Pace { get; set; }

    public Length CalculatedDistance { get; set; }

    public TimeSpan TimeForCalculatedDistance { get; set; }
  }
}
