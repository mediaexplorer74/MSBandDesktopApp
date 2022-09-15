// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.TemperatureUnitsComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (TemperatureUnitType), typeof (int))]
  public class TemperatureUnitsComboBoxIndexConverter : IValueConverter
  {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (object)this.Convert((TemperatureUnitType)value);
        }

        public int Convert(TemperatureUnitType value)
        {
            return (int)value == 1 || (int)value != 2 ? 0 : 1;
        }

        public object ConvertBack
        (
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture
        )
        {
          return (object) this.ConvertBack((int) value);
        }

        public TemperatureUnitType ConvertBack(int value)
        {
            return value == 0 || value != 1 ? (TemperatureUnitType)1 : (TemperatureUnitType)2;
        }
    }
}
