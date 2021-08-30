// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public sealed class DynamicConfiguration : IDynamicConfiguration
  {
    private readonly OobeConfiguration oobe = new OobeConfiguration();
    private readonly MapsConfiguration maps = new MapsConfiguration();
    private readonly DynamicConfigurationFeatures features = new DynamicConfigurationFeatures();

    [DataMember(Name = "oobe")]
    public OobeConfiguration Oobe => this.oobe;

    [DataMember(Name = "maps")]
    public MapsConfiguration Maps => this.maps;

    [DataMember(Name = "features")]
    public DynamicConfigurationFeatures Features => this.features;

    IOobeConfiguration IDynamicConfiguration.Oobe => (IOobeConfiguration) this.Oobe;

    IMapsConfiguration IDynamicConfiguration.Maps => (IMapsConfiguration) this.Maps;

    IDynamicConfigurationFeatures IDynamicConfiguration.Features => (IDynamicConfigurationFeatures) this.Features;
  }
}
