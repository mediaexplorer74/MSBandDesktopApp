// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.NullableIntToStringConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (int?), typeof (string))]
  public class NullableIntToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((int?) value);

    public string Convert(int? value) => !value.HasValue ? "" : value.Value.ToString();

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((string) value);
    }

    public int? ConvertBack(string value)
    {
      int result;
      return int.TryParse(value.Trim(), out result) ? new int?(result) : new int?();
    }
  }
}
