// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Models.Social.SocialTileStatusResponse
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Models.Social
{
  [DataContract]
  public class SocialTileStatusResponse
  {
    [DataMember(IsRequired = true, Name = "status")]
    public SocialTileStatus Status { get; set; }

    [DataMember(IsRequired = true, Name = "line1")]
    public string Line1 { get; set; }

    [DataMember(IsRequired = true, Name = "line2")]
    public string Line2 { get; set; }

    [DataMember(IsRequired = true, Name = "numericValue")]
    public double NumericValue { get; set; }

    [DataMember(IsRequired = true, Name = "relativeUrl")]
    public string RelativeUrl { get; private set; }
  }
}
