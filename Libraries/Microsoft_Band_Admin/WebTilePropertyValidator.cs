// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTilePropertyValidator
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  public class WebTilePropertyValidator
  {
    private Dictionary<string, string> propertyErrors;

    public WebTilePropertyValidator() => this.propertyErrors = new Dictionary<string, string>();

    public bool AllowInvalidValues { get; set; }

    public Dictionary<string, string> PropertyErrors => this.propertyErrors;

    public void ClearPropertyError(string propertyName) => this.propertyErrors.Remove(propertyName);

    public void CheckProperty(string propertyName, bool valid, string errorString)
    {
      this.ClearPropertyError(propertyName);
      if (valid)
        return;
      this.propertyErrors[propertyName] = errorString;
      if (!this.AllowInvalidValues)
        throw new WebTileException(string.Format(CommonSR.WTPropertyError, new object[2]
        {
          (object) propertyName,
          (object) errorString
        }));
    }

    public void SetProperty<T>(
      ref T storage,
      T value,
      string propertyName,
      bool valid,
      string errorString)
    {
      this.CheckProperty(propertyName, valid, errorString);
      storage = value;
    }

    public void SetUintProperty(
      ref uint storage,
      uint value,
      string propertyName,
      uint minValue,
      uint maxValue)
    {
      this.SetProperty<uint>(ref storage, value, propertyName, (minValue > value ? 0 : (value <= maxValue ? 1 : 0)) != 0, string.Format(CommonSR.WTPropertyUintRangeError, new object[2]
      {
        (object) minValue,
        (object) maxValue
      }));
    }

    public void SetStringProperty(
      ref string storage,
      string value,
      string propertyName,
      int minLength,
      int maxLength)
    {
      if (value == null)
        storage = value;
      else
        this.SetProperty<string>(ref storage, value, propertyName, (minLength > value.Length ? 0 : (value.Length <= maxLength ? 1 : 0)) != 0, string.Format(CommonSR.WTPropertStringLengthError, new object[2]
        {
          (object) minLength,
          (object) maxLength
        }));
    }
  }
}
