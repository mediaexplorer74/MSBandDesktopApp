// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.VideoFile
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  public sealed class VideoFile
  {
    [XmlAttribute("formatCode")]
    public int FormatCode { get; set; }

    [XmlAttribute("height")]
    public int Height { get; set; }

    [XmlElement("uri", Namespace = "urn:schemas-microsoft-com:msnvideo:catalog")]
    public string Url { get; set; }

    [XmlAttribute("width")]
    public int Width { get; set; }

    public static class FormatCodes
    {
      public static readonly IList<int> SupportedTypes = (IList<int>) new List<int>()
      {
        101,
        102,
        103,
        104,
        1002,
        1003
      };
      private const int H264Low = 101;
      private const int H264SD = 102;
      private const int H264HQ = 103;
      private const int H264HD = 104;
      private const int PartnerMezzanine1 = 1002;
      private const int PartnerMezzanine2 = 1003;
    }
  }
}
