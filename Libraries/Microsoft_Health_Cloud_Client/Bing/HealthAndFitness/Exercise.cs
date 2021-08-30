// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Exercise
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class Exercise : DeserializableObjectBase
  {
    [DataMember(Name = "diffcltylvl")]
    public IList<string> Level { get; set; }

    [DataMember(Name = "exname")]
    public string Name { get; set; }

    [DataMember(Name = "focus")]
    public IList<string> Focus { get; set; }

    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "no")]
    public string Number { get; set; }

    [DataMember(Name = "repstimes")]
    public string RepTimes { get; set; }

    [DataMember(Name = "rest")]
    public string Rest { get; set; }

    [DataMember(Name = "sets")]
    public string Sets { get; set; }

    [DataMember(Name = "thumbnail")]
    public string Image { get; set; }

    [DataMember(Name = "videoid")]
    public string VideoId { get; set; }

    [DataMember(Name = "repsdur")]
    public string RepDuration { get; set; }

    [DataMember(Name = "kcompletiontype")]
    public CompletionType CompletionType { get; set; }

    [DataMember(Name = "kcompletionvalue")]
    public int CompletionValue { get; set; }

    [DataMember(Name = "kisusecustomaryunits")]
    public bool IsUseCustomaryUnits { get; set; }

    [DataMember(Name = "kshowinterstitial")]
    public bool ShowInterstitial { get; set; }
  }
}
