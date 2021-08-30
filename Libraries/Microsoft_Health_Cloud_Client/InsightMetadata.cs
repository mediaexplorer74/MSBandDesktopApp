// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.InsightMetadata
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class InsightMetadata : DeserializableObjectBase
  {
    [DataMember]
    public string InsightID { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public bool Enabled { get; set; }

    [DataMember]
    public string ParentInsightID { get; set; }

    [DataMember]
    public string Class { get; set; }

    [DataMember]
    public string Type { get; set; }

    [DataMember]
    public string Importance { get; set; }

    [DataMember]
    public string TimeSensitivity { get; set; }

    [DataMember]
    public bool ForceDeviceNudge { get; set; }

    [DataMember]
    public bool ForceDeviceDisplay { get; set; }

    [DataMember]
    public int Timeframe { get; set; }

    [DataMember]
    public int GroupId { get; set; }

    [DataMember]
    public InsightTracker Tracker { get; set; }

    [DataMember]
    public bool OptOut { get; set; }

    protected override void Validate() => base.Validate();
  }
}
