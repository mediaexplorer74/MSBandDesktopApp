// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Reminder
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class Reminder
  {
    public Reminder() => this.AllowableTimeRanges = (IList<Microsoft.Health.Cloud.Client.AllowableTimeRanges>) new List<Microsoft.Health.Cloud.Client.AllowableTimeRanges>();

    [DataMember]
    public bool Enabled { get; set; }

    [DataMember]
    public string HabitId { get; set; }

    [DataMember]
    public string HabitName { get; set; }

    [DataMember]
    public Uri ImageUrl { get; set; }

    [DataMember]
    public Uri ThumbnailImageUrl { get; set; }

    [DataMember]
    public string PlanId { get; set; }

    [DataMember]
    public string PlanName { get; set; }

    [DataMember]
    public int Type { get; set; }

    [DataMember]
    public DateTimeOffset TargetUtc { get; set; }

    [DataMember]
    public int TargetTZOffsetMin { get; set; }

    [DataMember]
    public int AdditionalOffsetSec { get; set; }

    [DataMember]
    public IList<Microsoft.Health.Cloud.Client.AllowableTimeRanges> AllowableTimeRanges { get; private set; }

    [DataMember]
    public Conditions Conditions { get; set; }

    [DataMember]
    public bool HasRestrictedSchedule { get; set; }

    [DataMember]
    public bool CanBeRemoved { get; set; }

    [DataMember]
    public bool CanEditProperties { get; set; }

    [DataMember]
    public bool CanEditReminder { get; set; }

    [DataMember]
    public bool CanSkipInstance { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string GuidedWorkoutPlanId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int? GuidedWorkoutIndex { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int? GuidedWorkoutWeekId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int? GuidedWorkoutDayId { get; set; }
  }
}
