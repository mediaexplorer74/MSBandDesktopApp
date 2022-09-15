// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.WeightEditVisibilityMultiConverter
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
  public class WeightEditVisibilityMultiConverter : IMultiValueConverter
  {
    public static readonly Visibility FallbackVisibility;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is bool && (bool)values[0] && values[1] is MassUnitType 
                ? (object)this.Convert((MassUnitType)values[1], (MassUnitType)parameter) 
                : (object)Visibility.Hidden;
        }

        public Visibility Convert(MassUnitType displayUnits, MassUnitType param)
        {
            return (int)displayUnits == 1 || (int)displayUnits != 2 
                ? ((int)param != 1 ? Visibility.Hidden : Visibility.Visible) 
                : ((int)param != 2 ? Visibility.Hidden : Visibility.Visible);
        }

        public object[] ConvertBack
        (
          object value,
          Type[] targetTypes,
          object parameter,
          CultureInfo culture
        )
        {
          throw new InvalidOperationException();
        }
  }
}
