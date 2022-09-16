// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.WeightUnitsConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (MassUnitType), typeof (string))]
  public class WeightUnitsConverter : IValueConverter
  {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return (object)this.Convert((MassUnitType)value);
        }

        public string Convert(MassUnitType value)
        {
            return (int)value == 1 || (int)value != 2 
                ? LStrings.UoM_Profile_Pounds 
                : LStrings.UoM_Profile_Kilograms;
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
