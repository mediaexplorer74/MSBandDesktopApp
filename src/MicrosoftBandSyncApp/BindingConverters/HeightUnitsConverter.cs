// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.HeightUnitsConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (DistanceUnitType), typeof (string))]
  public class HeightUnitsConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((DistanceUnitType) value, (HeightUnitsConverter.UnitType) parameter);

    public string Convert(DistanceUnitType value, HeightUnitsConverter.UnitType type)
    {
      if (type != HeightUnitsConverter.UnitType.HeightHigh && type == HeightUnitsConverter.UnitType.HeightLow)
        return LStrings.UoM_Profile_Inches;
      return (int)value == 1 || (int)value != 2 
                ? LStrings.UoM_Profile_Feet 
                : LStrings.UoM_Profile_Centimeters;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }

    public enum UnitType
    {
      HeightHigh,
      HeightLow,
    }
  }
}
