// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.BikeMetricComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (BikeDisplayMetricType), typeof (int))]
  public class BikeMetricComboBoxIndexConverter : IValueConverter
  {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (object)this.Convert((BikeDisplayMetricType)value, (BandClass)parameter);
        }

        public int Convert(BikeDisplayMetricType value, BandClass deviceType)
        {
          if ((int)deviceType == 1)
          {
            switch ((int)value - 1)
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
        switch ((int)value - 1)
        {
          case 1:
            return 2;
          case 2:
            return 1;
          case 3:
            return 4;
          case 4:
            return 5;
          case 5:
            return 7;
          case 6:
            return 3;
          case 7:
            return 6;
          default:
            return 0;
        }
      }
    }

    public object ConvertBack
    (
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture
    )
    {
      return (object) this.ConvertBack((int) value, (BandClass) parameter);
    }

    public BikeDisplayMetricType ConvertBack(int value, BandClass deviceType)
    {
      if ((int)deviceType == 1)
      {
        switch (value)
        {
          case 1:
            return (BikeDisplayMetricType) 3;
          case 2:
            return (BikeDisplayMetricType) 2;
          case 3:
            return (BikeDisplayMetricType) 5;
          case 4:
            return (BikeDisplayMetricType) 4;
          default:
            return (BikeDisplayMetricType) 1;
        }
      }
      else
      {
        switch (value)
        {
          case 1:
            return (BikeDisplayMetricType) 3;
          case 2:
            return (BikeDisplayMetricType) 2;
          case 3:
            return (BikeDisplayMetricType) 7;
          case 4:
            return (BikeDisplayMetricType) 4;
          case 5:
            return (BikeDisplayMetricType) 5;
          case 6:
            return (BikeDisplayMetricType) 8;
          case 7:
            return (BikeDisplayMetricType) 6;
          default:
            return (BikeDisplayMetricType) 1;
        }
      }
    }
  }
}
