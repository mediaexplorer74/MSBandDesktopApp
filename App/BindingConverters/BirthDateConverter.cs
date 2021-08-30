// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.BirthDateConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (DateTime?), typeof (string))]
  public class BirthDateConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((DateTime?) value);

    public string Convert(DateTime? value)
    {
      string empty = string.Empty;
      if (value.HasValue)
        empty = value.Value.ToString("MMMM, yyyy");
      return empty;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }
  }
}
