// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserWellnessPlansResponse
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserWellnessPlansResponse
  {
    public UserWellnessPlansResponse() => this.UserWellnessPlans = (IList<UserWellnessPlan>) new List<UserWellnessPlan>();

    [DataMember]
    public IList<UserWellnessPlan> UserWellnessPlans { get; private set; }

    [DataMember]
    public object NextPage { get; set; }

    [DataMember]
    public int ItemCount { get; set; }
  }
}
