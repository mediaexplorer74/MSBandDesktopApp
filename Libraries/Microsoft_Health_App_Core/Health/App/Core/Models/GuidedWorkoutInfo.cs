// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.GuidedWorkoutInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class GuidedWorkoutInfo
  {
    public string WorkoutPlanId { get; set; }

    public string WorkoutPlanName { get; set; }

    public string WorkoutName { get; set; }

    public int WorkoutPlanInstanceId { get; set; }

    public int DayId { get; set; }

    public int WeekId { get; set; }

    public bool IsRestDay { get; set; }

    public bool IsSubscribed { get; set; }

    public bool IsSyncedToBand { get; set; }

    public int WorkoutIndex { get; set; }
  }
}
