// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HikeEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Microsoft.Health.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class HikeEvent : RouteBasedExerciseEvent
  {
    [DataMember(Name = "ClimbRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed ClimbSpeed { get; set; }

    [DataMember(Name = "MaxClimbRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed MaxClimbSpeed { get; set; }

    [DataMember(Name = "AscentRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed AscentSpeed { get; set; }

    [DataMember(Name = "MaxAscentRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed MaxAscentSpeed { get; set; }

    [DataMember(Name = "DescentRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed DescentSpeed { get; set; }

    [DataMember(Name = "MaxDescentRateInCMPerHour")]
    [JsonConverter(typeof (CentimetersPerHourToSpeedConverter))]
    public Speed MaxDescentSpeed { get; set; }

    [DataMember(Name = "RestingTimeInSeconds")]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan RestingTime { get; set; }

    [DataMember(Name = "ActiveTimeInSeconds")]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan ActiveTime { get; set; }

    [DataMember]
    public IList<HikeEventSequence> Sequences { get; set; }

    [DataMember(Name = "GPSState")]
    public GpsState GpsState { get; set; }
  }
}
