// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Authentication.RequestSecurityTokenResponse
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Authentication
{
  [DataContract]
  internal class RequestSecurityTokenResponse
  {
    [DataMember]
    public string AppliesTo { get; set; }

    [DataMember]
    public string Context { get; set; }

    [DataMember]
    public long Created { get; set; }

    [DataMember]
    public long Expires { get; set; }

    [DataMember]
    public string SecurityToken { get; set; }

    [DataMember]
    public string TokenType { get; set; }
  }
}
