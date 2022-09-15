// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.UserInfo
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DesktopSyncApp
{
  [DataContract]
  public class UserInfo
  {
    private static DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(typeof (UserInfo));

    public static UserInfo DeserializeJson(byte[] json) => UserInfo.JsonSerializer.ReadObject((Stream) new MemoryStream(json)) as UserInfo;

    [DataMember(Name = "ODSUserID")]
    public string UserID { get; set; }

    [DataMember(Name = "EndPoint")]
    public string EndPoint { get; set; }

    [DataMember(Name = "FUSEndPoint")]
    public string FUSEndPoint { get; set; }
  }
}
