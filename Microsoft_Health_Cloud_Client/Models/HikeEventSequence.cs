// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Models.HikeEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Models
{
  [DataContract]
  public class HikeEventSequence : RouteBasedExerciseEventSequence
  {
    [DataMember(Name = "POIType")]
    public PointOfInterestType PoiType { get; set; }

    [DataMember(Name = "UserPOIOrdinal")]
    public int UserPoiOrdinal { get; set; }

    [DataMember]
    public Location Location { get; set; }

    [DataMember]
    public Elevation Elevation { get; set; }

    [DataMember(Name = "ClimbRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed ClimbSpeed { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length AltitudeGain { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length AltitudeLoss { get; set; }
  }
}
