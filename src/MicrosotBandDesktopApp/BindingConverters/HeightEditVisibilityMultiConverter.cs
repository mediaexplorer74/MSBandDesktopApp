// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.HeightEditVisibilityMultiConverter
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
  public class HeightEditVisibilityMultiConverter : IMultiValueConverter
  {
    public static readonly Visibility FallbackVisibility;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] is bool && (bool) values[0] && values[1] is DistanceUnitType ? (object) this.Convert((DistanceUnitType) values[1], (DistanceUnitType) parameter) : (object) Visibility.Hidden;

    public Visibility Convert(DistanceUnitType displayUnits, DistanceUnitType param) => displayUnits == 1 || displayUnits != 2 ? (param != 1 ? Visibility.Hidden : Visibility.Visible) : (param != 2 ? Visibility.Hidden : Visibility.Visible);

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }
  }
}
