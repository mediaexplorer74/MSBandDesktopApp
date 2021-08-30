// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HRZones
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class HRZones : DeserializableObjectBase
  {
    [DataMember]
    public int Under { get; set; }

    [DataMember]
    public int Aerobic { get; set; }

    [DataMember]
    public int Anaerobic { get; set; }

    [DataMember]
    public int FitnessZone { get; set; }

    [DataMember]
    public int HealthyHeart { get; set; }

    [DataMember]
    public int RedLine { get; set; }

    [DataMember]
    public int Over { get; set; }

    [DataMember]
    public int? MaxHR { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("Under", this.Under);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Aerobic", this.Aerobic);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Anaerobic", this.Anaerobic);
      JsonAssert.PropertyIsGreaterOrEqualToZero("FitnessZone", this.FitnessZone);
      JsonAssert.PropertyIsGreaterOrEqualToZero("HealthyHeart", this.HealthyHeart);
      JsonAssert.PropertyIsGreaterOrEqualToZero("RedLine", this.RedLine);
      JsonAssert.PropertyIsGreaterOrEqualToZero("Over", this.Over);
    }
  }
}
