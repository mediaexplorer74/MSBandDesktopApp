// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.BikeEvent
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
  public class BikeEvent : RouteBasedExerciseEvent
  {
    [DataMember]
    [JsonConverter(typeof (CentimetersPerSecondToSpeedConverter))]
    public Speed AverageSpeed { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersPerSecondToSpeedConverter))]
    public Speed MaxSpeed { get; set; }

    [DataMember]
    public IList<BikeEventSequence> Sequences { get; set; }

    [DataMember]
    public int SplitGroupSize { get; set; }

    [DataMember(Name = "GPSState")]
    public GpsState GpsState { get; set; }
  }
}
