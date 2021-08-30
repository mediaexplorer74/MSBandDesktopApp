// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.RouteBasedExerciseEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Microsoft.Health.Cloud.Client.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class RouteBasedExerciseEvent : ExerciseEventBase
  {
    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistance { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length ActualDistance { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length SplitDistance { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length WayPointDistance { get; set; }

    [DataMember]
    public MapPointElevationSource MapPointElevationSource { get; set; }

    [DataMember]
    public IList<MapPoint> MapPoints { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalAltitudeGain { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalAltitudeLoss { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length MinAltitude { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length MaxAltitude { get; set; }

    [DataMember(Name = "IsLowPowerGPS")]
    public bool IsLowPowerGps { get; set; }
  }
}
