// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.BikeSplitComboBoxIndexConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (int), typeof (int))]
  public class BikeSplitComboBoxIndexConverter : IValueConverter
  {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (object)this.Convert((int)value);
        }

        public int Convert(int value)
        {
            switch (value)
            {
            case 2:
                return 1;
            case 3:
                return 2;
            case 4:
                return 3;
            case 5:
                return 4;
            case 10:
                return 5;
            case 15:
                return 6;
            case 20:
                return 7;
            case 25:
                return 8;
            case 50:
                return 9;
            default:
                return 0;
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
          return (object) this.ConvertBack((int) value);
        }

        public int ConvertBack(int value)
        {
          switch (value)
          {
            case 1:
              return 2;
            case 2:
              return 3;
            case 3:
              return 4;
            case 4:
              return 5;
            case 5:
              return 10;
            case 6:
              return 15;
            case 7:
              return 20;
            case 8:
              return 25;
            case 9:
              return 50;
            default:
              return 1;
          }
       }
  }
}
