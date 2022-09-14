// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserWellnessPlanProgress
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserWellnessPlanProgress : DeserializableObjectBase
  {
    public UserWellnessPlanProgress()
    {
      this.WeeklyActivityProgressSummary = (IList<Microsoft.Health.Cloud.Client.WeeklyActivityProgressSummary>) new List<Microsoft.Health.Cloud.Client.WeeklyActivityProgressSummary>();
      this.WeeklySleepProgressSummary = (IList<Microsoft.Health.Cloud.Client.WeeklySleepProgressSummary>) new List<Microsoft.Health.Cloud.Client.WeeklySleepProgressSummary>();
    }

    [DataMember(Name = "WeeklyActivityProgressSummaryDTO")]
    public IList<Microsoft.Health.Cloud.Client.WeeklyActivityProgressSummary> WeeklyActivityProgressSummary { get; private set; }

    [DataMember(Name = "WeeklySleepProgressSummaryDTO")]
    public IList<Microsoft.Health.Cloud.Client.WeeklySleepProgressSummary> WeeklySleepProgressSummary { get; private set; }

    [DataMember]
    public object NextPage { get; set; }

    [DataMember]
    public int? ItemCount { get; set; }
  }
}
