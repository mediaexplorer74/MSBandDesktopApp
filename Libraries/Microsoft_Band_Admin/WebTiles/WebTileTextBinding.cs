// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileTextBinding
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  public class WebTileTextBinding
  {
    private short elementId;
    private string textValue;
    private WebTilePropertyValidator validator = new WebTilePropertyValidator();

    public WebTilePropertyValidator Validator => this.validator;

    public bool AllowInvalidValues
    {
      get => this.validator.AllowInvalidValues;
      set => this.validator.AllowInvalidValues = value;
    }

    public Dictionary<string, string> PropertyErrors => this.validator.PropertyErrors;

    [DataMember(IsRequired = true, Name = "elementId")]
    public short ElementId
    {
      get => this.elementId;
      set => this.elementId = value;
    }

    [DataMember(IsRequired = true, Name = "value")]
    public string TextValue
    {
      get => this.textValue;
      set => this.textValue = string.IsNullOrWhiteSpace(value) ? "" : value;
    }
  }
}
