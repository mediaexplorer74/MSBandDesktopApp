// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.MonthComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (int), typeof (int))]
  public class MonthComboBoxIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((int) value);

    public int Convert(int value) => Math2.Between<int>(value, 1, 12) - 1;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((int) value);
    }

    public int ConvertBack(int value) => Math2.Between<int>(value, 0, 11) + 1;
  }
}
