// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  [JsonConverter(typeof (UserEventConverter))]
  public class UserEvent : DeserializableObjectBase
  {
    public UserEvent() => this.Evidences = (IList<Evidence>) new List<Evidence>();

    [DataMember]
    public EventType EventType { get; set; }

    [DataMember]
    public string EventId { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan Duration { get; set; }

    [DataMember]
    public string ParentEventId { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int DeliveryId { get; set; }

    [DataMember]
    public DateTimeOffset StartTime { get; set; }

    [DataMember]
    public int CaloriesBurned { get; set; }

    [DataMember]
    public DateTimeOffset DayId { get; set; }

    [DataMember]
    public string Feeling { get; set; }

    [DataMember]
    public int AverageHeartRate { get; set; }

    [DataMember]
    public int LowestHeartRate { get; set; }

    [DataMember]
    public int PeakHeartRate { get; set; }

    [DataMember]
    public int Flags { get; set; }

    [DataMember]
    public int UvExposure { get; set; }

    [DataMember]
    public IList<UserActivity> Info { get; set; }

    [DataMember]
    public IList<Evidence> Evidences { get; private set; }

    [DataMember]
    public int CardioScore { get; set; }

    [DataMember]
    public int IntenseCardioSeconds { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesBurned", this.CaloriesBurned);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Duration", this.Duration);
      JsonAssert.PropertyIsGreaterOrEqualToZero("UvExposure", this.UvExposure);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CardioScore", this.CardioScore);
      JsonAssert.PropertyIsGreaterOrEqualToZero("IntenseCardioSeconds", this.IntenseCardioSeconds);
      JsonAssert.PropertyIsNotNull("EventId", (object) this.EventId);
      JsonAssert.DateTimePropertyIsSet("StartTime", this.StartTime);
    }

    public virtual bool IsComplete() => true;
  }
}
