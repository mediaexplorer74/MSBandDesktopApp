// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileNotification
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  public class WebTileNotification
  {
    private string condition;
    private string title;
    private string body;
    private WebTilePropertyValidator validator = new WebTilePropertyValidator();

    public bool AllowInvalidValues
    {
      get => this.validator.AllowInvalidValues;
      set => this.validator.AllowInvalidValues = value;
    }

    public Dictionary<string, string> PropertyErrors => this.validator.PropertyErrors;

    [DataMember(IsRequired = true, Name = "condition")]
    public string Condition
    {
      get => this.condition;
      set
      {
        this.validator.ClearPropertyError(nameof (Condition));
        WebTileCondition webTileCondition = new WebTileCondition((Dictionary<string, string>) null);
        try
        {
          webTileCondition.ComputeValue(value);
        }
        catch (InvalidDataException ex)
        {
          this.validator.CheckProperty(nameof (Condition), false, ex.Message);
        }
        this.condition = value;
      }
    }

    [DataMember(IsRequired = true, Name = "title")]
    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    [DataMember(Name = "body")]
    public string Body
    {
      get => this.body;
      set => this.body = value;
    }
  }
}
