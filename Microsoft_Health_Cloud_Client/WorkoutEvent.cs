// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WorkoutEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WorkoutEvent : ExerciseEventBase
  {
    [DataMember]
    public string WorkoutPlanId { get; set; }

    [DataMember]
    public int WorkoutIndex { get; set; }

    [DataMember]
    public int WorkoutWeekId { get; set; }

    [DataMember]
    public int WorkoutDayId { get; set; }

    [DataMember]
    public int WorkoutPlanInstanceId { get; set; }

    [DataMember]
    public CompletionType CompletionType { get; set; }

    [DataMember(Name = "KDisplaySubType")]
    public DisplaySubType DisplaySubType { get; set; }

    [DataMember]
    public int CompletionValue { get; set; }

    [DataMember]
    public int CyclesPerformed { get; set; }

    [DataMember]
    public int RoundsPerformed { get; set; }

    [DataMember]
    public int RepetitionsPerformed { get; set; }

    [DataMember(Name = "KIsSupportedCounting")]
    public bool IsSupportedCounting { get; set; }

    [DataMember]
    public int TotalSteps { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistanceInCM { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed AvgPace { get; set; }

    [DataMember]
    public IList<WorkoutEventSequence> Sequences { get; set; }
  }
}
