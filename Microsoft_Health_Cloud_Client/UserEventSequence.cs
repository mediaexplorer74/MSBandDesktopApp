// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserEventSequence : DeserializableObjectBase
  {
    [DataMember]
    public long SequenceId { get; set; }

    [DataMember]
    public DateTimeOffset StartTime { get; set; }

    [DataMember]
    public EventSequenceType SequenceType { get; set; }

    [DataMember]
    public int Order { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan Duration { get; set; }

    [DataMember]
    public int CaloriesBurned { get; set; }

    [DataMember]
    public Guid LocationBlob { get; set; }

    [DataMember]
    public int AverageHeartRate { get; set; }

    [DataMember]
    public int LowestHeartRate { get; set; }

    [DataMember]
    public int PeakHeartRate { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterThanZero("SequenceId", this.SequenceId);
      JsonAssert.DateTimePropertyIsSet("StartTime", this.StartTime);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Duration", this.Duration);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesBurned", this.CaloriesBurned);
    }
  }
}
