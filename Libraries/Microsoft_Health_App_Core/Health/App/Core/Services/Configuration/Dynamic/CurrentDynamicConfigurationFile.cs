// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.CurrentDynamicConfigurationFile
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public sealed class CurrentDynamicConfigurationFile
  {
    public static readonly Uri CurrentDynamicConfigurationFileSchema = new Uri("https://schemas.microsoft.com/health/application/configuration/current/2015/08");
    private readonly DynamicConfigurationFile file;

    public CurrentDynamicConfigurationFile()
      : this(DateTimeOffset.MinValue, ConfigurationFileSource.Unknown, (string) null, (DynamicConfigurationFile) null)
    {
    }

    public CurrentDynamicConfigurationFile(
      DateTimeOffset timestamp,
      ConfigurationFileSource source,
      string regionName,
      DynamicConfigurationFile file)
    {
      this.Schema = CurrentDynamicConfigurationFile.CurrentDynamicConfigurationFileSchema;
      this.Timestamp = timestamp;
      this.Source = source;
      this.RegionName = regionName;
      this.file = file ?? new DynamicConfigurationFile();
    }

    [DataMember(Name = "$schema")]
    public Uri Schema { get; set; }

    [DataMember(Name = "timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [DataMember(Name = "source")]
    public ConfigurationFileSource Source { get; set; }

    [DataMember(Name = "regionName")]
    public string RegionName { get; set; }

    [DataMember(Name = "file")]
    public DynamicConfigurationFile File => this.file;
  }
}
