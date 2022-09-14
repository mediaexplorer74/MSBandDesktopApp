// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.Clients.LiveLogin.TokenRefreshResponse
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Http.Clients.LiveLogin
{
  [DataContract]
  public class TokenRefreshResponse
  {
    [DataMember(Name = "access_token")]
    public string AccessToken { get; set; }

    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; set; }

    [DataMember(Name = "expires_in")]
    public int ExpiresInSeconds { get; set; }
  }
}
