// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WorkoutEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WorkoutEventSequence : ExerciseEventSequence
  {
    [DataMember]
    public double StressBalance { get; set; }

    [DataMember]
    public double MaximalV02 { get; set; }

    [DataMember]
    public string AnalysisStatus { get; set; }

    [DataMember]
    public int CycleOrdinal { get; set; }

    [DataMember]
    public int CircuitOrdinal { get; set; }

    [DataMember]
    public int RoundOrdinal { get; set; }

    [DataMember]
    public int SetOrdinal { get; set; }

    [DataMember]
    public int ExerciseOrdinal { get; set; }

    [DataMember]
    public int ExercisePosition { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string ExerciseStringID { get; set; }

    [DataMember]
    public int ExerciseID { get; set; }

    [DataMember]
    public int CircuitID { get; set; }

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
    public int WorkoutPartnerId { get; set; }

    [DataMember]
    public bool DoNotCount { get; set; }

    [DataMember]
    public bool UseAccelerometer { get; set; }

    [DataMember(Name = "UseGPS")]
    public bool UseGps { get; set; }

    [DataMember]
    public bool UseWeight { get; set; }

    [DataMember]
    public bool UseCustomaryUnits { get; set; }

    [DataMember]
    public int CountableID { get; set; }

    [DataMember]
    public int Weight { get; set; }

    [DataMember]
    public string ExerciseType { get; set; }

    [DataMember]
    public string ExerciseCategory { get; set; }

    [DataMember]
    public CompletionType CompletionType { get; set; }

    [DataMember]
    public int CompletionValue { get; set; }

    [DataMember]
    public int ComputedCompletionValue { get; set; }

    [DataMember]
    public int WeightUsed { get; set; }

    [DataMember]
    public int UserCompletionValue { get; set; }

    [DataMember]
    public CircuitType CircuitType { get; set; }

    [DataMember]
    public CircuitGroupType CircuitGroupType { get; set; }

    [DataMember]
    public string ExerciseFinishStatus { get; set; }

    [DataMember]
    public bool IsRest { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length DistanceInCM { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed Pace { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("CycleOrdinal", this.CycleOrdinal);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CircuitOrdinal", this.CircuitOrdinal);
      JsonAssert.PropertyIsGreaterOrEqualToZero("RoundOrdinal", this.RoundOrdinal);
      JsonAssert.PropertyIsGreaterOrEqualToZero("ExerciseOrdinal", this.ExerciseOrdinal);
    }
  }
}
