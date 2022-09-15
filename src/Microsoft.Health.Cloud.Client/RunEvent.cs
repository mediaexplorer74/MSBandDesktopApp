// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.RunEvent
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
  public class RunEvent : RouteBasedExerciseEvent
  {
    [DataMember]
    public int StepsTaken { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed Pace { get; set; }

    [DataMember]
    public IList<RunEventSequence> Sequences { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("TotalDistance", this.TotalDistance);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StepsTaken", this.StepsTaken);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Pace", this.Pace);
    }
  }
}
