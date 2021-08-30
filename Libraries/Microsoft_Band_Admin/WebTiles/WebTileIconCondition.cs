// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileIconCondition
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  public class WebTileIconCondition
  {
    private string condition;
    private string iconName;

    [DataMember(IsRequired = true, Name = "condition")]
    public string Condition
    {
      get => this.condition;
      set => this.condition = value;
    }

    [DataMember(IsRequired = true, Name = "icon")]
    public string IconName
    {
      get => this.iconName;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.iconName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(CommonSR.WTIconNameCannotBeEmpty, nameof (value));
      }
    }
  }
}
