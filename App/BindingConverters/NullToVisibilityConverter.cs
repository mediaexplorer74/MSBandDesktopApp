// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.BindingConverters.NullToVisibilityConverter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DesktopSyncApp.BindingConverters
{
  [ValueConversion(typeof (object), typeof (Visibility))]
  public class NullToVisibilityConverter : IValueConverter
  {
    public NullToVisibilityConverter.Mapping DefaultMapping { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (object) this.Convert(value, parameter == null || !(parameter is NullToVisibilityConverter.Mapping mapping) ? this.DefaultMapping : mapping);

    public Visibility Convert(object value, NullToVisibilityConverter.Mapping parameter)
    {
      switch (parameter)
      {
        case NullToVisibilityConverter.Mapping.NullCollapsed:
          return value == null ? Visibility.Collapsed : Visibility.Visible;
        case NullToVisibilityConverter.Mapping.NotNullHidden:
          return value != null ? Visibility.Hidden : Visibility.Visible;
        case NullToVisibilityConverter.Mapping.NotNullCollapsed:
          return value != null ? Visibility.Collapsed : Visibility.Visible;
        default:
          return value == null ? Visibility.Hidden : Visibility.Visible;
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
      NullHidden,
      NullCollapsed,
      NotNullHidden,
      NotNullCollapsed,
    }
  }
}
