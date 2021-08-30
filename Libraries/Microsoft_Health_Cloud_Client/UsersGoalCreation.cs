// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UsersGoalCreation
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UsersGoalCreation
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string TemplateId { get; set; }

    [DataMember]
    public DateTimeOffset StartTime { get; set; }

    [DataMember]
    public DateTimeOffset EndTime { get; set; }

    [DataMember]
    public IList<GoalValueHistory> ValueHistory { get; set; }
  }
}
