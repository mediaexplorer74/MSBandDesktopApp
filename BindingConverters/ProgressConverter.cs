// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.ProgressConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (double?), typeof (int))]
  public class ProgressConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((double?) value);

    public int Convert(double? value) => !value.HasValue ? 0 : System.Convert.ToInt32(this.ForceBetween(Math.Max(value.Value, 0.0), 0.0, 100.0));

    private double ForceBetween(double value, double min, double max)
    {
      if (value < min)
        return min;
      return value > max ? max : value;
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
