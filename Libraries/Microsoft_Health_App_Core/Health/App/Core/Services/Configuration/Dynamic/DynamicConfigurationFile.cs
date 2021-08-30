// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationFile
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public sealed class DynamicConfigurationFile
  {
    public static readonly Version ExpectedMinimumVersion = new Version(3, 0);
    private readonly DynamicConfiguration configuration = new DynamicConfiguration();

    [DataMember(Name = "$schema")]
    public Uri Schema { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "version")]
    [JsonConverter(typeof (VersionConverter))]
    public Version Version { get; set; }

    [DataMember(Name = "configuration")]
    public DynamicConfiguration Configuration => this.configuration;
  }
}
