// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.Reminder
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class Reminder
  {
    private const string AssociatedCommandKey = "AssociatedCommand";

    public Reminder(
      string habitId,
      string habitName,
      string planName,
      bool enabled,
      DateTimeOffset targetTime,
      DateTimeOffset? allowableStart,
      DateTimeOffset? allowableEnd,
      AssociatedCommand associatedCommand,
      bool hasRestrictedSchedule,
      bool canEditProperties,
      bool canEditReminder,
      bool canSkip,
      string guidedWorkoutPlanId,
      int? guidedWorkoutIndex,
      int? guidedWorkoutWeekId,
      int? guidedWorkoutDayId)
    {
      this.HabitId = habitId;
      this.HabitName = habitName;
      this.PlanName = planName;
      this.Enabled = enabled;
      this.TargetTime = targetTime;
      this.AllowableStart = allowableStart;
      this.AllowableEnd = allowableEnd;
      this.AssociatedCommand = associatedCommand;
      this.HasRestrictedSchedule = hasRestrictedSchedule;
      this.CanEditProperties = canEditProperties;
      this.CanEditReminder = canEditReminder;
      this.CanSkip = canSkip;
      this.GuidedWorkoutPlanId = guidedWorkoutPlanId;
      this.GuidedWorkoutIndex = guidedWorkoutIndex;
      this.GuidedWorkoutWeekId = guidedWorkoutWeekId;
      this.GuidedWorkoutDayId = guidedWorkoutDayId;
    }

    public string HabitId { get; }

    public string HabitName { get; }

    public string PlanName { get; }

    public bool Enabled { get; }

    public DateTimeOffset TargetTime { get; }

    public DateTimeOffset? AllowableStart { get; }

    public DateTimeOffset? AllowableEnd { get; }

    public AssociatedCommand AssociatedCommand { get; }

    public bool HasRestrictedSchedule { get; }

    public bool CanEditProperties { get; }

    public bool CanEditReminder { get; }

    public bool CanSkip { get; }

    public string GuidedWorkoutPlanId { get; set; }

    public int? GuidedWorkoutIndex { get; set; }

    public int? GuidedWorkoutWeekId { get; set; }

    public int? GuidedWorkoutDayId { get; set; }

    public static Reminder FromCloudModel(Microsoft.Health.Cloud.Client.Reminder cloudReminder)
    {
      bool flag = cloudReminder.HasRestrictedSchedule;
      DateTimeOffset? nullable1;
      DateTimeOffset dateTimeOffset1;
      DateTimeOffset? nullable2;
      if (cloudReminder.AllowableTimeRanges.Count > 0)
      {
        ref DateTimeOffset? local1 = ref nullable1;
        dateTimeOffset1 = cloudReminder.AllowableTimeRanges[0].Start;
        DateTimeOffset dateTimeOffset2 = dateTimeOffset1.AddMinutes((double) cloudReminder.TargetTZOffsetMin);
        local1 = new DateTimeOffset?(dateTimeOffset2);
        ref DateTimeOffset? local2 = ref nullable2;
        dateTimeOffset1 = cloudReminder.AllowableTimeRanges[0].End;
        DateTimeOffset dateTimeOffset3 = dateTimeOffset1.AddMinutes((double) cloudReminder.TargetTZOffsetMin);
        local2 = new DateTimeOffset?(dateTimeOffset3);
      }
      else
      {
        flag = false;
        nullable1 = new DateTimeOffset?();
        nullable2 = new DateTimeOffset?();
      }
      string habitId = cloudReminder.HabitId;
      string habitName = cloudReminder.HabitName;
      string planName = cloudReminder.PlanName;
      int num1 = cloudReminder.Enabled ? 1 : 0;
      dateTimeOffset1 = cloudReminder.TargetUtc;
      DateTimeOffset targetTime = dateTimeOffset1.AddMinutes((double) cloudReminder.TargetTZOffsetMin);
      DateTimeOffset? allowableStart = nullable1;
      DateTimeOffset? allowableEnd = nullable2;
      int associatedCommand = (int) Reminder.GetAssociatedCommand(cloudReminder.Conditions);
      int num2 = flag ? 1 : 0;
      int num3 = cloudReminder.CanEditProperties ? 1 : 0;
      int num4 = cloudReminder.CanEditReminder ? 1 : 0;
      int num5 = cloudReminder.CanSkipInstance ? 1 : 0;
      string guidedWorkoutPlanId = cloudReminder.GuidedWorkoutPlanId;
      int? guidedWorkoutIndex = cloudReminder.GuidedWorkoutIndex;
      int? guidedWorkoutWeekId = cloudReminder.GuidedWorkoutWeekId;
      int? guidedWorkoutDayId = cloudReminder.GuidedWorkoutDayId;
      return new Reminder(habitId, habitName, planName, num1 != 0, targetTime, allowableStart, allowableEnd, (AssociatedCommand) associatedCommand, num2 != 0, num3 != 0, num4 != 0, num5 != 0, guidedWorkoutPlanId, guidedWorkoutIndex, guidedWorkoutWeekId, guidedWorkoutDayId);
    }

    private static AssociatedCommand GetAssociatedCommand(Conditions conditions)
    {
      if (conditions == null || conditions.AssociatedCommand == null)
        return AssociatedCommand.None;
      string associatedCommand = conditions.AssociatedCommand;
      if (associatedCommand == "SET_SLEEP_NOTIFICATION")
        return AssociatedCommand.SetSleepNotification;
      return associatedCommand == "SET_LIGHT_EXPO_NOTIFICATION" ? AssociatedCommand.SetLightExposureNotification : AssociatedCommand.Unknown;
    }
  }
}
