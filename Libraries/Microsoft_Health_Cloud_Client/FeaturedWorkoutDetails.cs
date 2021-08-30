// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.FeaturedWorkoutDetails
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class FeaturedWorkoutDetails
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "path")]
    public string Image { get; set; }

    [DataMember(Name = "lvl")]
    public string Level { get; set; }

    [DataMember(Name = "dur")]
    public string Duration { get; set; }
  }
}
