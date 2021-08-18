// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.BoolToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (bool), typeof (Visibility))]
  public class BoolToVisibilityConverter : IValueConverter
  {
    public BoolToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert((bool) value, parameter == null || !(parameter is BoolToVisibilityConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public Visibility Convert(bool value, BoolToVisibilityConverter.Mapping parameter)
    {
      switch (parameter)
      {
        case BoolToVisibilityConverter.Mapping.FalseCollapsed:
          return !value ? Visibility.Collapsed : Visibility.Visible;
        case BoolToVisibilityConverter.Mapping.TrueHidden:
          return value ? Visibility.Hidden : Visibility.Visible;
        case BoolToVisibilityConverter.Mapping.TrueCollapsed:
          return value ? Visibility.Collapsed : Visibility.Visible;
        default:
          return !value ? Visibility.Hidden : Visibility.Visible;
      }
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException();
    }

    public enum Mapping
    {
      FalseHidden,
      FalseCollapsed,
      TrueHidden,
      TrueCollapsed,
    }
  }
}
