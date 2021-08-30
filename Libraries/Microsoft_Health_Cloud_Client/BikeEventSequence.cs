// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.BikeEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Microsoft.Health.Cloud.Client.Models;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class BikeEventSequence : RouteBasedExerciseEventSequence
  {
    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length AltitudeGain { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length AltitudeLoss { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersPerSecondToSpeedConverter))]
    public Speed SplitSpeed { get; set; }
  }
}
