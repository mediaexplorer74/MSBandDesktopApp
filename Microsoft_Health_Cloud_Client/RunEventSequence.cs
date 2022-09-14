// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.RunEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Microsoft.Health.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class RunEventSequence : RouteBasedExerciseEventSequence, IComparable
  {
    [DataMember]
    public int StepsTaken { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed SplitPace { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("StepsTaken", this.StepsTaken);
      JsonAssert.PropertyIsGreaterOrEqualToZero("TotalDistance", this.TotalDistance);
      JsonAssert.PropertyIsGreaterOrEqualToZero("AverageHeartRate", this.AverageHeartRate);
      JsonAssert.PropertyIsGreaterOrEqualToZero("SplitDistance", this.SplitDistance);
    }

    public int CompareTo(object obj) => (int) (this.TotalDistance - ((RouteBasedExerciseEventSequence) obj).TotalDistance).TotalMillimeters;
  }
}
