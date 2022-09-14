// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HealthCloudConnectionInfo
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Authentication;
using System;

namespace Microsoft.Health.Cloud.Client
{
  public class HealthCloudConnectionInfo
  {
    private string podSecurityToken;

    public Uri BaseUri { get; set; }

    public string SecurityToken { get; set; }

    public Uri PodEndpoint { get; set; }

    public Uri FusEndpoint { get; set; }

    public Uri SocialServiceEndPoint { get; set; }

    public Uri HnFEndpoint { get; set; }

    public string HnFQueryParameters { get; set; }

    public string PodSecurityToken
    {
      get => this.podSecurityToken;
      set
      {
        this.PodSecurityTokenObject = value != null ? new SimpleWebToken(value) : (SimpleWebToken) null;
        this.podSecurityToken = value;
      }
    }

    internal SimpleWebToken PodSecurityTokenObject { get; private set; }

    public string UserId { get; set; }
  }
}
