// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.RunMetricComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (RunDisplayMetricType), typeof (int))]
  public class RunMetricComboBoxIndexConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((RunDisplayMetricType) value, (BandClass) parameter);

    public int Convert(RunDisplayMetricType value, BandClass deviceType)
    {
      if (deviceType == 1)
      {
        switch ((int) value)
        {
          case 1:
            return 2;
          case 2:
            return 1;
          case 3:
            return 4;
          case 4:
            return 3;
          default:
            return 0;
        }
      }
      else
      {
        switch ((int) value)
        {
          case 1:
            return 2;
          case 2:
            return 1;
          case 3:
            return 4;
          case 4:
            return 5;
          case 6:
            return 7;
          case 7:
            return 3;
          case 8:
            return 6;
          default:
            return 0;
        }
      }
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) this.ConvertBack((int) value);
    }

    public RunDisplayMetricType ConvertBack(int value)
    {
      switch (value)
      {
        case 1:
          return (RunDisplayMetricType) 2;
        case 2:
          return (RunDisplayMetricType) 1;
        case 3:
          return (RunDisplayMetricType) 4;
        case 4:
          return (RunDisplayMetricType) 3;
        default:
          return (RunDisplayMetricType) 0;
      }
    }
  }
}
