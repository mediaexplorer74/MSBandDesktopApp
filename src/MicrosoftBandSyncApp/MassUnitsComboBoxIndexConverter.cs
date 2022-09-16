// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.MassUnitsComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (MassUnitType), typeof (int))]
  public class MassUnitsComboBoxIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((MassUnitType) value);

    public int Convert(MassUnitType value) => (int)value == 1 || (int)value != 2 ? 0 : 1;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((int) value);
    }

    public MassUnitType ConvertBack(int value) => value == 0 || value != 1 ? (MassUnitType) 1 : (MassUnitType) 2;
  }
}
