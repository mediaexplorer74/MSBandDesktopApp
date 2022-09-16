// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.DisplayUnitsToHeightLowValueVisibiliyConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (byte), typeof (Visibility))]
  public class DisplayUnitsToHeightLowValueVisibiliyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((DistanceUnitType) value);

        public Visibility Convert(DistanceUnitType value)
        {
            return (int)value != 1 ? Visibility.Collapsed : Visibility.Visible;
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
