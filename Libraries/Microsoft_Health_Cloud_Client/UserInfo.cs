// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserInfo
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserInfo : DeserializableObjectBase
  {
    [DataMember]
    public DateTimeOffset CreatedOn { get; set; }

    [DataMember]
    public string EndPoint { get; set; }

    [DataMember]
    public string FusEndPoint { get; set; }

    [DataMember]
    public string SocialServiceEndPoint { get; set; }

    [DataMember]
    public string AuthedHnFEndPoint { get; set; }

    [DataMember]
    public string AuthedHnFQueryParameters { get; set; }

    [DataMember]
    public string OdsUserID { get; set; }

    [DataMember]
    public string LfsUserID { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNullOrWhiteSpace("EndPoint", this.EndPoint);
      JsonAssert.PropertyIsNotNullOrWhiteSpace("LFSUserID", this.LfsUserID);
      JsonAssert.PropertyIsNotNullOrWhiteSpace("ODSUserID", this.OdsUserID);
    }
  }
}
