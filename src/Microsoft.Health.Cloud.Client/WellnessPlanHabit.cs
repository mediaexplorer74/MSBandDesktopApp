// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WellnessPlanHabit
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WellnessPlanHabit
  {
    public WellnessPlanHabit()
    {
      this.Schedules = (IList<WellnessPlanSchedule>) new List<WellnessPlanSchedule>();
      this.Activities = (IList<WellnessPlanActivity>) new List<WellnessPlanActivity>();
    }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Owner { get; set; }

    [DataMember]
    public string TemplateId { get; set; }

    [DataMember]
    public bool? IsReminderEnabled { get; set; }

    [DataMember]
    public WellnessHabitStatus Status { get; set; }

    [DataMember]
    public string ImageUrl { get; set; }

    [DataMember]
    public string ThumbnailImageUrl { get; set; }

    [DataMember]
    public IList<WellnessPlanSchedule> Schedules { get; private set; }

    [DataMember]
    public IList<WellnessPlanActivity> Activities { get; private set; }

    [DataMember]
    public WellnessPlanHabitFlags Flags { get; set; }
  }
}
