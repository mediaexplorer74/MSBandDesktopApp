// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.GolfEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Events.Golf
{
  [DataContract]
  public sealed class GolfEvent : UserEvent
  {
    public GolfEvent() => this.Sequences = (IList<GolfEventHoleSequence>) new List<GolfEventHoleSequence>();

    [DataMember]
    public string CourseID { get; set; }

    [DataMember]
    public string CourseMapVersion { get; set; }

    [DataMember]
    public string CourseName { get; set; }

    [DataMember]
    public int CoursePar { get; set; }

    [DataMember]
    public int ParForHolesPlayed { get; set; }

    [DataMember]
    public string TeeNameSelected { get; set; }

    [DataMember]
    public int TotalHolesPlayed { get; set; }

    [DataMember]
    public int TotalScore { get; set; }

    [DataMember]
    public int ParOrBetterCount { get; set; }

    [DataMember(Name = "LongestDriveInCm")]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length LongestDrive { get; set; }

    [DataMember(Name = "PaceOfPlayInSeconds")]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan PaceOfPlay { get; set; }

    [DataMember]
    public int TotalStepCount { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistanceWalkedInCm { get; set; }

    [DataMember(Name = "GPSState")]
    public int GpsState { get; set; }

    [DataMember]
    public IList<GolfEventHoleSequence> Sequences { get; private set; }
  }
}
