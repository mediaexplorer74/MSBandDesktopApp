// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WellnessPlanActivity
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WellnessPlanActivity
  {
    public WellnessPlanActivity() => this.AllowableTimeRanges = (IList<Microsoft.Health.Cloud.Client.AllowableTimeRanges>) new List<Microsoft.Health.Cloud.Client.AllowableTimeRanges>();

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Owner { get; set; }

    [DataMember]
    public string TemplateId { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public DateTimeOffset? CreatedOn { get; set; }

    [DataMember]
    public DateTimeOffset? LastUpdatedOn { get; set; }

    [DataMember]
    public WellnessPlanActivityType Type { get; set; }

    [DataMember]
    public IList<Microsoft.Health.Cloud.Client.AllowableTimeRanges> AllowableTimeRanges { get; private set; }

    [DataMember]
    public IList<WellnessPlanType> AssociatedPlanTypes { get; set; }

    [DataMember]
    public IList<WellnessPlanActivityLevel> ActivityLevels { get; set; }

    [DataMember]
    public BandSettings BandSettings { get; set; }

    [DataMember]
    public IDictionary<string, ActivityProperty> Properties { get; set; }
  }
}
