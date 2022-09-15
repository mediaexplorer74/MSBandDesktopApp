// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.TemperatureUnitsConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (TemperatureUnitType), typeof (string))]
  public class TemperatureUnitsConverter : IValueConverter
  {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (object)this.Convert((TemperatureUnitType)value);
        }

        public string Convert(TemperatureUnitType value)
        {
            return (int)value == 1 || (int)value != 2 
                ? Strings.Label_UoM_Fahrenheit 
                : Strings.Label_UoM_Celsius;
        }

        public object ConvertBack
        (
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture
        )
        {
          throw new InvalidOperationException();
        }
  }
}
