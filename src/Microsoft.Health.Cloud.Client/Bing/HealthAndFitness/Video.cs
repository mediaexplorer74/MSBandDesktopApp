// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.Video
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Xml.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [XmlRoot("video", Namespace = "urn:schemas-microsoft-com:msnvideo:catalog")]
  public sealed class Video
  {
    [XmlArray("videoFiles", Namespace = "urn:schemas-microsoft-com:msnvideo:catalog")]
    [XmlArrayItem("videoFile", typeof (VideoFile), Namespace = "urn:schemas-microsoft-com:msnvideo:catalog")]
    public VideoFile[] VideoFiles { get; set; }
  }
}
