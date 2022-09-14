// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.MapPoint
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Microsoft.Health.Cloud.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class MapPoint : DeserializableObjectBase
  {
    [DataMember]
    public int HeartRate { get; set; }

    [DataMember]
    public bool IsPaused { get; set; }

    [DataMember]
    public bool IsResume { get; set; }

    [DataMember]
    public Location Location { get; set; }

    [DataMember]
    public Elevation Elevation { get; set; }

    [DataMember]
    public int MapPointOrdinal { get; set; }

    [DataMember]
    [JsonConverter(typeof (StringEnumConverter))]
    public MapPointType MapPointType { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed Pace { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersPerSecondToSpeedConverter))]
    public Speed Speed { get; set; }

    [DataMember]
    public int ScaledPace { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan SecondsSinceStart { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistance { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length ActualDistance { get; set; }

    [DataMember]
    public int SplitOrdinal { get; set; }

    [DataMember(Name = "POIType")]
    [JsonConverter(typeof (StringEnumConverter))]
    public PointOfInterestType PoiType { get; set; }

    [DataMember(Name = "UserPOIOrdinal")]
    public int UserPoiOrdinal { get; set; }

    protected override void Validate() => base.Validate();
  }
}
