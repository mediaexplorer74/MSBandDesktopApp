// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserWellnessPlan
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserWellnessPlan
  {
    public UserWellnessPlan()
    {
      this.Habits = (IList<WellnessPlanHabit>) new List<WellnessPlanHabit>();
      this.Goals = (IList<UsersGoal>) new List<UsersGoal>();
    }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string AuthorBrand { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string AuthorLogoUrl { get; set; }

    [DataMember]
    public string PlanLogoUrl { get; set; }

    [DataMember]
    public WellnessPlanType PlanType { get; set; }

    [DataMember]
    public bool? IsAutoProgressEnabled { get; set; }

    [DataMember]
    public DateTimeOffset? CreatedOn { get; set; }

    [DataMember]
    public DateTimeOffset? LastModified { get; set; }

    [DataMember]
    public WellnessPlanRampFactor RampFactor { get; set; }

    [DataMember]
    public WellnessPlanStatus Status { get; set; }

    [DataMember]
    public bool? HasBiometricDataBeenApplied { get; set; }

    [DataMember]
    public IList<WellnessPlanHabit> Habits { get; private set; }

    [DataMember]
    public IList<UsersGoal> Goals { get; private set; }
  }
}
