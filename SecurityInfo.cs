// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.SecurityInfo
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DesktopSyncApp
{
  [DataContract]
  public class SecurityInfo
  {
    private static readonly DataContractJsonSerializer JsonObjectSerializer = new DataContractJsonSerializer(typeof (SecurityInfo));

    public static SecurityInfo DeserializeJsonObject(byte[] json) => SecurityInfo.JsonObjectSerializer.ReadObject((Stream) new MemoryStream(json)) as SecurityInfo;

    public SecurityInfo(string securityEnvironment) => this.SecurityEnvironment = securityEnvironment != null ? securityEnvironment : throw new ArgumentNullException(nameof (securityEnvironment));

    [DataMember(Name = "SecurityEnvironment")]
    public string SecurityEnvironment { get; private set; }

    [DataMember(Name = "AccessToken")]
    public string AccessToken { get; private set; }

    [DataMember(Name = "AccessTokenExpires")]
    public DateTime? AccessTokenExpires { get; private set; }

    [DataMember(Name = "RefreshToken")]
    public string RefreshToken { get; private set; }

    [DataMember(Name = "RefreshTokenExpires")]
    public DateTime? RefreshTokenExpires { get; private set; }

    [DataMember(Name = "PodAddress")]
    public string PodAddress { get; private set; }

    [DataMember(Name = "KATToken")]
    public string KATToken { get; private set; }

    [DataMember(Name = "KATTokenExpires")]
    public DateTime? KATTokenExpires { get; private set; }

    public Uri CreateLoginUri() => new Uri(string.Format(Globals.LoginUrlFormat, (object) this.SecurityEnvironment));

    public Uri CreateTokenRefreshUri()
    {
      if (this.RefreshToken == null)
        throw new Exception("No refresh token present");
      return new Uri(string.Format(Globals.TokenRefreshUrlFormat, (object) this.SecurityEnvironment, (object) this.RefreshToken));
    }

    public Uri CreateLogoutUri() => new Uri(Globals.LogoutUrl);

    public byte[] SerializeJsonObject()
    {
      MemoryStream memoryStream = new MemoryStream();
      SecurityInfo.JsonObjectSerializer.WriteObject((Stream) memoryStream, (object) this);
      return memoryStream.ToArray();
    }

    public void Update(LoginResponse response)
    {
      DateTime utcNow = DateTime.UtcNow;
      this.AccessToken = response.AccessToken;
      this.AccessTokenExpires = new DateTime?(utcNow.AddSeconds((double) response.ExpiresIn));
      this.RefreshToken = response.RefreshToken;
      this.RefreshTokenExpires = new DateTime?(utcNow.Add(Globals.RefreshTokenTimeToLive));
    }

    public void Update(TokenRefreshResponse response)
    {
      DateTime utcNow = DateTime.UtcNow;
      this.AccessToken = response.AccessToken;
      this.AccessTokenExpires = new DateTime?(utcNow.AddSeconds((double) response.ExpiresIn));
      this.RefreshToken = response.RefreshToken;
      this.RefreshTokenExpires = new DateTime?(utcNow.Add(Globals.RefreshTokenTimeToLive));
    }

    public void Update(string podAddress, string katToken)
    {
      DateTime utcNow = DateTime.UtcNow;
      this.PodAddress = podAddress;
      this.KATToken = katToken;
      this.KATTokenExpires = new DateTime?(utcNow.Add(Globals.KATTokenTimeToLive));
    }

    public void Clear()
    {
      this.AccessToken = (string) null;
      this.AccessTokenExpires = new DateTime?();
      this.RefreshToken = (string) null;
      this.RefreshTokenExpires = new DateTime?();
      this.KATToken = (string) null;
      this.KATTokenExpires = new DateTime?();
    }
  }
}
