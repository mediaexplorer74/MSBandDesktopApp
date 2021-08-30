// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.GolfEventHoleSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Events.Golf
{
  [DataContract]
  public sealed class GolfEventHoleSequence : UserEventSequence
  {
    [DataMember]
    public int HoleNumber { get; set; }

    [DataMember(Name = "DistanceToPinInCm")]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length DistanceToPin { get; set; }

    [DataMember(Name = "DistanceWalkedInCm")]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length DistanceWalked { get; set; }

    [DataMember]
    public int HolePar { get; set; }

    [DataMember]
    public int HoleDifficultyIndex { get; set; }

    [DataMember]
    public int UserScore { get; set; }

    [DataMember]
    public string HoleShotOverlayImageUrl { get; set; }

    [DataMember]
    public int StepCount { get; set; }
  }
}
