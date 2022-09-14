// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserProfile
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserProfile : DeserializableObjectBase
  {
    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length StrideLength { get; set; }

    [DataMember]
    public int StridesPerMinute { get; set; }

    [DataMember]
    public int PreferredLocaleName { get; set; }

    [DataMember]
    [JsonConverter(typeof (MsPerKilometerToSpeedConverter))]
    public Speed BestRunPace { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length BestRunDistance { get; set; }

    [DataMember]
    public int StepGoalForTheDay { get; set; }

    [DataMember]
    public int CaloriesGoalForTheDay { get; set; }

    [DataMember]
    public int TimeZoneOffset { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("BestRunDistance", this.BestRunDistance);
      JsonAssert.PropertyIsGreaterOrEqualToZero("BestRunPace", this.BestRunPace);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesGoalForTheDay", this.CaloriesGoalForTheDay);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StepGoalForTheDay", this.StepGoalForTheDay);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StrideLength", this.StrideLength);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StridesPerMinute", this.StridesPerMinute);
      JsonAssert.PropertyIsWithinRange("TimeZoneOffset", this.TimeZoneOffset, -1440, 1440);
    }
  }
}
