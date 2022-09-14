// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DailySleepProgressSummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class DailySleepProgressSummary : DeserializableObjectBase
  {
    [DataMember]
    public DateTimeOffset TimeOfDay { get; set; }

    [DataMember]
    public DateTimeOffset NextTimeOfDay { get; set; }

    [DataMember]
    public int? ActualSleepMinutes { get; set; }

    [DataMember]
    public int? SleepMinutesTarget { get; set; }

    [DataMember]
    public int? SleepEfficiencyPercentage { get; set; }

    [DataMember]
    public int? OptimalSleepEfficiencyPercentage { get; set; }

    [DataMember]
    public int? SleepConsistencyPercentage { get; set; }

    [DataMember]
    public int? SleepRestoration { get; set; }

    [DataMember]
    public bool HasReportedSleep { get; set; }
  }
}
