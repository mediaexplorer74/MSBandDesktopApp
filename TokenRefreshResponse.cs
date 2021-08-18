// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.TokenRefreshResponse
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DesktopSyncApp
{
  [DataContract]
  public class TokenRefreshResponse
  {
    private static readonly DataContractJsonSerializer JsonObjectSerializer = new DataContractJsonSerializer(typeof (TokenRefreshResponse));

    public static TokenRefreshResponse DeserializeJsonObject(byte[] json) => TokenRefreshResponse.JsonObjectSerializer.ReadObject((Stream) new MemoryStream(json)) as TokenRefreshResponse;

    [DataMember(Name = "access_token")]
    public string AccessToken { get; private set; }

    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; private set; }

    [DataMember(Name = "expires_in")]
    public int ExpiresIn { get; private set; }
  }
}
