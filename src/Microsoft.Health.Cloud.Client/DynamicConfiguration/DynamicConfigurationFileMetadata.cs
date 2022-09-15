// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DynamicConfiguration.DynamicConfigurationFileMetadata
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.DynamicConfiguration
{
  [DataContract]
  public sealed class DynamicConfigurationFileMetadata
  {
    [DataMember]
    public string Application { get; set; }

    [DataMember]
    [JsonConverter(typeof (VersionConverter))]
    public Version Version { get; set; }

    [DataMember]
    public string Locale { get; set; }

    [DataMember(Name = "PrimaryURL")]
    public Uri PrimaryUrl { get; set; }

    [DataMember(Name = "MirrorURL")]
    public Uri MirrorUrl { get; set; }

    [DataMember]
    public long SizeInBytes { get; set; }

    [DataMember]
    public string HashMd5 { get; set; }
  }
}
