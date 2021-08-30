// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.RaisedInsight
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class RaisedInsight : DeserializableObjectBase
  {
    public RaisedInsight()
    {
      this.DataUsedPivot = (IList<InsightDataUsedPivot>) new List<InsightDataUsedPivot>();
      this.CategoryPivot = (IList<InsightCategoryPivot>) new List<InsightCategoryPivot>();
    }

    [DataMember]
    public string RaisedInsightId { get; set; }

    [DataMember]
    public string InsightId { get; set; }

    [DataMember]
    public bool Expired { get; set; }

    [DataMember]
    public bool Acknowledged { get; set; }

    [DataMember]
    public DateTimeOffset EffectiveDT { get; set; }

    [DataMember]
    public DateTimeOffset? ExpirationDT { get; set; }

    [DataMember]
    public string IM_Msg { get; set; }

    [DataMember]
    public string IM_Action_Msg { get; set; }

    [DataMember]
    public string IM_Help { get; set; }

    [DataMember]
    public string DeviceLine1_Msg { get; set; }

    [DataMember]
    public string DeviceLine1_Help { get; set; }

    [DataMember]
    public string DeviceLine2_Msg { get; set; }

    [DataMember]
    public string DeviceLine2_Help { get; set; }

    [DataMember]
    public IList<InsightDataUsedPivot> DataUsedPivot { get; private set; }

    [DataMember]
    public InsightComparisonPivot ComparisonPivot { get; set; }

    [DataMember]
    public IList<InsightCategoryPivot> CategoryPivot { get; private set; }

    [DataMember]
    public InsightTimespanPivot TimespanPivot { get; set; }

    [DataMember]
    public InsightTonePivot TonePivot { get; set; }

    [DataMember]
    public InsightScopePivot ScopePivot { get; set; }

    [DataMember]
    public bool NoteworthyPivot { get; set; }

    [DataMember]
    public IList<Microsoft.Health.Cloud.Client.InsightEvidence> InsightEvidence { get; set; }

    protected override void Validate() => base.Validate();
  }
}
