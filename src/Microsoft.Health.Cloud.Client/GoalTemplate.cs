// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.GoalTemplate
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class GoalTemplate : DeserializableObjectBase
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public GoalCategory Category { get; set; }

    [DataMember]
    public GoalType Type { get; set; }

    [DataMember]
    public GoalCheckIn CheckInCadence { get; set; }

    [DataMember]
    public IList<GoalValueTemplate> GoalValueTemplates { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNullOrWhiteSpace("Id", this.Id);
      JsonAssert.PropertyIsNotNullOrWhiteSpace("Name", this.Name);
      JsonAssert.IsTrue(this.GoalValueTemplates != null, "GoalValueTemplates Cannot be null");
    }
  }
}
