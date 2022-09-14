// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileCacheInfo
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  internal class WebTileCacheInfo
  {
    private Dictionary<string, WebTileResourceCacheInfo> resourceCacheInfo;
    private Dictionary<string, string> variableMappings;

    [DataMember(Name = "ResourceCacheInfo")]
    public Dictionary<string, WebTileResourceCacheInfo> ResourceCacheInfo
    {
      get => this.resourceCacheInfo;
      set => this.resourceCacheInfo = value;
    }

    [DataMember(EmitDefaultValue = false, Name = "VariableMappings")]
    public Dictionary<string, string> VariableMappings
    {
      get => this.variableMappings;
      set => this.variableMappings = value;
    }

    [DataMember(Name = "LastUpdateError")]
    public bool LastUpdateError { get; set; }
  }
}
